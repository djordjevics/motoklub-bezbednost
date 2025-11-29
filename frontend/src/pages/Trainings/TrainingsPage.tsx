import { Box, Typography } from '@mui/material'
import TrainingsList from '../../components/Trainings/TrainingsList'

const TrainingsPage = () => {
  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Training Sessions
      </Typography>
      <TrainingsList />
    </Box>
  )
}

export default TrainingsPage

