import { Navigate, useLocation } from 'react-router-dom'
import { getAccessToken } from '../../services/authToken'

interface ProtectedRouteProps {
  children: React.ReactNode
}

const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  const location = useLocation()
  const token = getAccessToken()

  if (token === null || token === '') {
    return <Navigate to="/login" replace state={{ from: location.pathname }} />
  }

  return <>{children}</>
}

export default ProtectedRoute
