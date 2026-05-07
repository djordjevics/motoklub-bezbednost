import { useState } from 'react'
import {
  Box,
  Button,
  Chip,
  CircularProgress,
  Divider,
  IconButton,
  Paper,
  Stack,
  Tooltip,
  Typography,
} from '@mui/material'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import EditIcon from '@mui/icons-material/Edit'
import AddIcon from '@mui/icons-material/Add'
import { useNavigate, useParams } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { trainingService } from '../../services/trainingService'
import type { Training, TrainingSession } from '../../types/Training'
import TrainingSessionForm from '../../components/TrainingSessions/TrainingSessionForm'
import AttendeesTable from '../../components/TrainingSessions/AttendeesTable'
import AttendeeForm from '../../components/TrainingSessions/AttendeeForm'
import { formatIsoDateDdMmYyyyOrDash } from '../../utils/dateFormat'

const formatDate = (value?: string): string => formatIsoDateDdMmYyyyOrDash(value)

interface SummaryItemProps {
  label: string
  value: React.ReactNode
}

const SummaryItem = ({ label, value }: SummaryItemProps) => (
  <Box>
    <Typography variant="caption" color="text.secondary">
      {label}
    </Typography>
    <Typography variant="body1">{value}</Typography>
  </Box>
)

const TrainingSessionDetailPage = () => {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const { id } = useParams<{ id: string }>()
  const sessionId = Number(id)

  const [editOpen, setEditOpen] = useState(false)
  const [attendeeFormOpen, setAttendeeFormOpen] = useState(false)
  const [editingTraining, setEditingTraining] = useState<Training | null>(null)

  const { data: session, isLoading, error } = useQuery<TrainingSession>({
    queryKey: ['trainingSession', sessionId],
    queryFn: () => trainingService.getSessionById(sessionId),
    enabled: !Number.isNaN(sessionId) && sessionId > 0,
  })

  const handleAddAttendee = () => {
    setEditingTraining(null)
    setAttendeeFormOpen(true)
  }

  const handleEditAttendee = (training: Training) => {
    setEditingTraining(training)
    setAttendeeFormOpen(true)
  }

  const handleCloseAttendeeForm = () => {
    setAttendeeFormOpen(false)
    setEditingTraining(null)
  }

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 200 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (error || !session) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography color="error">{t('common.error')}</Typography>
      </Box>
    )
  }

  const titleParts: string[] = []
  if (session.city) {
    titleParts.push(session.city)
  }
  const dateForTitle = formatDate(session.theoryDate)
  if (dateForTitle !== '-') {
    titleParts.push(dateForTitle)
  }
  const title = titleParts.length > 0 ? titleParts.join(' • ') : `#${session.id}`

  return (
    <Box>
      <Stack direction="row" alignItems="center" spacing={1} sx={{ mb: 2 }}>
        <Tooltip title={t('common.back')}>
          <IconButton onClick={() => navigate(-1)} aria-label={t('common.back')}>
            <ArrowBackIcon />
          </IconButton>
        </Tooltip>
        <Typography variant="h4" sx={{ flex: 1 }}>
          {title}
        </Typography>
        {session.level?.name && (
          <Chip label={session.level.name} color="primary" variant="outlined" />
        )}
      </Stack>

      <Paper sx={{ p: 2, mb: 3 }}>
        <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 2 }}>
          <Typography variant="h6">{t('common.details')}</Typography>
          <Button startIcon={<EditIcon />} onClick={() => setEditOpen(true)}>
            {t('common.edit')}
          </Button>
        </Stack>
        <Box
          sx={{
            display: 'grid',
            gap: 2,
            gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr', md: 'repeat(3, 1fr)' },
          }}
        >
          <SummaryItem label={t('trainingSessions.fields.city')} value={session.city ?? '-'} />
          <SummaryItem
            label={t('trainingSessions.fields.theoryDate')}
            value={formatDate(session.theoryDate)}
          />
          <SummaryItem
            label={t('trainingSessions.fields.polygonDate')}
            value={formatDate(session.polygonDate)}
          />
          <SummaryItem
            label={t('trainingSessions.fields.level')}
            value={session.level?.name ?? '-'}
          />
          <SummaryItem
            label={t('trainingSessions.fields.price')}
            value={session.price ?? '-'}
          />
          <SummaryItem
            label={t('trainingSessions.fields.instructors')}
            value={session.instructors ?? '-'}
          />
          <Box sx={{ gridColumn: { xs: '1', sm: '1 / span 2', md: '1 / span 3' } }}>
            <SummaryItem
              label={t('trainingSessions.fields.note')}
              value={session.note ?? '-'}
            />
          </Box>
        </Box>
      </Paper>

      <Divider sx={{ mb: 2 }} />

      <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 2 }}>
        <Typography variant="h5">{t('trainingSessions.attendees')}</Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={handleAddAttendee}>
          {t('trainingSessions.addAttendee')}
        </Button>
      </Stack>

      <AttendeesTable sessionId={sessionId} onEdit={handleEditAttendee} />

      <TrainingSessionForm
        open={editOpen}
        sessionId={sessionId}
        onClose={() => setEditOpen(false)}
      />

      <AttendeeForm
        open={attendeeFormOpen}
        sessionId={sessionId}
        training={editingTraining}
        onClose={handleCloseAttendeeForm}
      />
    </Box>
  )
}

export default TrainingSessionDetailPage
