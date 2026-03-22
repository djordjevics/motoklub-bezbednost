import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { ThemeProvider, createTheme } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import Layout from './components/Layout/Layout'
import ProtectedRoute from './components/Auth/ProtectedRoute'
import LoginPage from './pages/Auth/LoginPage'
import Dashboard from './pages/Dashboard/Dashboard'
import MembersPage from './pages/Members/MembersPage'
import MotorcyclesPage from './pages/Motorcycles/MotorcyclesPage'
import TrainingsPage from './pages/Trainings/TrainingsPage'
import EquipmentPage from './pages/Equipment/EquipmentPage'

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
})

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <Layout />
              </ProtectedRoute>
            }
          >
            <Route index element={<Navigate to="/dashboard" replace />} />
            <Route path="dashboard" element={<Dashboard />} />
            <Route path="members" element={<MembersPage />} />
            <Route path="motorcycles" element={<MotorcyclesPage />} />
            <Route path="trainings" element={<TrainingsPage />} />
            <Route path="equipment" element={<EquipmentPage />} />
          </Route>
        </Routes>
      </Router>
    </ThemeProvider>
  )
}

export default App

