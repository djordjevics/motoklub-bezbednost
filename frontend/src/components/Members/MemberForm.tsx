import { useEffect, useMemo } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
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
  MenuItem,
  TextField,
  Typography,
} from '@mui/material'
import { memberService } from '../../services/memberService'
import { lookupsService } from '../../services/lookupsService'
import type { Member } from '../../types/Member'
import type { MemberType } from '../../types/MemberType'
import FormDatePicker from '../common/FormDatePicker'
import { defaultMemberTypeId } from '../../utils/lookupDefaults'
import { todayIsoDateString } from '../../utils/dateFormat'

interface MemberFormProps {
  open: boolean
  memberId: number | null
  onClose: () => void
}

interface FormValues {
  name: string
  surname: string
  jmbg: string
  email: string
  dateOfBirth: string
  registeredOn: string
  workplace: string
  mobilePhone: string
  emergencyContact: string
  emergencyContactPhone: string
  address: string
  note: string
  memberTypeId: string
}

const defaultValues: FormValues = {
  name: '',
  surname: '',
  jmbg: '',
  email: '',
  dateOfBirth: '',
  registeredOn: '',
  workplace: '',
  mobilePhone: '',
  emergencyContact: '',
  emergencyContactPhone: '',
  address: '',
  note: '',
  memberTypeId: '',
}

const newMemberSeedValues = (): Pick<FormValues, 'dateOfBirth' | 'registeredOn'> => ({
  dateOfBirth: todayIsoDateString(),
  registeredOn: todayIsoDateString(),
})

const stripDate = (value?: string | null): string => {
  if (!value) return ''
  return value.split('T')[0]
}

const trimOrUndefined = (value: string): string | undefined => {
  const trimmed = value.trim()
  return trimmed === '' ? undefined : trimmed
}

