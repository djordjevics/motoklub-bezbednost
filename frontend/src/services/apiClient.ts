import axios from 'axios'
import { getCurrentUser, fetchAuthSession } from '@aws-amplify/auth'
import API_BASE_URL from '../config/apiConfig'

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Add auth token to requests
apiClient.interceptors.request.use(
  async (config) => {
    try {
      const session = await fetchAuthSession()
      if (session.tokens?.idToken) {
        config.headers.Authorization = `Bearer ${session.tokens.idToken.toString()}`
      }
    } catch (error) {
      console.error('Error getting auth session:', error)
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

export default apiClient

