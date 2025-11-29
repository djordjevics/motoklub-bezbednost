import { Box, Typography } from '@mui/material'
import MotorcyclesList from '../../components/Motorcycles/MotorcyclesList'

const MotorcyclesPage = () => {
  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Motorcycles
      </Typography>
      <MotorcyclesList />
    </Box>
  )
}

export default MotorcyclesPage

