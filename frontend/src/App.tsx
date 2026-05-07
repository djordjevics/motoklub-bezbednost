import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { ThemeProvider, createTheme } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import Layout from './components/Layout/Layout'
import Dashboard from './pages/Dashboard/Dashboard'
import MembersPage from './pages/Members/MembersPage'
import MemberDetailPage from './pages/Members/MemberDetailPage'
import TrainingSessionsPage from './pages/TrainingSessions/TrainingSessionsPage'
import TrainingSessionDetailPage from './pages/TrainingSessions/TrainingSessionDetailPage'
import MotorcyclesOverviewPage from './pages/Motorcycles/MotorcyclesOverviewPage'
import MotorcycleGroupDetailPage from './pages/Motorcycles/MotorcycleGroupDetailPage'

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
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <CssBaseline />
        <Router>
          <Routes>
            <Route path="/" element={<Layout />}>
              <Route index element={<Dashboard />} />
              <Route path="dashboard" element={<Dashboard />} />
              <Route path="members" element={<MembersPage />} />
              <Route path="members/:id" element={<MemberDetailPage />} />
              <Route path="training-sessions" element={<TrainingSessionsPage />} />
              <Route path="training-sessions/:id" element={<TrainingSessionDetailPage />} />
              <Route path="motorcycles" element={<MotorcyclesOverviewPage />} />
              <Route path="motorcycles/:brandKey/:modelKey" element={<MotorcycleGroupDetailPage />} />
              <Route path="*" element={<Navigate to="/" replace />} />
            </Route>
          </Routes>
        </Router>
      </LocalizationProvider>
    </ThemeProvider>
  )
}

export default App
