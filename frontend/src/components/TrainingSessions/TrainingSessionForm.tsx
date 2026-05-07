import { useEffect } from 'react'

import {

  Box,

  Button,

  CircularProgress,

  Dialog,

  DialogActions,

  DialogContent,

  DialogTitle,

  MenuItem,

  TextField,

} from '@mui/material'

import { Controller, useForm } from 'react-hook-form'

import { yupResolver } from '@hookform/resolvers/yup'

import * as yup from 'yup'

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'

import { useTranslation } from 'react-i18next'

import { trainingService } from '../../services/trainingService'

import { lookupsService } from '../../services/lookupsService'

import type { Level } from '../../types/Level'

import type { TrainingSession } from '../../types/Training'

import FormDatePicker from '../common/FormDatePicker'

import { defaultLevelIdString } from '../../utils/lookupDefaults'

import { todayIsoDateString } from '../../utils/dateFormat'



interface Props {

  open: boolean

  sessionId: number | null

  onClose: () => void

}



interface FormValues {

  theoryDate?: string

  polygonDate?: string

  city?: string

  levelId?: string

  price?: string

  instructors?: string

  note?: string

}



const emptyValues: FormValues = {

  theoryDate: '',

  polygonDate: '',

  city: '',

  levelId: '',

  price: '',

  instructors: '',

  note: '',

}



const stripDate = (value?: string): string => (value ? value.split('T')[0] : '')



