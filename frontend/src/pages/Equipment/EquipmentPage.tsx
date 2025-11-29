import { Box, Typography } from '@mui/material'
import EquipmentList from '../../components/Equipment/EquipmentList'

const EquipmentPage = () => {
  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Equipment
      </Typography>
      <EquipmentList />
    </Box>
  )
}

export default EquipmentPage

