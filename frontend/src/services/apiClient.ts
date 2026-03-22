import axios from 'axios'
import API_BASE_URL from '../config/apiConfig'
import { clearAccessToken, getAccessToken } from './authToken'

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

apiClient.interceptors.request.use((config) => {
  const token = getAccessToken()
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      clearAccessToken()
      if (!window.location.pathname.startsWith('/login')) {
        window.location.assign('/login')
      }
    }
    return Promise.reject(error)
  },
)

export default apiClient
