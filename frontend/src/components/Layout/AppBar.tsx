import { AppBar as MuiAppBar, Toolbar, Typography } from '@mui/material'

const AppBar = () => {
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
      </Toolbar>
    </MuiAppBar>
  )
}

export default AppBar

