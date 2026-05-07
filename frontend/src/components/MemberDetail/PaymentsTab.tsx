import { useEffect, useState } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
  IconButton,
  MenuItem,
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
import { membershipPaymentService } from '../../services/membershipPaymentService'
import { lookupsService } from '../../services/lookupsService'
import type { MembershipPayment } from '../../types/MembershipPayment'
import { ConfirmDialog } from '../common/ConfirmDialog'
import FormDatePicker from '../common/FormDatePicker'
import { formatIsoDateDdMmYyyyOrDash, todayIsoDateString } from '../../utils/dateFormat'
import { defaultPaymentTypeId } from '../../utils/lookupDefaults'

interface PaymentsTabProps {
  memberId: number
  isInactiveMemberRecord: boolean
  isMembershipPaymentRequired: boolean
  isMembershipPaymentExemptDueToAge: boolean
  memberTypePaysMembership: boolean
}

interface PaymentFormValues {
  amount: string
  paymentDate: string
  paymentForYear: string
  paymentTypeId: number | ''
  note: string
}

const buildEmptyDefaults = (): PaymentFormValues => ({
  amount: '',
  paymentDate: todayIsoDateString(),
  paymentForYear: String(new Date().getFullYear()),
  paymentTypeId: '',
  note: '',
})

