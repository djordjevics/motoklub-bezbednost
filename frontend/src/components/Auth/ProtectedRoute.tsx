import { useEffect, useState } from 'react'
import { Navigate } from 'react-router-dom'
import { getCurrentUser } from '@aws-amplify/auth'
import { Box, CircularProgress } from '@mui/material'
import amplifyConfig from '../../config/amplifyConfig'

interface ProtectedRouteProps {
  children: React.ReactNode
}

// Development mode: bypass auth if Cognito is not configured
const isDevelopmentMode = !amplifyConfig.Auth.Cognito.userPoolId || !amplifyConfig.Auth.Cognito.userPoolClientId

const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null)

  useEffect(() => {
    // In development mode without config, allow access
    if (isDevelopmentMode) {
      setIsAuthenticated(true)
      return
    }

    const checkAuth = async () => {
      try {
        // Add timeout to prevent hanging
        const timeoutPromise = new Promise((_, reject) =>
          setTimeout(() => reject(new Error('Auth check timeout')), 5000)
        )
        await Promise.race([getCurrentUser(), timeoutPromise])
        setIsAuthenticated(true)
      } catch (error) {
        console.error('Auth check failed:', error)
        setIsAuthenticated(false)
      }
    }
    checkAuth()
  }, [])

  if (isAuthenticated === null) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: '100vh',
        }}
      >
        <CircularProgress />
      </Box>
    )
  }

  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />
}

export default ProtectedRoute

