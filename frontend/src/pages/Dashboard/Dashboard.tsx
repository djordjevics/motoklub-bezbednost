import { Box, Grid, Paper, Typography } from '@mui/material'
import { useQuery } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { memberService } from '../../services/memberService'
import { motorcycleService } from '../../services/motorcycleService'
import { trainingService } from '../../services/trainingService'

const Dashboard = () => {
  const { t } = useTranslation()
  const navigate = useNavigate()

  const { data: members } = useQuery({
    queryKey: ['members'],
    queryFn: memberService.getAll,
  })

  const { data: motorcycles } = useQuery({
    queryKey: ['motorcycles'],
    queryFn: motorcycleService.getAll,
  })

  const { data: trainingSessions } = useQuery({
    queryKey: ['trainingSessions'],
    queryFn: trainingService.getAllSessions,
  })

  type Stat = {
    title: string
    value: number
    onClick?: () => void
  }

  const stats: Stat[] = [
    {
      title: t('dashboard.totalMembers'),
      value: members?.length ?? 0,
      onClick: () => navigate('/members'),
    },
    {
      title: t('dashboard.totalSessions'),
      value: trainingSessions?.length ?? 0,
      onClick: () => navigate('/training-sessions'),
    },
    {
      title: t('dashboard.totalMotorcycles'),
      value: motorcycles?.length ?? 0,
      onClick: () => navigate('/motorcycles'),
    },
    {
      title: t('dashboard.membersWithTraining'),
      value: members?.filter((m) => (m.trainings?.length ?? 0) > 0).length ?? 0,
      onClick: () => navigate('/members?training=with'),
    },
  ]

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ mb: 3 }}>
        {t('dashboard.title')}
      </Typography>
      <Grid container spacing={3} sx={{ mt: 2 }}>
        {stats.map((stat) => (
          <Grid item xs={12} sm={6} md={3} key={stat.title}>
            <Paper
              sx={{
                p: 3,
                textAlign: 'center',
                ...(stat.onClick
                  ? {
                      cursor: 'pointer',
                      '&:hover': { bgcolor: 'action.hover' },
                    }
                  : {}),
              }}
              onClick={stat.onClick}
              role={stat.onClick ? 'button' : undefined}
              tabIndex={stat.onClick ? 0 : undefined}
              onKeyDown={
                stat.onClick
                  ? (e) => {
                      if (e.key === 'Enter' || e.key === ' ') {
                        e.preventDefault()
                        stat.onClick?.()
                      }
                    }
                  : undefined
              }
            >
              <Typography variant="h4" color="primary">
                {stat.value}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {stat.title}
              </Typography>
            </Paper>
          </Grid>
        ))}
      </Grid>
    </Box>
  )
}

export default Dashboard
