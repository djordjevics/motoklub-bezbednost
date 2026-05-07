import { useEffect } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  FormControlLabel,
  Grid,
  Paper,
  Switch,
  TextField,
  Typography,
} from '@mui/material'
import { useTranslation } from 'react-i18next'
import { equipmentService } from '../../services/equipmentService'
import type { Equipment } from '../../types/Equipment'

interface EquipmentTabProps {
  memberId: number
  recordLocked?: boolean
}

interface EquipmentFormValues {
  pants: boolean
  jacket: boolean
  vest: boolean
  workShirt: boolean
  formalShirt: boolean
  note: string
}

const emptyDefaults: EquipmentFormValues = {
  pants: false,
  jacket: false,
  vest: false,
  workShirt: false,
  formalShirt: false,
  note: '',
}

const toFormValues = (e: Equipment): EquipmentFormValues => ({
  pants: e.pants,
  jacket: e.jacket,
  vest: e.vest,
  workShirt: e.workShirt,
  formalShirt: e.formalShirt,
  note: e.note ?? '',
})

const emptyToUndef = (value: string): string | undefined => {
  const trimmed = value.trim()
  return trimmed === '' ? undefined : trimmed
}

const EquipmentTab = ({ memberId, recordLocked = false }: EquipmentTabProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const queryKey = ['equipment', 'byMember', memberId] as const

  const { data, isLoading, error } = useQuery({
    queryKey,
    queryFn: () => equipmentService.getByMemberId(memberId),
  })

  const existing = data && data.length > 0 ? data[0] : null

  const { control, register, handleSubmit, reset } = useForm<EquipmentFormValues>({
    defaultValues: emptyDefaults,
  })

  useEffect(() => {
    reset(existing ? toFormValues(existing) : emptyDefaults)
  }, [existing, reset])

  const saveMutation = useMutation({
    mutationFn: async (values: EquipmentFormValues) => {
      const payload = {
        pants: values.pants,
        jacket: values.jacket,
        vest: values.vest,
        workShirt: values.workShirt,
        formalShirt: values.formalShirt,
        note: emptyToUndef(values.note),
      }
      if (existing) {
        await equipmentService.update(existing.id, payload)
      } else {
        await equipmentService.create({ ...payload, memberId })
      }
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
    },
  })

  const onSubmit = handleSubmit((values) => {
    if (recordLocked) return
    saveMutation.mutate(values)
  })

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (error) {
    return <Typography color="error">{t('common.error')}</Typography>
  }

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h6" sx={{ mb: 2 }}>
        {t('equipment.title')}
      </Typography>

      {!existing && (
        <Alert severity="info" sx={{ mb: 2 }}>
          {t('common.noData')}
        </Alert>
      )}

      <Box component="form" onSubmit={onSubmit} noValidate>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6}>
            <Controller
              name="pants"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      disabled={recordLocked}
                      onChange={(e) => field.onChange(e.target.checked)}
                    />
                  }
                  label={t('equipment.fields.pants')}
                />
              )}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="jacket"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      disabled={recordLocked}
                      onChange={(e) => field.onChange(e.target.checked)}
                    />
                  }
                  label={t('equipment.fields.jacket')}
                />
              )}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="vest"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      disabled={recordLocked}
                      onChange={(e) => field.onChange(e.target.checked)}
                    />
                  }
                  label={t('equipment.fields.vest')}
                />
              )}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="workShirt"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      disabled={recordLocked}
                      onChange={(e) => field.onChange(e.target.checked)}
                    />
                  }
                  label={t('equipment.fields.workShirt')}
                />
              )}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="formalShirt"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      disabled={recordLocked}
                      onChange={(e) => field.onChange(e.target.checked)}
                    />
                  }
                  label={t('equipment.fields.formalShirt')}
                />
              )}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label={t('equipment.fields.note')}
              fullWidth
              multiline
              minRows={3}
              disabled={recordLocked}
              {...register('note')}
            />
          </Grid>
        </Grid>

        <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3 }}>
          <Button type="submit" variant="contained" disabled={saveMutation.isPending || recordLocked}>
            {t('common.save')}
          </Button>
        </Box>
      </Box>
    </Paper>
  )
}

export default EquipmentTab
