import { useMemo, useState } from 'react'
import {
  Box,
  Chip,
  CircularProgress,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Tooltip,
  Typography,
} from '@mui/material'
import EditIcon from '@mui/icons-material/Edit'
import DeleteIcon from '@mui/icons-material/Delete'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { trainingService } from '../../services/trainingService'
import { memberService } from '../../services/memberService'
import type { Training } from '../../types/Training'
import type { Member } from '../../types/Member'
import { ConfirmDialog } from '../common/ConfirmDialog'
import { formatMotorcycleBrandModelPlate } from '../../utils/motorcycleLabel'

interface Props {
  sessionId: number
  onEdit: (training: Training) => void
}

const AttendeesTable = ({ sessionId, onEdit }: Props) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const [deletingId, setDeletingId] = useState<number | null>(null)

  const { data: trainings, isLoading: trainingsLoading, error: trainingsError } = useQuery<Training[]>({
    queryKey: ['trainings', 'bySession', sessionId],
    queryFn: () => trainingService.getTrainingsBySession(sessionId),
  })

  const { data: members } = useQuery<Member[]>({
    queryKey: ['members'],
    queryFn: memberService.getAll,
  })

  const memberMap = useMemo(() => {
    const map = new Map<number, Member>()
    members?.forEach((member) => map.set(member.id, member))
    return map
  }, [members])

  const deleteMutation = useMutation({
    mutationFn: (id: number) => trainingService.deleteTraining(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['trainings', 'bySession', sessionId] })
      setDeletingId(null)
    },
  })

  if (trainingsLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 160 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (trainingsError) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography color="error">{t('common.error')}</Typography>
      </Box>
    )
  }

  const renderMember = (training: Training) => {
    const member = memberMap.get(training.memberId) ?? training.member
    if (!member) {
      return `#${training.memberId}`
    }
    return `${member.name} ${member.surname}`
  }

  const renderMotorcycle = (training: Training) => {
    if (!training.motorcycle) {
      return `#${training.motorcycleId}`
    }
    const label = formatMotorcycleBrandModelPlate(training.motorcycle)
    return label === '-' ? `#${training.motorcycleId}` : label
  }

  return (
    <>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>
                {t('members.fields.name')} / {t('members.fields.surname')}
              </TableCell>
              <TableCell>{t('trainings.fields.motorcycle')}</TableCell>
              <TableCell>{t('trainings.fields.repeatingAttendance')}</TableCell>
              <TableCell>{t('trainings.fields.certificate')}</TableCell>
              <TableCell>{t('trainings.fields.note')}</TableCell>
              <TableCell align="right">{t('common.actions')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {trainings && trainings.length > 0 ? (
              trainings.map((training) => (
                <TableRow key={training.id} hover>
                  <TableCell>{renderMember(training)}</TableCell>
                  <TableCell>{renderMotorcycle(training)}</TableCell>
                  <TableCell>
                    <Chip
                      label={training.repeatingAttendance ? t('common.yes') : t('common.no')}
                      size="small"
                      color={training.repeatingAttendance ? 'info' : 'default'}
                      variant={training.repeatingAttendance ? 'filled' : 'outlined'}
                    />
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={training.isCertificateIssued ? t('common.yes') : t('common.no')}
                      size="small"
                      color={training.isCertificateIssued ? 'success' : 'default'}
                      variant={training.isCertificateIssued ? 'filled' : 'outlined'}
                    />
                  </TableCell>
                  <TableCell>{training.note ?? '-'}</TableCell>
                  <TableCell align="right">
                    <Tooltip title={t('common.edit')}>
                      <IconButton size="small" onClick={() => onEdit(training)}>
                        <EditIcon fontSize="small" />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title={t('common.delete')}>
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => setDeletingId(training.id)}
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Tooltip>
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

      <ConfirmDialog
        open={deletingId !== null}
        title={t('common.delete')}
        destructive
        confirmText={t('common.delete')}
        onConfirm={() => {
          if (deletingId !== null) {
            deleteMutation.mutate(deletingId)
          }
        }}
        onCancel={() => setDeletingId(null)}
      />
    </>
  )
}

export default AttendeesTable
