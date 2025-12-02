import { AppBar as MuiAppBar, Toolbar, Typography, IconButton, Box } from '@mui/material'
import { signOut } from '@aws-amplify/auth'
import { useNavigate } from 'react-router-dom'
import LogoutIcon from '@mui/icons-material/Logout'

const AppBar = () => {
  const navigate = useNavigate()

  const handleSignOut = async () => {
    try {
      await signOut()
      navigate('/login')
    } catch (error) {
      console.error('Error signing out:', error)
    }
  }

  return (
    <MuiAppBar
      position="fixed"
      sx={{
        zIndex: (theme) => theme.zIndex.drawer + 1,
        width: '100%',
      }}
    >
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          Motoklub Bezbednost
        </Typography>
        <Box>
          <IconButton color="inherit" onClick={handleSignOut}>
            <LogoutIcon />
          </IconButton>
        </Box>
      </Toolbar>
    </MuiAppBar>
  )
}

export default AppBar