const TrainingSessionForm = ({ open, sessionId, onClose }: Props) => {

  const { t } = useTranslation()

  const queryClient = useQueryClient()

  const isEdit = sessionId !== null



  const schema: yup.ObjectSchema<FormValues> = yup.object({

    theoryDate: yup.string().optional(),

    polygonDate: yup.string().optional(),

    city: yup.string().max(100).optional(),

    levelId: yup.string().optional(),

    price: yup.string().optional(),

    instructors: yup.string().optional(),

    note: yup.string().optional(),

  })



  const {

    control,

    register,

    handleSubmit,

    reset,

    formState: { errors },

  } = useForm<FormValues>({

    resolver: yupResolver(schema),

    defaultValues: emptyValues,

  })



  const { data: levels } = useQuery<Level[]>({

    queryKey: ['levels'],

    queryFn: lookupsService.getLevels,

    enabled: open,

  })



  const { data: session, isLoading: isSessionLoading } = useQuery<TrainingSession>({

    queryKey: ['trainingSession', sessionId],

    queryFn: () => trainingService.getSessionById(sessionId as number),

    enabled: open && isEdit,

  })



  useEffect(() => {

    if (!open) {

      return

    }

    if (isEdit && session) {

      reset({

        theoryDate: stripDate(session.theoryDate),

        polygonDate: stripDate(session.polygonDate),

        city: session.city ?? '',

        levelId: session.levelId !== undefined && session.levelId !== null ? String(session.levelId) : '',

        price: session.price !== undefined && session.price !== null ? String(session.price) : '',

        instructors: session.instructors ?? '',

        note: session.note ?? '',

      })

    } else if (!isEdit) {

      const td = todayIsoDateString()

      reset({

        ...emptyValues,

        theoryDate: td,

        polygonDate: td,

        levelId: defaultLevelIdString(levels),

      })

    }

  }, [open, isEdit, session, levels, reset])



  const buildPayload = (values: FormValues): Omit<TrainingSession, 'id' | 'level'> => ({

    theoryDate: values.theoryDate ? values.theoryDate : undefined,

    polygonDate: values.polygonDate ? values.polygonDate : undefined,

    city: values.city ? values.city : undefined,

    levelId: values.levelId ? Number(values.levelId) : undefined,

    price: values.price !== undefined && values.price !== '' ? Number(values.price) : undefined,

    instructors: values.instructors ? values.instructors : undefined,

    note: values.note ? values.note : undefined,

  })



  const createMutation = useMutation({

    mutationFn: (payload: Omit<TrainingSession, 'id' | 'level'>) => trainingService.createSession(payload),

    onSuccess: () => {

      queryClient.invalidateQueries({ queryKey: ['trainingSessions'] })

      onClose()

    },

  })



  const updateMutation = useMutation({

    mutationFn: (payload: Omit<TrainingSession, 'id' | 'level'>) =>

      trainingService.updateSession(sessionId as number, payload),

    onSuccess: () => {

      queryClient.invalidateQueries({ queryKey: ['trainingSessions'] })

      queryClient.invalidateQueries({ queryKey: ['trainingSession', sessionId] })

      onClose()

    },

  })



  const onSubmit = (values: FormValues) => {

    const payload = buildPayload(values)

    if (isEdit) {

      updateMutation.mutate(payload)

    } else {

      createMutation.mutate(payload)

    }

  }



  const submitting = createMutation.isPending || updateMutation.isPending

  const showLoading = isEdit && isSessionLoading



  return (

    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>

      <DialogTitle>{isEdit ? t('common.edit') : t('trainingSessions.addNew')}</DialogTitle>

      <form onSubmit={handleSubmit(onSubmit)} noValidate>

        <DialogContent dividers>

          {showLoading ? (

            <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>

              <CircularProgress />

            </Box>

          ) : (

            <Box sx={{ display: 'grid', gap: 2, gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' } }}>

              <Controller

                name="theoryDate"

                control={control}

                render={({ field }) => (

                  <FormDatePicker

                    label={t('trainingSessions.fields.theoryDate')}

                    value={field.value ?? ''}

                    onChange={field.onChange}

                    error={!!errors.theoryDate}

                    helperText={errors.theoryDate?.message}

                    fullWidth

                  />

                )}

              />

              <Controller

                name="polygonDate"

                control={control}

                render={({ field }) => (

                  <FormDatePicker

                    label={t('trainingSessions.fields.polygonDate')}

                    value={field.value ?? ''}

                    onChange={field.onChange}

                    error={!!errors.polygonDate}

                    helperText={errors.polygonDate?.message}

                    fullWidth

                  />

                )}

              />

              <TextField

                label={t('trainingSessions.fields.city')}

                {...register('city')}

                error={!!errors.city}

                helperText={errors.city?.message}

                fullWidth

              />

              <Controller

                name="levelId"

                control={control}

                render={({ field }) => (

                  <TextField

                    label={t('trainingSessions.fields.level')}

                    select

                    disabled={!levels?.length}

                    {...field}

                    error={!!errors.levelId}

                    helperText={errors.levelId?.message}

                    fullWidth

                  >

                    {levels?.map((level) => (

                      <MenuItem key={level.id} value={String(level.id)}>

                        {level.name ?? `#${level.id}`}

                      </MenuItem>

                    ))}

                  </TextField>

                )}

              />

              <TextField

                label={t('trainingSessions.fields.price')}

                type="number"

                {...register('price')}

                error={!!errors.price}

                helperText={errors.price?.message}

                fullWidth

              />

              <TextField

                label={t('trainingSessions.fields.instructors')}

                {...register('instructors')}

                error={!!errors.instructors}

                helperText={errors.instructors?.message}

                fullWidth

              />

              <Box sx={{ gridColumn: { sm: '1 / span 2' } }}>

                <TextField

                  label={t('trainingSessions.fields.note')}

                  multiline

                  minRows={3}

                  {...register('note')}

                  error={!!errors.note}

                  helperText={errors.note?.message}

                  fullWidth

                />

              </Box>

            </Box>

          )}

        </DialogContent>

        <DialogActions>

          <Button onClick={onClose} disabled={submitting}>

            {t('common.cancel')}

          </Button>

          <Button type="submit" variant="contained" disabled={submitting || showLoading}>

            {t('common.save')}

          </Button>

        </DialogActions>

      </form>

    </Dialog>

  )

}



export default TrainingSessionForm

