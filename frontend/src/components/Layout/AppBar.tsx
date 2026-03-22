import { useNavigate } from 'react-router-dom'
import { AppBar as MuiAppBar, Button, Toolbar, Typography } from '@mui/material'
import { clearAccessToken } from '../../services/authToken'

const AppBar = () => {
  const navigate = useNavigate()

  const handleLogout = () => {
    clearAccessToken()
    navigate('/login', { replace: true })
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
        <Button color="inherit" onClick={handleLogout}>
          Log out
        </Button>
      </Toolbar>
    </MuiAppBar>
  )
}

export default AppBar

