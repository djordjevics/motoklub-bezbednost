import { Outlet } from 'react-router-dom'
import { Box, Container } from '@mui/material'
import AppBar from './AppBar'
import Sidebar from './Sidebar'

const Layout = () => {
  return (
    <Box sx={{ display: 'flex', minHeight: '100vh' }}>
      <AppBar />
      <Sidebar />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          marginTop: '64px',
          marginLeft: '240px',
          width: 'calc(100% - 240px)',
        }}
      >
        <Container maxWidth="xl">
          <Outlet />
        </Container>
      </Box>
    </Box>
  )
}

export default Layout

