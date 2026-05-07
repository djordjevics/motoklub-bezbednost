import { useEffect } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Box,
  Button,
  FormControlLabel,
  FormHelperText,
  Grid,
  MenuItem,
  Paper,
  Switch,
  TextField,
} from '@mui/material'
import { useTranslation } from 'react-i18next'
import { memberService } from '../../services/memberService'
import { lookupsService } from '../../services/lookupsService'
import type { Member } from '../../types/Member'
import FormDatePicker from '../common/FormDatePicker'
import { defaultMemberTypeId } from '../../utils/lookupDefaults'

interface ProfileTabProps {
  memberId: number
  member: Member
}

interface ProfileFormValues {
  name: string
  surname: string
  jmbg: string
  dateOfBirth: string
  workplace: string
  mobilePhone: string
  email: string
  address: string
  emergencyContact: string
  emergencyContactPhone: string
  registeredOn: string
  memberTypeId: number | ''
  membershipExemptManual: boolean
  note: string
}

const stripDate = (value?: string): string => (value ? value.split('T')[0] : '')

const toFormValues = (member: Member): ProfileFormValues => ({
  name: member.name ?? '',
  surname: member.surname ?? '',
  jmbg: member.jmbg ?? '',
  dateOfBirth: stripDate(member.dateOfBirth),
  workplace: member.workplace ?? '',
  mobilePhone: member.mobilePhone ?? '',
  email: member.email ?? '',
  address: member.address ?? '',
  emergencyContact: member.emergencyContact ?? '',
  emergencyContactPhone: member.emergencyContactPhone ?? '',
  registeredOn: stripDate(member.registeredOn),
  memberTypeId: member.memberTypeId ?? '',
  membershipExemptManual: member.membershipExemptManual ?? false,
  note: member.note ?? '',
})

const emptyToUndef = (value: string): string | undefined => {
  const trimmed = value.trim()
  return trimmed === '' ? undefined : trimmed
}

