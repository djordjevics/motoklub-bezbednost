import { Authenticator } from '@aws-amplify/ui-react'
import '@aws-amplify/ui-react/styles.css'
import { Box, Container, Typography } from '@mui/material'

const LoginPage = () => {
  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Typography component="h1" variant="h2" gutterBottom sx={{ fontWeight: 'bold', mb: 4 }}>
          Hello Motorcycle
        </Typography>
        <Typography component="h2" variant="h5" gutterBottom sx={{ mb: 3 }}>
          Motoklub Bezbednost
        </Typography>
        <Authenticator />
      </Box>
    </Container>
  )
}

export default LoginPage