const MemberForm = ({ open, memberId, onClose }: MemberFormProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const isEdit = memberId !== null

  const schema = useMemo(
    () =>
      yup.object({
        name: yup
          .string()
          .trim()
          .required(t('validation.required'))
          .max(100, t('validation.maxLength', { count: 100 })),
        surname: yup
          .string()
          .trim()
          .required(t('validation.required'))
          .max(100, t('validation.maxLength', { count: 100 })),
        jmbg: yup
          .string()
          .defined()
          .default('')
          .max(13, t('validation.maxLength', { count: 13 })),
        email: yup.string().defined().default('').email(t('validation.email')),
        dateOfBirth: yup.string().defined().default(''),
        registeredOn: yup.string().defined().default(''),
        workplace: yup.string().defined().default(''),
        mobilePhone: yup.string().defined().default(''),
        emergencyContact: yup.string().defined().default(''),
        emergencyContactPhone: yup.string().defined().default(''),
        address: yup.string().defined().default(''),
        note: yup.string().defined().default(''),
        memberTypeId: yup.string().defined().default(''),
      }),
    [t],
  )

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: yupResolver(schema as unknown as yup.ObjectSchema<FormValues>),
    defaultValues,
  })

  const { data: memberTypes } = useQuery<MemberType[]>({
    queryKey: ['memberTypes'],
    queryFn: lookupsService.getMemberTypes,
    enabled: open,
  })

  const { data: existingMember, isLoading: isLoadingMember } = useQuery<Member>({
    queryKey: ['member', memberId],
    queryFn: () => memberService.getById(memberId as number),
    enabled: open && isEdit,
  })

  useEffect(() => {
    if (!open) return
    if (isEdit && existingMember) {
      reset({
        name: existingMember.name ?? '',
        surname: existingMember.surname ?? '',
        jmbg: existingMember.jmbg ?? '',
        email: existingMember.email ?? '',
        dateOfBirth: stripDate(existingMember.dateOfBirth),
        registeredOn: stripDate(existingMember.registeredOn),
        workplace: existingMember.workplace ?? '',
        mobilePhone: existingMember.mobilePhone ?? '',
        emergencyContact: existingMember.emergencyContact ?? '',
        emergencyContactPhone: existingMember.emergencyContactPhone ?? '',
        address: existingMember.address ?? '',
        note: existingMember.note ?? '',
        memberTypeId:
          existingMember.memberTypeId != null ? String(existingMember.memberTypeId) : '',
      })
    } else if (!isEdit) {
      reset({ ...defaultValues, ...newMemberSeedValues() })
    }
  }, [open, isEdit, existingMember, reset])

  useEffect(() => {
    if (!open || isEdit || !memberTypes?.length) return
    const id = defaultMemberTypeId(memberTypes)
    if (id == null) return
    reset((form) => ({ ...form, memberTypeId: String(id) }))
  }, [open, isEdit, memberTypes, reset])

  const mutation = useMutation({
    mutationFn: async (values: FormValues) => {
      const payload: Partial<Member> = {
        name: values.name.trim(),
        surname: values.surname.trim(),
        jmbg: trimOrUndefined(values.jmbg),
        email: trimOrUndefined(values.email),
        dateOfBirth: trimOrUndefined(values.dateOfBirth),
        registeredOn: trimOrUndefined(values.registeredOn),
        workplace: trimOrUndefined(values.workplace),
        mobilePhone: trimOrUndefined(values.mobilePhone),
        emergencyContact: trimOrUndefined(values.emergencyContact),
        emergencyContactPhone: trimOrUndefined(values.emergencyContactPhone),
        address: trimOrUndefined(values.address),
        note: trimOrUndefined(values.note),
        memberTypeId:
          values.memberTypeId !== '' ? Number(values.memberTypeId) : undefined,
      }

      if (isEdit && memberId !== null) {
        // PUT is partial: omitting this would leave the flag unchanged in the API, but the dialog
        // must not accidentally depend on an older client cache — always send the stored value.
        if (existingMember) {
          payload.membershipExemptManual = existingMember.membershipExemptManual ?? false
        }
        await memberService.update(memberId, payload)
      } else {
        await memberService.create(payload as Omit<Member, 'id'>)
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['members'] })
      if (memberId !== null) {
        queryClient.invalidateQueries({ queryKey: ['member', memberId] })
      }
      onClose()
    },
  })

  const recordLockedInactive = Boolean(isEdit && existingMember && existingMember.membershipExemptManual)

  const onSubmit = handleSubmit((values) => {
    if (recordLockedInactive) return
    mutation.mutate(values)
  })

  const mutationError = mutation.error instanceof Error ? mutation.error.message : null

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>
        {t(isEdit ? 'members.form.editTitle' : 'members.form.createTitle')}
      </DialogTitle>
      <Box component="form" onSubmit={onSubmit} noValidate>
        <DialogContent dividers>
          {isEdit && isLoadingMember ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
              <CircularProgress />
            </Box>
          ) : (
            <Grid container spacing={2}>
              {recordLockedInactive ? (
                <Grid item xs={12}>
                  <Alert severity="warning">{t('members.memberFormInactiveReadonly')}</Alert>
                </Grid>
              ) : null}
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.name')}
                  required
                  disabled={recordLockedInactive}
                  {...register('name')}
                  error={Boolean(errors.name)}
                  helperText={errors.name?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.surname')}
                  required
                  disabled={recordLockedInactive}
                  {...register('surname')}
                  error={Boolean(errors.surname)}
                  helperText={errors.surname?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.jmbg')}
                  disabled={recordLockedInactive}
                  {...register('jmbg')}
                  error={Boolean(errors.jmbg)}
                  helperText={errors.jmbg?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <Controller
                  name="dateOfBirth"
                  control={control}
                  render={({ field }) => (
                    <FormDatePicker
                      label={t('members.fields.dateOfBirth')}
                      value={field.value}
                      onChange={field.onChange}
                      error={Boolean(errors.dateOfBirth)}
                      helperText={errors.dateOfBirth?.message}
                      fullWidth
                      disabled={recordLockedInactive}
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <Controller
                  name="memberTypeId"
                  control={control}
                  render={({ field }) => (
                    <TextField
                      select
                      fullWidth
                      size="small"
                      label={t('members.fields.memberType')}
                      {...field}
                      error={Boolean(errors.memberTypeId)}
                      helperText={errors.memberTypeId?.message}
                      disabled={recordLockedInactive || !memberTypes?.length}
                    >
                      {memberTypes?.map((mt) => (
                        <MenuItem key={mt.id} value={String(mt.id)}>
                          {mt.typeName ?? `#${mt.id}`}
                        </MenuItem>
                      ))}
                    </TextField>
                  )}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <Controller
                  name="registeredOn"
                  control={control}
                  render={({ field }) => (
                    <FormDatePicker
                      label={t('members.fields.registeredOn')}
                      value={field.value}
                      onChange={field.onChange}
                      error={Boolean(errors.registeredOn)}
                      helperText={errors.registeredOn?.message}
                      fullWidth
                      disabled={recordLockedInactive}
                    />
                  )}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.workplace')}
                  disabled={recordLockedInactive}
                  {...register('workplace')}
                  error={Boolean(errors.workplace)}
                  helperText={errors.workplace?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.mobilePhone')}
                  disabled={recordLockedInactive}
                  {...register('mobilePhone')}
                  error={Boolean(errors.mobilePhone)}
                  helperText={errors.mobilePhone?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.email')}
                  disabled={recordLockedInactive}
                  {...register('email')}
                  error={Boolean(errors.email)}
                  helperText={errors.email?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.address')}
                  disabled={recordLockedInactive}
                  {...register('address')}
                  error={Boolean(errors.address)}
                  helperText={errors.address?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.emergencyContact')}
                  disabled={recordLockedInactive}
                  {...register('emergencyContact')}
                  error={Boolean(errors.emergencyContact)}
                  helperText={errors.emergencyContact?.message}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  size="small"
                  label={t('members.fields.emergencyContactPhone')}
                  disabled={recordLockedInactive}
                  {...register('emergencyContactPhone')}
                  error={Boolean(errors.emergencyContactPhone)}
                  helperText={errors.emergencyContactPhone?.message}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  size="small"
                  multiline
                  minRows={2}
                  label={t('members.fields.note')}
                  disabled={recordLockedInactive}
                  {...register('note')}
                  error={Boolean(errors.note)}
                  helperText={errors.note?.message}
                />
              </Grid>
            </Grid>
          )}
        </DialogContent>
        <DialogActions sx={{ flexDirection: 'column', alignItems: 'stretch', gap: 1, p: 2 }}>
          {mutationError && (
            <Typography color="error" variant="body2">
              {mutationError}
            </Typography>
          )}
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 1 }}>
            <Button onClick={onClose} disabled={mutation.isPending}>
              {t('common.cancel')}
            </Button>
            {!recordLockedInactive ? (
              <Button type="submit" variant="contained" disabled={mutation.isPending}>
                {t('common.save')}
              </Button>
            ) : null}
          </Box>
        </DialogActions>
      </Box>
    </Dialog>
  )
}

export default MemberForm
