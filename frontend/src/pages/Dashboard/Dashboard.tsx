import { Box, Grid, Paper, Typography } from '@mui/material'
import { useQuery } from '@tanstack/react-query'
import { memberService } from '../../services/memberService'
import { motorcycleService } from '../../services/motorcycleService'
import { trainingService } from '../../services/trainingService'

const Dashboard = () => {
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

  const stats = [
    { title: 'Total Members', value: members?.length || 0 },
    { title: 'Total Motorcycles', value: motorcycles?.length || 0 },
    { title: 'Training Sessions', value: trainingSessions?.length || 0 },
    { title: 'Active Members', value: members?.filter((m) => m.isActive).length || 0 },
  ]

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ mb: 3 }}>
        Dashboard
      </Typography>
      <Grid container spacing={3} sx={{ mt: 2 }}>
        {stats.map((stat) => (
          <Grid item xs={12} sm={6} md={3} key={stat.title}>
            <Paper sx={{ p: 3, textAlign: 'center' }}>
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

