import { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
  IconButton,
  Paper,
  Stack,
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
import { motorcycleService } from '../../services/motorcycleService'
import type { Motorcycle } from '../../types/Member'
import { ConfirmDialog } from '../common/ConfirmDialog'

interface MotorcyclesTabProps {
  memberId: number
  recordLocked?: boolean
}

interface MotorcycleFormValues {
  brandName: string
  commercialName: string
  modelName: string
  engineDisplacment: string
  enginePower: string
  color: string
  registerPlate: string
}

const emptyDefaults: MotorcycleFormValues = {
  brandName: '',
  commercialName: '',
  modelName: '',
  engineDisplacment: '',
  enginePower: '',
  color: '',
  registerPlate: '',
}

const toFormValues = (m: Motorcycle): MotorcycleFormValues => ({
  brandName: m.brandName ?? '',
  commercialName: m.commercialName ?? '',
  modelName: m.modelName ?? '',
  engineDisplacment: m.engineDisplacment != null ? String(m.engineDisplacment) : '',
  enginePower: m.enginePower != null ? String(m.enginePower) : '',
  color: m.color ?? '',
  registerPlate: m.registerPlate ?? '',
})

const emptyToUndef = (value: string): string | undefined => {
  const trimmed = value.trim()
  return trimmed === '' ? undefined : trimmed
}

const numberOrUndef = (value: string): number | undefined => {
  const trimmed = value.trim()
  if (trimmed === '') return undefined
  const n = Number(trimmed)
  return Number.isFinite(n) ? n : undefined
}

const MotorcyclesTab = ({ memberId, recordLocked = false }: MotorcyclesTabProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const queryKey = ['motorcycles', 'byMember', memberId] as const

  const { data, isLoading, error } = useQuery({
    queryKey,
    queryFn: () => motorcycleService.getByMemberId(memberId),
  })

  const [dialogOpen, setDialogOpen] = useState(false)
  const [editing, setEditing] = useState<Motorcycle | null>(null)
  const [confirmDelete, setConfirmDelete] = useState<Motorcycle | null>(null)

  const schema = yup.object({
    brandName: yup
      .string()
      .trim()
      .required(t('validation.required'))
      .max(100, t('validation.maxLength', { count: 100 })),
    commercialName: yup.string().trim().max(100, t('validation.maxLength', { count: 100 })).default(''),
    modelName: yup.string().trim().max(100, t('validation.maxLength', { count: 100 })).default(''),
    engineDisplacment: yup
      .string()
      .test('number', t('validation.required'), (v) => v === undefined || v === '' || !Number.isNaN(Number(v)))
      .default(''),
    enginePower: yup
      .string()
      .test('number', t('validation.required'), (v) => v === undefined || v === '' || !Number.isNaN(Number(v)))
      .default(''),
    color: yup.string().trim().max(50, t('validation.maxLength', { count: 50 })).default(''),
    registerPlate: yup.string().trim().max(20, t('validation.maxLength', { count: 20 })).default(''),
  })

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<MotorcycleFormValues>({
    resolver: yupResolver(schema) as never,
    defaultValues: emptyDefaults,
  })

  useEffect(() => {
    if (dialogOpen) {
      reset(editing ? toFormValues(editing) : emptyDefaults)
    }
  }, [dialogOpen, editing, reset])

  const saveMutation = useMutation({
    mutationFn: async (values: MotorcycleFormValues) => {
      const payload = {
        brandName: values.brandName.trim(),
        commercialName: emptyToUndef(values.commercialName),
        modelName: emptyToUndef(values.modelName),
        engineDisplacment: numberOrUndef(values.engineDisplacment),
        enginePower: numberOrUndef(values.enginePower),
        color: emptyToUndef(values.color),
        registerPlate: emptyToUndef(values.registerPlate),
      }
      if (editing) {
        await motorcycleService.update(editing.id, payload)
      } else {
        await motorcycleService.create({ ...payload, memberId })
      }
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setDialogOpen(false)
      setEditing(null)
    },
  })

  const deleteMutation = useMutation({
    mutationFn: (id: number) => motorcycleService.delete(id),
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

  const handleEdit = (m: Motorcycle) => {
    setEditing(m)
    setDialogOpen(true)
  }

  return (
    <Box>
      <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mb: 2 }}>
        <Typography variant="h6">{t('motorcycles.title')}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          disabled={recordLocked}
        >
          {t('motorcycles.addNew')}
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
                <TableCell>{t('motorcycles.fields.brandName')}</TableCell>
                <TableCell>{t('motorcycles.fields.commercialName')}</TableCell>
                <TableCell>{t('motorcycles.fields.modelName')}</TableCell>
                <TableCell>{t('motorcycles.fields.engineDisplacment')}</TableCell>
                <TableCell>{t('motorcycles.fields.enginePower')}</TableCell>
                <TableCell>{t('motorcycles.fields.color')}</TableCell>
                <TableCell>{t('motorcycles.fields.registerPlate')}</TableCell>
                <TableCell align="right">{t('common.actions')}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data && data.length > 0 ? (
                data.map((m) => (
                  <TableRow key={m.id} hover>
                    <TableCell>{m.brandName ?? '-'}</TableCell>
                    <TableCell>{m.commercialName ?? '-'}</TableCell>
                    <TableCell>{m.modelName ?? '-'}</TableCell>
                    <TableCell>{m.engineDisplacment ?? '-'}</TableCell>
                    <TableCell>{m.enginePower ?? '-'}</TableCell>
                    <TableCell>{m.color ?? '-'}</TableCell>
                    <TableCell>{m.registerPlate ?? '-'}</TableCell>
                    <TableCell align="right">
                      <IconButton
                        size="small"
                        onClick={() => handleEdit(m)}
                        aria-label={t('common.edit')}
                        disabled={recordLocked}
                      >
                        <EditIcon fontSize="small" />
                      </IconButton>
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => setConfirmDelete(m)}
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
                  <TableCell colSpan={8} align="center">
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

      <Dialog
        open={dialogOpen}
        onClose={() => setDialogOpen(false)}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>{editing ? t('common.edit') : t('motorcycles.addNew')}</DialogTitle>
        <Box component="form" onSubmit={onSubmit} noValidate>
          <DialogContent dividers>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.brandName')}
                  fullWidth
                  required
                  {...register('brandName')}
                  error={!!errors.brandName}
                  helperText={errors.brandName?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.commercialName')}
                  fullWidth
                  {...register('commercialName')}
                  error={!!errors.commercialName}
                  helperText={errors.commercialName?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.modelName')}
                  fullWidth
                  {...register('modelName')}
                  error={!!errors.modelName}
                  helperText={errors.modelName?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.engineDisplacment')}
                  type="number"
                  fullWidth
                  {...register('engineDisplacment')}
                  error={!!errors.engineDisplacment}
                  helperText={errors.engineDisplacment?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.enginePower')}
                  type="number"
                  fullWidth
                  {...register('enginePower')}
                  error={!!errors.enginePower}
                  helperText={errors.enginePower?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.color')}
                  fullWidth
                  {...register('color')}
                  error={!!errors.color}
                  helperText={errors.color?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('motorcycles.fields.registerPlate')}
                  fullWidth
                  {...register('registerPlate')}
                  error={!!errors.registerPlate}
                  helperText={errors.registerPlate?.message}
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
        title={t('motorcycles.delete.title')}
        message={t('motorcycles.delete.message', {
          name: confirmDelete?.brandName ?? '',
        })}
        destructive
        confirmText={t('common.delete')}
        onConfirm={() => confirmDelete && deleteMutation.mutate(confirmDelete.id)}
        onCancel={() => setConfirmDelete(null)}
      />
    </Box>
  )
}

export default MotorcyclesTab
