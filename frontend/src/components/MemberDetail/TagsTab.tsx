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
import { tagService } from '../../services/tagService'
import type { Tag } from '../../types/Tag'
import { ConfirmDialog } from '../common/ConfirmDialog'
import FormDatePicker from '../common/FormDatePicker'
import { formatIsoDateDdMmYyyyOrDash, todayIsoDateString } from '../../utils/dateFormat'
import { isTagActive } from '../../utils/tagActive'

interface TagsTabProps {
  memberId: number
  recordLocked?: boolean
}

interface TagFormValues {
  tagNumber: string
  assignedDate: string
  validFrom: string
  validTo: string
}

const stripDate = (value?: string): string => (value ? value.split('T')[0] : '')

const newTagDefaults = (): TagFormValues => {
  const d = todayIsoDateString()
  return {
    tagNumber: '',
    assignedDate: d,
    validFrom: d,
    validTo: d,
  }
}

const toFormValues = (tag: Tag): TagFormValues => ({
  tagNumber: tag.tagNumber != null ? String(tag.tagNumber) : '',
  assignedDate: stripDate(tag.assignedDate),
  validFrom: stripDate(tag.validFrom),
  validTo: stripDate(tag.validTo),
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

const TagsTab = ({ memberId, recordLocked = false }: TagsTabProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const queryKey = ['tags', 'byMember', memberId] as const

  const { data, isLoading, error } = useQuery({
    queryKey,
    queryFn: () => tagService.getByMember(memberId),
  })

  const [dialogOpen, setDialogOpen] = useState(false)
  const [editing, setEditing] = useState<Tag | null>(null)
  const [confirmDelete, setConfirmDelete] = useState<Tag | null>(null)

  const schema = yup.object({
    tagNumber: yup
      .string()
      .test('required', t('validation.required'), (v) => v != null && v !== '')
      .test('number', t('validation.required'), (v) => v == null || v === '' || !Number.isNaN(Number(v)))
      .default(''),
    assignedDate: yup.string().default(''),
    validFrom: yup.string().default(''),
    validTo: yup.string().default(''),
  })

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<TagFormValues>({
    resolver: yupResolver(schema) as never,
    defaultValues: newTagDefaults(),
  })

  useEffect(() => {
    if (dialogOpen) {
      reset(editing ? toFormValues(editing) : newTagDefaults())
    }
  }, [dialogOpen, editing, reset])

  const saveMutation = useMutation({
    mutationFn: async (values: TagFormValues) => {
      const payload = {
        memberId,
        tagNumber: numberOrUndef(values.tagNumber),
        assignedDate: emptyToUndef(values.assignedDate),
        validFrom: emptyToUndef(values.validFrom),
        validTo: emptyToUndef(values.validTo),
      }
      if (editing) {
        await tagService.update(editing.id, payload)
      } else {
        await tagService.create(payload)
      }
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setDialogOpen(false)
      setEditing(null)
    },
  })

  const deleteMutation = useMutation({
    mutationFn: (id: number) => tagService.delete(id),
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

  const handleEdit = (tag: Tag) => {
    setEditing(tag)
    setDialogOpen(true)
  }

  return (
    <Box>
      <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mb: 2 }}>
        <Typography variant="h6">{t('tags.title')}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          disabled={recordLocked}
        >
          {t('tags.addNew')}
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
                <TableCell>{t('tags.fields.tagNumber')}</TableCell>
                <TableCell>{t('tags.fields.assignedDate')}</TableCell>
                <TableCell>{t('tags.fields.validFrom')}</TableCell>
                <TableCell>{t('tags.fields.validTo')}</TableCell>
                <TableCell>{t('tags.active')}</TableCell>
                <TableCell align="right">{t('common.actions')}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data && data.length > 0 ? (
                data.map((tag) => {
                  const active = isTagActive(tag)
                  return (
                    <TableRow key={tag.id} hover>
                      <TableCell>{tag.tagNumber ?? '-'}</TableCell>
                      <TableCell>{formatIsoDateDdMmYyyyOrDash(tag.assignedDate)}</TableCell>
                      <TableCell>{formatIsoDateDdMmYyyyOrDash(tag.validFrom)}</TableCell>
                      <TableCell>{formatIsoDateDdMmYyyyOrDash(tag.validTo)}</TableCell>
                      <TableCell>
                        {active === null ? (
                          'N/A'
                        ) : (
                          <Chip
                            size="small"
                            label={active ? t('common.yes') : t('common.no')}
                            color={active ? 'success' : 'default'}
                          />
                        )}
                      </TableCell>
                      <TableCell align="right">
                        <IconButton
                          size="small"
                          onClick={() => handleEdit(tag)}
                          aria-label={t('common.edit')}
                          disabled={recordLocked}
                        >
                          <EditIcon fontSize="small" />
                        </IconButton>
                        <IconButton
                          size="small"
                          color="error"
                          onClick={() => setConfirmDelete(tag)}
                          aria-label={t('common.delete')}
                          disabled={recordLocked}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  )
                })
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
        <DialogTitle>{editing ? t('common.edit') : t('tags.addNew')}</DialogTitle>
        <Box component="form" onSubmit={onSubmit} noValidate>
          <DialogContent dividers>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('tags.fields.tagNumber')}
                  type="number"
                  fullWidth
                  required
                  {...register('tagNumber')}
                  error={!!errors.tagNumber}
                  helperText={errors.tagNumber?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <Controller
                  name="assignedDate"
                  control={control}
                  render={({ field }) => (
                    <FormDatePicker
                      label={t('tags.fields.assignedDate')}
                      value={field.value}
                      onChange={field.onChange}
                      fullWidth
                      size="medium"
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <Controller
                  name="validFrom"
                  control={control}
                  render={({ field }) => (
                    <FormDatePicker
                      label={t('tags.fields.validFrom')}
                      value={field.value}
                      onChange={field.onChange}
                      fullWidth
                      size="medium"
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <Controller
                  name="validTo"
                  control={control}
                  render={({ field }) => (
                    <FormDatePicker
                      label={t('tags.fields.validTo')}
                      value={field.value}
                      onChange={field.onChange}
                      fullWidth
                      size="medium"
                    />
                  )}
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

export default TagsTab