const toFormValues = (p: MembershipPayment): PaymentFormValues => ({
  amount: p.amount != null ? String(p.amount) : '',
  paymentDate: p.paymentDate ? p.paymentDate.split('T')[0] : '',
  paymentForYear: p.paymentForYear != null ? String(p.paymentForYear) : '',
  paymentTypeId: p.paymentTypeId ?? '',
  note: p.note ?? '',
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

const amountFormatter = new Intl.NumberFormat()

const PaymentsTab = (props: PaymentsTabProps) => {
  const { memberId, isInactiveMemberRecord, isMembershipPaymentExemptDueToAge, memberTypePaysMembership } =
    props
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const queryKey = ['payments', 'byMember', memberId] as const

  const { data, isLoading, error } = useQuery({
    queryKey,
    queryFn: () => membershipPaymentService.getByMember(memberId),
  })

  const { data: paymentTypes } = useQuery({
    queryKey: ['paymentTypes'],
    queryFn: lookupsService.getPaymentTypes,
  })

  const [dialogOpen, setDialogOpen] = useState(false)
  const [editing, setEditing] = useState<MembershipPayment | null>(null)
  const [confirmDelete, setConfirmDelete] = useState<MembershipPayment | null>(null)

  const schema = yup.object({
    amount: yup
      .string()
      .test('number', t('validation.required'), (v) => v === undefined || v === '' || !Number.isNaN(Number(v)))
      .default(''),
    paymentDate: yup.string().default(''),
    paymentForYear: yup
      .string()
      .test('number', t('validation.required'), (v) => v === undefined || v === '' || !Number.isNaN(Number(v)))
      .default(''),
    paymentTypeId: yup
      .mixed<number | ''>()
      .test('paymentTypeId', '', () => true)
      .default(''),
    note: yup.string().trim().max(2000, t('validation.maxLength', { count: 2000 })).default(''),
  })

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<PaymentFormValues>({
    resolver: yupResolver(schema) as never,
    defaultValues: buildEmptyDefaults(),
  })

  useEffect(() => {
    if (!dialogOpen) {
      return
    }
    if (editing) {
      reset(toFormValues(editing))
      return
    }
    reset({
      ...buildEmptyDefaults(),
      paymentTypeId: defaultPaymentTypeId(paymentTypes) ?? '',
    })
  }, [dialogOpen, editing, paymentTypes, reset])

  const saveMutation = useMutation({
    mutationFn: async (values: PaymentFormValues) => {
      const typeId =
        values.paymentTypeId === '' ? defaultPaymentTypeId(paymentTypes) : Number(values.paymentTypeId)
      const payload = {
        memberId,
        amount: numberOrUndef(values.amount),
        paymentDate: emptyToUndef(values.paymentDate),
        paymentForYear: numberOrUndef(values.paymentForYear),
        paymentTypeId: typeId,
        note: emptyToUndef(values.note),
      }
      if (editing) {
        await membershipPaymentService.update(editing.id, payload)
      } else {
        await membershipPaymentService.create(payload)
      }
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setDialogOpen(false)
      setEditing(null)
    },
  })

  const deleteMutation = useMutation({
    mutationFn: (id: number) => membershipPaymentService.delete(id),
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

  const handleEdit = (p: MembershipPayment) => {
    setEditing(p)
    setDialogOpen(true)
  }

  return (
    <Box>
      {isInactiveMemberRecord && (
        <Alert severity="warning" sx={{ mb: 2 }}>
          {t('payments.exemptInactiveNotice')}
        </Alert>
      )}
      {!isInactiveMemberRecord && memberTypePaysMembership && isMembershipPaymentExemptDueToAge && (
        <Alert severity="info" sx={{ mb: 2 }}>
          {t('payments.exemptAgeNotice')}
        </Alert>
      )}
      {!memberTypePaysMembership && (
        <Alert severity="info" sx={{ mb: 2 }}>
          {t('payments.nonPayerTypeNotice')}
        </Alert>
      )}

      <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mb: 2 }}>
        <Typography variant="h6">{t('payments.title')}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          disabled={isInactiveMemberRecord}
        >
          {t('payments.addNew')}
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
                <TableCell>{t('payments.fields.paymentDate')}</TableCell>
                <TableCell>{t('payments.fields.paymentForYear')}</TableCell>
                <TableCell>{t('payments.fields.amount')}</TableCell>
                <TableCell>{t('payments.fields.paymentType')}</TableCell>
                <TableCell>{t('payments.fields.note')}</TableCell>
                <TableCell align="right">{t('common.actions')}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data && data.length > 0 ? (
                data.map((p) => (
                  <TableRow key={p.id} hover>
                    <TableCell>{formatIsoDateDdMmYyyyOrDash(p.paymentDate)}</TableCell>
                    <TableCell>{p.paymentForYear ?? '-'}</TableCell>
                    <TableCell>
                      {p.amount != null
                        ? `${amountFormatter.format(p.amount)} ${t('payments.currency')}`
                        : '-'}
                    </TableCell>
                    <TableCell>{p.paymentType?.type ?? '-'}</TableCell>
                    <TableCell>{p.note ?? '-'}</TableCell>
                    <TableCell align="right">
                      <IconButton
                        size="small"
                        onClick={() => handleEdit(p)}
                        aria-label={t('common.edit')}
                        disabled={isInactiveMemberRecord}
                      >
                        <EditIcon fontSize="small" />
                      </IconButton>
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => setConfirmDelete(p)}
                        aria-label={t('common.delete')}
                        disabled={isInactiveMemberRecord}
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
        <DialogTitle>{editing ? t('common.edit') : t('payments.addNew')}</DialogTitle>
        <Box component="form" onSubmit={onSubmit} noValidate>
          <DialogContent dividers>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('payments.fields.amount')}
                  type="number"
                  fullWidth
                  {...register('amount')}
                  error={!!errors.amount}
                  helperText={errors.amount?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <Controller
                  name="paymentDate"
                  control={control}
                  render={({ field }) => (
                    <FormDatePicker
                      label={t('payments.fields.paymentDate')}
                      value={field.value}
                      onChange={field.onChange}
                      error={!!errors.paymentDate}
                      helperText={errors.paymentDate?.message}
                      fullWidth
                      size="medium"
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  label={t('payments.fields.paymentForYear')}
                  type="number"
                  fullWidth
                  {...register('paymentForYear')}
                  error={!!errors.paymentForYear}
                  helperText={errors.paymentForYear?.message}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <Controller
                  name="paymentTypeId"
                  control={control}
                  render={({ field }) => (
                    <TextField
                      select
                      label={t('payments.fields.paymentType')}
                      fullWidth
                      disabled={!paymentTypes?.length}
                      value={
                        field.value === '' && defaultPaymentTypeId(paymentTypes) != null
                          ? String(defaultPaymentTypeId(paymentTypes))
                          : field.value === ''
                            ? ''
                            : String(field.value)
                      }
                      onChange={(e) => {
                        field.onChange(Number(e.target.value))
                      }}
                    >
                      {paymentTypes?.map((pt) => (
                        <MenuItem key={pt.id} value={String(pt.id)}>
                          {pt.type}
                        </MenuItem>
                      ))}
                    </TextField>
                  )}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  label={t('payments.fields.note')}
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

export default PaymentsTab
