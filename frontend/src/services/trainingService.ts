import apiClient from './apiClient'
import type { Training, TrainingSession } from '../types/Training'

export const trainingService = {
  // Training Sessions
  getAllSessions: async (): Promise<TrainingSession[]> => {
    const response = await apiClient.get<TrainingSession[]>('/trainings')
    return response.data
  },

  getSessionById: async (id: number): Promise<TrainingSession> => {
    const response = await apiClient.get<TrainingSession>(`/trainings/${id}`)
    return response.data
  },

  createSession: async (session: Omit<TrainingSession, 'id' | 'level'>): Promise<TrainingSession> => {
    const response = await apiClient.post<TrainingSession>('/trainings/sessions', session)
    return response.data
  },

  updateSession: async (id: number, session: Partial<Omit<TrainingSession, 'level'>>): Promise<void> => {
    await apiClient.put(`/trainings/sessions/${id}`, { ...session, id })
  },

  deleteSession: async (id: number): Promise<void> => {
    await apiClient.delete(`/trainings/sessions/${id}`)
  },

  // Trainings (attendees of a session)
  getTrainingsByMember: async (memberId: number): Promise<Training[]> => {
    const response = await apiClient.get<Training[]>(`/trainings/member/${memberId}`)
    return response.data
  },

  getTrainingsBySession: async (sessionId: number): Promise<Training[]> => {
    const response = await apiClient.get<Training[]>(`/trainings/sessions/${sessionId}/trainings`)
    return response.data
  },

  getTrainingById: async (id: number): Promise<Training> => {
    const response = await apiClient.get<Training>(`/trainings/trainings/${id}`)
    return response.data
  },

  createTraining: async (training: Omit<Training, 'id' | 'member' | 'motorcycle' | 'trainingSession'>): Promise<Training> => {
    const response = await apiClient.post<Training>('/trainings/trainings', training)
    return response.data
  },

  updateTraining: async (id: number, training: Partial<Omit<Training, 'member' | 'motorcycle' | 'trainingSession'>>): Promise<void> => {
    await apiClient.put(`/trainings/trainings/${id}`, { ...training, id })
  },

  deleteTraining: async (id: number): Promise<void> => {
    await apiClient.delete(`/trainings/trainings/${id}`)
  },
}
