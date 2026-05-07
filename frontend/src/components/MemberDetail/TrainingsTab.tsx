import { useEffect, useState } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Box,
  Button,
  Chip,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControlLabel,
  Grid,
  IconButton,
  MenuItem,
  Paper,
  Stack,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from '@mui/material'
import AddIcon from '@mui/icons-material/Add'
import EditIcon from '@mui/icons-material/Edit'
import DeleteIcon from '@mui/icons-material/Delete'
import { useTranslation } from 'react-i18next'
import { trainingService } from '../../services/trainingService'
import { motorcycleService } from '../../services/motorcycleService'
import type { Training, TrainingSession } from '../../types/Training'
import { ConfirmDialog } from '../common/ConfirmDialog'
import { formatIsoDateDdMmYyyyOrDash } from '../../utils/dateFormat'
import { formatMotorcycleBrandModelPlate } from '../../utils/motorcycleLabel'

interface TrainingsTabProps {
  memberId: number
  recordLocked?: boolean
}

interface TrainingFormValues {
  motorcycleId: number | ''
  trainingSessionId: number | ''
  repeatingAttendance: boolean
  isCertificateIssued: boolean
  note: string
}

const emptyDefaults: TrainingFormValues = {
  motorcycleId: '',
  trainingSessionId: '',
  repeatingAttendance: false,
  isCertificateIssued: false,
  note: '',
}

const toFormValues = (tr: Training): TrainingFormValues => ({
  motorcycleId: tr.motorcycleId,
  trainingSessionId: tr.trainingSessionId,
  repeatingAttendance: tr.repeatingAttendance ?? false,
  isCertificateIssued: tr.isCertificateIssued,
  note: tr.note ?? '',
})

const formatSession = (s?: TrainingSession): string => {
  if (!s) return '-'
  const date = s.theoryDate ? formatIsoDateDdMmYyyyOrDash(s.theoryDate) : '-'
  const city = s.city ?? ''
  if (date !== '-' && city) return `${date} — ${city}`
  if (date !== '-') return date
  if (city) return city
  return `#${s.id}`
}

const emptyToUndef = (value: string): string | undefined => {
  const trimmed = value.trim()
  return trimmed === '' ? undefined : trimmed
}