const ProfileTab = ({ memberId, member }: ProfileTabProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const inactiveRecord = Boolean(member.membershipExemptManual)
  const typePaysMembership = member.memberType ? member.memberType.paidMembership !== false : true
  const ageExempt = Boolean(member.isMembershipPaymentExemptDueToAge)

  const schema = yup.object({
    name: yup.string().trim().required(t('validation.required')).max(100, t('validation.maxLength', { count: 100 })),
    surname: yup.string().trim().required(t('validation.required')).max(100, t('validation.maxLength', { count: 100 })),
    jmbg: yup.string().trim().max(20, t('validation.maxLength', { count: 20 })).default(''),
    dateOfBirth: yup.string().default(''),
    workplace: yup.string().trim().max(200, t('validation.maxLength', { count: 200 })).default(''),
    mobilePhone: yup.string().trim().max(50, t('validation.maxLength', { count: 50 })).default(''),
    email: yup
      .string()
      .trim()
      .max(200, t('validation.maxLength', { count: 200 }))
      .test('email', t('validation.email'), (value) => {
        if (!value || value.trim() === '') return true
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)
      })
      .default(''),
    address: yup.string().trim().max(300, t('validation.maxLength', { count: 300 })).default(''),
    emergencyContact: yup
      .string()
      .trim()
      .max(200, t('validation.maxLength', { count: 200 }))
      .default(''),
    emergencyContactPhone: yup
      .string()
      .trim()
      .max(50, t('validation.maxLength', { count: 50 }))
      .default(''),
    registeredOn: yup.string().default(''),
    memberTypeId: yup
      .mixed<number | ''>()
      .test('memberTypeId', '', () => true)
      .default(''),
    membershipExemptManual: yup.boolean().default(false),
    note: yup.string().trim().max(2000, t('validation.maxLength', { count: 2000 })).default(''),
  })

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ProfileFormValues>({
    resolver: yupResolver(schema) as never,
    defaultValues: toFormValues(member),
    shouldUnregister: false,
  })

  const { data: memberTypes } = useQuery({
    queryKey: ['memberTypes'],
    queryFn: lookupsService.getMemberTypes,
  })

  useEffect(() => {
    reset(
      {
        ...toFormValues(member),
        // Avoid resetting the whole form when lookup data arrives/refetches.
        // This tab is for editing an existing member; keep the member-provided value.
        memberTypeId: member.memberTypeId != null ? member.memberTypeId : '',
      },
      // Refetches (e.g. after toggling exempt) can briefly carry stale data; do not clobber in-progress edits.
      { keepDirtyValues: true },
    )
  }, [member, reset])

  const exemptOnlyMutation = useMutation({
    mutationFn: (membershipExemptManual: boolean) => memberService.update(memberId, { membershipExemptManual }),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: ['member', memberId] })
      void queryClient.invalidateQueries({ queryKey: ['members'] })
    },
  })

  const updateMutation = useMutation({
    mutationFn: (values: ProfileFormValues) =>
      memberService.update(memberId, {
        name: values.name.trim(),
        surname: values.surname.trim(),
        jmbg: emptyToUndef(values.jmbg),
        dateOfBirth: emptyToUndef(values.dateOfBirth),
        workplace: emptyToUndef(values.workplace),
        mobilePhone: emptyToUndef(values.mobilePhone),
        email: emptyToUndef(values.email),
        address: emptyToUndef(values.address),
        emergencyContact: emptyToUndef(values.emergencyContact),
        emergencyContactPhone: emptyToUndef(values.emergencyContactPhone),
        registeredOn: emptyToUndef(values.registeredOn),
        memberTypeId:
          values.memberTypeId === ''
            ? defaultMemberTypeId(memberTypes)
            : Number(values.memberTypeId),
        membershipExemptManual: values.membershipExemptManual,
        note: emptyToUndef(values.note),
      }),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: ['member', memberId] })
      void queryClient.invalidateQueries({ queryKey: ['members'] })
    },
  })

  const onSubmit = handleSubmit((values) => {
    if (inactiveRecord) return
    updateMutation.mutate(values)
  })

  const fieldDisabled = inactiveRecord

  return (
    <Paper sx={{ p: 3 }}>
      <Box component="form" onSubmit={onSubmit} noValidate>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.name')}
              fullWidth
              required
              disabled={fieldDisabled}
              {...register('name')}
              error={!!errors.name}
              helperText={errors.name?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.surname')}
              fullWidth
              required
              disabled={fieldDisabled}
              {...register('surname')}
              error={!!errors.surname}
              helperText={errors.surname?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.jmbg')}
              fullWidth
              disabled={fieldDisabled}
              {...register('jmbg')}
              error={!!errors.jmbg}
              helperText={errors.jmbg?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="dateOfBirth"
              control={control}
              render={({ field }) => (
                <FormDatePicker
                  label={t('members.fields.dateOfBirth')}
                  value={field.value}
                  onChange={field.onChange}
                  error={!!errors.dateOfBirth}
                  helperText={errors.dateOfBirth?.message}
                  fullWidth
                  size="medium"
                  disabled={fieldDisabled}
                />
              )}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.workplace')}
              fullWidth
              disabled={fieldDisabled}
              {...register('workplace')}
              error={!!errors.workplace}
              helperText={errors.workplace?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.mobilePhone')}
              fullWidth
              disabled={fieldDisabled}
              {...register('mobilePhone')}
              error={!!errors.mobilePhone}
              helperText={errors.mobilePhone?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.email')}
              fullWidth
              disabled={fieldDisabled}
              {...register('email')}
              error={!!errors.email}
              helperText={errors.email?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.address')}
              fullWidth
              disabled={fieldDisabled}
              {...register('address')}
              error={!!errors.address}
              helperText={errors.address?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.emergencyContact')}
              fullWidth
              disabled={fieldDisabled}
              {...register('emergencyContact')}
              error={!!errors.emergencyContact}
              helperText={errors.emergencyContact?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              label={t('members.fields.emergencyContactPhone')}
              fullWidth
              disabled={fieldDisabled}
              {...register('emergencyContactPhone')}
              error={!!errors.emergencyContactPhone}
              helperText={errors.emergencyContactPhone?.message}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="registeredOn"
              control={control}
              render={({ field }) => (
                <FormDatePicker
                  label={t('members.fields.registeredOn')}
                  value={field.value}
                  onChange={field.onChange}
                  error={!!errors.registeredOn}
                  helperText={errors.registeredOn?.message}
                  fullWidth
                  size="medium"
                  disabled={fieldDisabled}
                />
              )}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="memberTypeId"
              control={control}
              render={({ field }) => (
                <TextField
                  select
                  label={t('members.fields.memberType')}
                  fullWidth
                  disabled={fieldDisabled || !memberTypes?.length}
                  value={field.value === '' ? '' : String(field.value)}
                  onChange={(e) => {
                    field.onChange(Number(e.target.value))
                  }}
                >
                  {memberTypes?.map((mt) => (
                    <MenuItem key={mt.id} value={String(mt.id)}>
                      {mt.typeName}
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />
          </Grid>
          <Grid item xs={12}>
            <Controller
              name="membershipExemptManual"
              control={control}
              render={({ field }) => (
                <Box>
                  <FormControlLabel
                    control={
                      <Switch
                        checked={field.value}
                        disabled={exemptOnlyMutation.isPending || updateMutation.isPending}
                        onChange={(e) => {
                          const v = e.target.checked
                          field.onChange(v)
                          exemptOnlyMutation.mutate(v)
                        }}
                      />
                    }
                    label={t('members.exempt')}
                  />
                  {typePaysMembership && ageExempt ? (
                    <FormHelperText sx={{ mx: 0 }}>{t('members.exemptHelperAgePayment')}</FormHelperText>
                  ) : null}
                </Box>
              )}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('members.fields.note')}
              fullWidth
              multiline
              minRows={3}
              disabled={fieldDisabled}
              {...register('note')}
              error={!!errors.note}
              helperText={errors.note?.message}
            />
          </Grid>
        </Grid>

        {!inactiveRecord ? (
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3 }}>
            <Button type="submit" variant="contained" disabled={updateMutation.isPending}>
              {t('common.save')}
            </Button>
          </Box>
        ) : null}
      </Box>
    </Paper>
  )
}

export default ProfileTab
