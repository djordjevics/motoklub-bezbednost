import apiClient from './apiClient'
import { TrainingSession, TrainingRecord } from '../types/Training'

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

  createSession: async (session: Omit<TrainingSession, 'id' | 'createdAt'>): Promise<TrainingSession> => {
    const response = await apiClient.post<TrainingSession>('/trainings/sessions', session)
    return response.data
  },

  updateSession: async (id: number, session: Partial<TrainingSession>): Promise<void> => {
    await apiClient.put(`/trainings/sessions/${id}`, { ...session, id })
  },

  deleteSession: async (id: number): Promise<void> => {
    await apiClient.delete(`/trainings/sessions/${id}`)
  },

  // Training Records
  getRecordsByMemberId: async (memberId: number): Promise<TrainingRecord[]> => {
    const response = await apiClient.get<TrainingRecord[]>(`/trainings/member/${memberId}`)
    return response.data
  },

  getRecordById: async (id: number): Promise<TrainingRecord> => {
    const response = await apiClient.get<TrainingRecord>(`/trainings/records/${id}`)
    return response.data
  },

  createRecord: async (record: Omit<TrainingRecord, 'id' | 'createdAt'>): Promise<TrainingRecord> => {
    const response = await apiClient.post<TrainingRecord>('/trainings/records', record)
    return response.data
  },
}