const TrainingsTab = ({ memberId, recordLocked = false }: TrainingsTabProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const queryKey = ['trainings', 'byMember', memberId] as const

  const { data: trainings, isLoading, error } = useQuery({
    queryKey,
    queryFn: () => trainingService.getTrainingsByMember(memberId),
  })

  const { data: sessions } = useQuery({
    queryKey: ['trainingSessions'],
    queryFn: trainingService.getAllSessions,
  })

  const { data: motorcycles } = useQuery({
    queryKey: ['motorcycles', 'byMember', memberId],
    queryFn: () => motorcycleService.getByMemberId(memberId),
  })

  const [dialogOpen, setDialogOpen] = useState(false)
  const [editing, setEditing] = useState<Training | null>(null)
  const [confirmDelete, setConfirmDelete] = useState<Training | null>(null)

  const schema = yup.object({
    motorcycleId: yup
      .mixed<number | ''>()
      .test('required', t('validation.required'), (v) => v !== '' && v != null)
      .default(''),
    trainingSessionId: yup
      .mixed<number | ''>()
      .test('required', t('validation.required'), (v) => v !== '' && v != null)
      .default(''),
    repeatingAttendance: yup.boolean().default(false),
    isCertificateIssued: yup.boolean().default(false),
    note: yup.string().trim().max(2000, t('validation.maxLength', { count: 2000 })).default(''),
  })

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<TrainingFormValues>({
    resolver: yupResolver(schema) as never,
    defaultValues: emptyDefaults,
  })

  useEffect(() => {
    if (dialogOpen) {
      reset(editing ? toFormValues(editing) : emptyDefaults)
    }
  }, [dialogOpen, editing, reset])

  const saveMutation = useMutation({
    mutationFn: async (values: TrainingFormValues) => {
      const payload = {
        memberId,
        motorcycleId: Number(values.motorcycleId),
        trainingSessionId: Number(values.trainingSessionId),
        repeatingAttendance: values.repeatingAttendance,
        isCertificateIssued: values.isCertificateIssued,
        note: emptyToUndef(values.note),
      }
      if (editing) {
        await trainingService.updateTraining(editing.id, {
          motorcycleId: Number(values.motorcycleId),
          repeatingAttendance: values.repeatingAttendance,
          isCertificateIssued: values.isCertificateIssued,
          note: emptyToUndef(values.note),
        })
      } else {
        await trainingService.createTraining(payload)
      }
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setDialogOpen(false)
      setEditing(null)
    },
  })

  const deleteMutation = useMutation({
    mutationFn: (id: number) => trainingService.deleteTraining(id),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setConfirmDelete(null)
    },
  })

  const onSubmit = handleSubmit((values) => {
    saveMutation.mutate(values)
  })

  const handleAdd = () => {
    setEditing(null)
    setDialogOpen(true)
  }

  const handleEdit = (tr: Training) => {
    setEditing(tr)
    setDialogOpen(true)
  }

  const memberMotorcycles = motorcycles ?? []

  return (
    <Box>
      <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mb: 2 }}>
        <Typography variant="h6">{t('trainings.memberTitle')}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          disabled={recordLocked}
        >
          {t('trainings.addNew')}
        </Button>
      </Stack>

      {isLoading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
          <CircularProgress />
        </Box>
      ) : error ? (
        <Typography color="error">{t('common.error')}</Typography>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>{t('trainings.fields.session')}</TableCell>
                <TableCell>{t('trainings.fields.motorcycle')}</TableCell>
                <TableCell>{t('trainings.fields.repeatingAttendance')}</TableCell>
                <TableCell>{t('trainings.fields.certificate')}</TableCell>
                <TableCell>{t('trainings.fields.note')}</TableCell>
                <TableCell align="right">{t('common.actions')}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {trainings && trainings.length > 0 ? (
                trainings.map((tr) => (
                  <TableRow key={tr.id} hover>
                    <TableCell>{formatSession(tr.trainingSession)}</TableCell>
                    <TableCell>{formatMotorcycleBrandModelPlate(tr.motorcycle)}</TableCell>
                    <TableCell>
                      <Chip
                        size="small"
                        label={tr.repeatingAttendance ? t('common.yes') : t('common.no')}
                        color={tr.repeatingAttendance ? 'info' : 'default'}
                        variant={tr.repeatingAttendance ? 'filled' : 'outlined'}
                      />
                    </TableCell>
                    <TableCell>
                      <Chip
                        size="small"
                        label={tr.isCertificateIssued ? t('common.yes') : t('common.no')}
                        color={tr.isCertificateIssued ? 'success' : 'default'}
                        variant={tr.isCertificateIssued ? 'filled' : 'outlined'}
                      />
                    </TableCell>
                    <TableCell>{tr.note ?? '-'}</TableCell>
                    <TableCell align="right">
                      <IconButton
                        size="small"
                        onClick={() => handleEdit(tr)}
                        aria-label={t('common.edit')}
                        disabled={recordLocked}
                      >
                        <EditIcon fontSize="small" />
                      </IconButton>
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => setConfirmDelete(tr)}
                        aria-label={t('common.delete')}
                        disabled={recordLocked}
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={6} align="center">
                    <Typography variant="body2" color="text.secondary" sx={{ py: 2 }}>
                      {t('common.noData')}
                    </Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)} maxWidth="sm" fullWidth>
        <DialogTitle>{editing ? t('common.edit') : t('trainings.addNew')}</DialogTitle>
        <Box component="form" onSubmit={onSubmit} noValidate>
          <DialogContent dividers>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <Controller
                  name="trainingSessionId"
                  control={control}
                  render={({ field }) => (
                    <TextField
                      select
                      label={t('trainings.fields.session')}
                      fullWidth
                      required
                      value={field.value === '' ? '' : String(field.value)}
                      onChange={(e) => {
                        const v = e.target.value
                        field.onChange(v === '' ? '' : Number(v))
                      }}
                      error={!!errors.trainingSessionId}
                      helperText={errors.trainingSessionId?.message as string | undefined}
                    >
                      <MenuItem value="">
                        <em>{t('common.select')}</em>
                      </MenuItem>
                      {sessions?.map((s) => (
                        <MenuItem key={s.id} value={String(s.id)}>
                          {formatSession(s)}
                        </MenuItem>
                      ))}
                    </TextField>
                  )}
                />
              </Grid>
              <Grid item xs={12}>
                <Controller
                  name="motorcycleId"
                  control={control}
                  render={({ field }) => (
                    <TextField
                      select
                      label={t('trainings.fields.motorcycle')}
                      fullWidth
                      required
                      value={field.value === '' ? '' : String(field.value)}
                      onChange={(e) => {
                        const v = e.target.value
                        field.onChange(v === '' ? '' : Number(v))
                      }}
                      error={!!errors.motorcycleId}
                      helperText={errors.motorcycleId?.message as string | undefined}
                    >
                      <MenuItem value="">
                        <em>{t('common.select')}</em>
                      </MenuItem>
                      {memberMotorcycles.map((m) => (
                        <MenuItem key={m.id} value={String(m.id)}>
                          {formatMotorcycleBrandModelPlate(m)}
                        </MenuItem>
                      ))}
                    </TextField>
                  )}
                />
              </Grid>
              <Grid item xs={12}>
                <Controller
                  name="repeatingAttendance"
                  control={control}
                  render={({ field }) => (
                    <FormControlLabel
                      control={
                        <Switch
                          checked={field.value}
                          onChange={(e) => field.onChange(e.target.checked)}
                        />
                      }
                      label={t('trainings.fields.repeatingAttendance')}
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12}>
                <Controller
                  name="isCertificateIssued"
                  control={control}
                  render={({ field }) => (
                    <FormControlLabel
                      control={
                        <Switch
                          checked={field.value}
                          onChange={(e) => field.onChange(e.target.checked)}
                        />
                      }
                      label={t('trainings.fields.isCertificateIssued')}
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  label={t('trainings.fields.note')}
                  fullWidth
                  multiline
                  minRows={3}
                  {...register('note')}
                  error={!!errors.note}
                  helperText={errors.note?.message}
                />
              </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setDialogOpen(false)}>{t('common.cancel')}</Button>
            <Button type="submit" variant="contained" disabled={saveMutation.isPending}>
              {t('common.save')}
            </Button>
          </DialogActions>
        </Box>
      </Dialog>

      <ConfirmDialog
        open={!!confirmDelete}
        title={t('common.delete')}
        destructive
        confirmText={t('common.delete')}
        onConfirm={() => confirmDelete && deleteMutation.mutate(confirmDelete.id)}
        onCancel={() => setConfirmDelete(null)}
      />
    </Box>
  )
}

export default TrainingsTab
