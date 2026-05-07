import { useMemo, useState } from 'react'
import {
  Box,
  Button,
  Chip,
  CircularProgress,
  IconButton,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Tooltip,
  Typography,
} from '@mui/material'
import AddIcon from '@mui/icons-material/Add'
import EditIcon from '@mui/icons-material/Edit'
import DeleteIcon from '@mui/icons-material/Delete'
import { useNavigate } from 'react-router-dom'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { trainingService } from '../../services/trainingService'
import { lookupsService } from '../../services/lookupsService'
import type { TrainingSession } from '../../types/Training'
import type { Level } from '../../types/Level'
import { ConfirmDialog } from '../../components/common/ConfirmDialog'
import TrainingSessionForm from '../../components/TrainingSessions/TrainingSessionForm'
import { formatIsoDateDdMmYyyyOrDash } from '../../utils/dateFormat'

const formatCellDate = (value?: string): string => formatIsoDateDdMmYyyyOrDash(value)

const TrainingSessionsPage = () => {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const queryClient = useQueryClient()

  const [formOpen, setFormOpen] = useState(false)
  const [editingId, setEditingId] = useState<number | null>(null)
  const [deletingId, setDeletingId] = useState<number | null>(null)

  const { data: sessions, isLoading, error } = useQuery<TrainingSession[]>({
    queryKey: ['trainingSessions'],
    queryFn: trainingService.getAllSessions,
  })

  const { data: levels } = useQuery<Level[]>({
    queryKey: ['levels'],
    queryFn: lookupsService.getLevels,
  })

  const levelMap = useMemo(() => {
    const map = new Map<number, Level>()
    levels?.forEach((level) => map.set(level.id, level))
    return map
  }, [levels])

  const deleteMutation = useMutation({
    mutationFn: (id: number) => trainingService.deleteSession(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['trainingSessions'] })
      setDeletingId(null)
    },
  })

  const handleAdd = () => {
    setEditingId(null)
    setFormOpen(true)
  }

  const handleEdit = (id: number) => {
    setEditingId(id)
    setFormOpen(true)
  }

  const handleCloseForm = () => {
    setFormOpen(false)
    setEditingId(null)
  }

  const handleRowClick = (id: number) => {
    navigate(`/training-sessions/${id}`)
  }

  const renderLevelChip = (session: TrainingSession) => {
    const levelName = session.level?.name ?? (session.levelId ? levelMap.get(session.levelId)?.name : undefined)
    if (!levelName) {
      return <span>-</span>
    }
    return <Chip label={levelName} size="small" color="primary" variant="outlined" />
  }

  return (
    <Box>
      <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 2 }}>
        <Typography variant="h4">{t('trainingSessions.title')}</Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={handleAdd}>
          {t('trainingSessions.addNew')}
        </Button>
      </Stack>

      {isLoading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 200 }}>
          <CircularProgress />
        </Box>
      ) : error ? (
        <Box sx={{ p: 2 }}>
          <Typography color="error">{t('common.error')}</Typography>
        </Box>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>{t('trainingSessions.fields.city')}</TableCell>
                <TableCell>{t('trainingSessions.fields.theoryDate')}</TableCell>
                <TableCell>{t('trainingSessions.fields.polygonDate')}</TableCell>
                <TableCell>{t('trainingSessions.fields.level')}</TableCell>
                <TableCell>{t('trainingSessions.fields.price')}</TableCell>
                <TableCell>{t('trainingSessions.fields.instructors')}</TableCell>
                <TableCell align="right">{t('common.actions')}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {sessions && sessions.length > 0 ? (
                sessions.map((session) => (
                  <TableRow
                    key={session.id}
                    hover
                    sx={{ cursor: 'pointer' }}
                    onClick={() => handleRowClick(session.id)}
                  >
                    <TableCell>{session.city ?? '-'}</TableCell>
                    <TableCell>{formatCellDate(session.theoryDate)}</TableCell>
                    <TableCell>{formatCellDate(session.polygonDate)}</TableCell>
                    <TableCell>{renderLevelChip(session)}</TableCell>
                    <TableCell>{session.price ?? '-'}</TableCell>
                    <TableCell>{session.instructors ?? '-'}</TableCell>
                    <TableCell
                      align="right"
                      onClick={(event) => event.stopPropagation()}
                      sx={{ cursor: 'default' }}
                    >
                      <Tooltip title={t('common.edit')}>
                        <IconButton size="small" onClick={() => handleEdit(session.id)}>
                          <EditIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>
                      <Tooltip title={t('common.delete')}>
                        <IconButton
                          size="small"
                          color="error"
                          onClick={() => setDeletingId(session.id)}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={7} align="center">
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

      <TrainingSessionForm open={formOpen} sessionId={editingId} onClose={handleCloseForm} />

      <ConfirmDialog
        open={deletingId !== null}
        title={t('trainingSessions.delete.title')}
        message={t('trainingSessions.delete.message')}
        destructive
        confirmText={t('common.delete')}
        onConfirm={() => {
          if (deletingId !== null) {
            deleteMutation.mutate(deletingId)
          }
        }}
        onCancel={() => setDeletingId(null)}
      />
    </Box>
  )
}

export default TrainingSessionsPage
