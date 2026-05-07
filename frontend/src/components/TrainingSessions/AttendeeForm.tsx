import { useEffect, useMemo } from 'react'
import {
  Autocomplete,
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControlLabel,
  Switch,
  TextField,
} from '@mui/material'
import { Controller, useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { trainingService } from '../../services/trainingService'
import { memberService } from '../../services/memberService'
import { motorcycleService } from '../../services/motorcycleService'
import type { Training } from '../../types/Training'
import type { Member, Motorcycle } from '../../types/Member'
import { formatMotorcycleBrandModelPlate } from '../../utils/motorcycleLabel'

function memberLabel(m: Member): string {
  const base = `${m.name ?? ''} ${m.surname ?? ''}`.trim()
  return base || `#${m.id}`
}

function filterMembers(options: Member[], input: string): Member[] {
  const q = input.trim().toLowerCase()
  if (!q) return options
  return options.filter((m) => {
    const full = memberLabel(m).toLowerCase()
    const email = (m.email ?? '').toLowerCase()
    const phone = (m.mobilePhone ?? '').toLowerCase()
    return full.includes(q) || email.includes(q) || phone.includes(q)
  })
}

function motorcycleSearchText(m: Motorcycle): string {
  return [m.brandName, m.commercialName, m.modelName, m.registerPlate]
    .filter(Boolean)
    .join(' ')
    .toLowerCase()
}

function filterMotorcycles(options: Motorcycle[], input: string): Motorcycle[] {
  const q = input.trim().toLowerCase()
  if (!q) return options
  return options.filter((m) => motorcycleSearchText(m).includes(q))
}

interface Props {
  open: boolean
  sessionId: number
  training: Training | null
  onClose: () => void
}

interface FormValues {
  memberId: string
  motorcycleId: string
  repeatingAttendance: boolean
  isCertificateIssued: boolean
  note?: string
}

const emptyValues: FormValues = {
  memberId: '',
  motorcycleId: '',
  repeatingAttendance: false,
  isCertificateIssued: false,
  note: '',
}

const AttendeeForm = ({ open, sessionId, training, onClose }: Props) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const isEdit = training !== null

  const schema: yup.ObjectSchema<FormValues> = yup.object({
    memberId: yup.string().required(t('validation.required')),
    motorcycleId: yup.string().required(t('validation.required')),
    repeatingAttendance: yup.boolean().required(),
    isCertificateIssued: yup.boolean().required(),
    note: yup.string().optional(),
  })

  const {
    control,
    register,
    handleSubmit,
    reset,
    setValue,
    watch,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: yupResolver(schema),
    defaultValues: emptyValues,
  })

  const memberId = watch('memberId')

  const { data: members, isLoading: membersLoading } = useQuery<Member[]>({
    queryKey: ['members'],
    queryFn: memberService.getAll,
    enabled: open,
  })

  const memberIdNum = memberId ? Number(memberId) : 0

  const { data: motorcycles, isLoading: motorcyclesLoading } = useQuery<Motorcycle[]>({
    queryKey: ['motorcycles', 'byMember', memberIdNum],
    queryFn: () => motorcycleService.getByMemberId(memberIdNum),
    enabled: open && memberIdNum > 0,
  })

  useEffect(() => {
    if (!open) {
      return
    }
    if (training) {
      reset({
        memberId: String(training.memberId),
        motorcycleId: String(training.motorcycleId),
        repeatingAttendance: training.repeatingAttendance ?? false,
        isCertificateIssued: training.isCertificateIssued,
        note: training.note ?? '',
      })
    } else {
      reset(emptyValues)
    }
  }, [open, training, reset])

  const createMutation = useMutation({
    mutationFn: (payload: Omit<Training, 'id' | 'member' | 'motorcycle' | 'trainingSession'>) =>
      trainingService.createTraining(payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['trainings', 'bySession', sessionId] })
      onClose()
    },
  })

  const updateMutation = useMutation({
    mutationFn: (payload: Partial<Omit<Training, 'member' | 'motorcycle' | 'trainingSession'>>) =>
      trainingService.updateTraining(training?.id as number, payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['trainings', 'bySession', sessionId] })
      onClose()
    },
  })

  const onSubmit = (values: FormValues) => {
    const note = values.note ? values.note : undefined
    if (isEdit && training) {
      updateMutation.mutate({
        id: training.id,
        memberId: Number(values.memberId),
        motorcycleId: Number(values.motorcycleId),
        trainingSessionId: sessionId,
        repeatingAttendance: values.repeatingAttendance,
        isCertificateIssued: values.isCertificateIssued,
        note,
      })
    } else {
      createMutation.mutate({
        memberId: Number(values.memberId),
        motorcycleId: Number(values.motorcycleId),
        trainingSessionId: sessionId,
        repeatingAttendance: values.repeatingAttendance,
        isCertificateIssued: values.isCertificateIssued,
        note,
      })
    }
  }

  const submitting = createMutation.isPending || updateMutation.isPending

  const selectableMembers = useMemo(() => {
    const all = members ?? []
    const roster = all.filter((m) => !m.membershipExemptManual)
    if (!isEdit || !training) return roster
    const current = all.find((m) => m.id === training.memberId)
    if (!current?.membershipExemptManual) return roster
    return [current, ...roster.filter((m) => m.id !== current.id)]
  }, [members, isEdit, training])

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{isEdit ? t('common.edit') : t('trainingSessions.addAttendee')}</DialogTitle>
      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        <DialogContent dividers>
          <Box sx={{ display: 'grid', gap: 2 }}>
            <Controller
              control={control}
              name="memberId"
              render={({ field }) => {
                const selectedMember = selectableMembers.find((m) => String(m.id) === field.value) ?? null
                return (
                  <Autocomplete
                    options={selectableMembers}
                    loading={membersLoading}
                    value={selectedMember}
                    onChange={(_, value) => {
                      field.onChange(value ? String(value.id) : '')
                      setValue('motorcycleId', '')
                    }}
                    getOptionLabel={(m) => memberLabel(m)}
                    isOptionEqualToValue={(a, b) => a.id === b.id}
                    filterOptions={(options, state) => filterMembers(options, state.inputValue)}
                    noOptionsText={t('common.noData')}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        label={`${t('members.fields.name')} / ${t('members.fields.surname')}`}
                        required
                        error={!!errors.memberId}
                        helperText={errors.memberId?.message ?? t('trainingSessions.attendeeSearch.memberHint')}
                        InputProps={{
                          ...params.InputProps,
                          endAdornment: (
                            <>
                              {membersLoading ? <CircularProgress color="inherit" size={20} /> : null}
                              {params.InputProps.endAdornment}
                            </>
                          ),
                        }}
                      />
                    )}
                  />
                )
              }}
            />
            <Controller
              control={control}
              name="motorcycleId"
              render={({ field }) => {
                const motoList = motorcycles ?? []
                const selectedMoto = motoList.find((m) => String(m.id) === field.value) ?? null
                return (
                  <Autocomplete
                    options={motoList}
                    loading={motorcyclesLoading}
                    disabled={!memberIdNum}
                    value={selectedMoto}
                    onChange={(_, value) => {
                      field.onChange(value ? String(value.id) : '')
                    }}
                    getOptionLabel={(m) => {
                      const lbl = formatMotorcycleBrandModelPlate(m)
                      return lbl === '-' ? `#${m.id}` : lbl
                    }}
                    isOptionEqualToValue={(a, b) => a.id === b.id}
                    filterOptions={(options, state) => filterMotorcycles(options, state.inputValue)}
                    noOptionsText={t('common.noData')}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        label={t('trainings.fields.motorcycle')}
                        required
                        error={!!errors.motorcycleId}
                        helperText={errors.motorcycleId?.message ?? t('trainingSessions.attendeeSearch.motorcycleHint')}
                        InputProps={{
                          ...params.InputProps,
                          endAdornment: (
                            <>
                              {motorcyclesLoading ? <CircularProgress color="inherit" size={20} /> : null}
                              {params.InputProps.endAdornment}
                            </>
                          ),
                        }}
                      />
                    )}
                  />
                )
              }}
            />
            <Controller
              control={control}
              name="repeatingAttendance"
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      onChange={(event) => field.onChange(event.target.checked)}
                    />
                  }
                  label={t('trainings.fields.repeatingAttendance')}
                />
              )}
            />
            <Controller
              control={control}
              name="isCertificateIssued"
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      onChange={(event) => field.onChange(event.target.checked)}
                    />
                  }
                  label={t('trainings.fields.isCertificateIssued')}
                />
              )}
            />
            <TextField
              label={t('trainings.fields.note')}
              multiline
              minRows={3}
              {...register('note')}
              error={!!errors.note}
              helperText={errors.note?.message}
              fullWidth
            />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} disabled={submitting}>
            {t('common.cancel')}
          </Button>
          <Button type="submit" variant="contained" disabled={submitting}>
            {t('common.save')}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}

export default AttendeeForm
