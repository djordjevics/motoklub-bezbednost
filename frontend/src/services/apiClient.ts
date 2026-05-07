import axios from 'axios'
import API_BASE_URL from '../config/apiConfig'

// Auth is currently disabled (Motoklub.RequireAuthenticatedApi=false). The client stays minimal —
// no token interceptors — until/unless authentication is reintroduced.
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

export default apiClient
