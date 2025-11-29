import apiClient from './apiClient'
import { Equipment } from '../types/Equipment'

export const equipmentService = {
  getAll: async (): Promise<Equipment[]> => {
    const response = await apiClient.get<Equipment[]>('/equipment')
    return response.data
  },

  getById: async (id: number): Promise<Equipment> => {
    const response = await apiClient.get<Equipment>(`/equipment/${id}`)
    return response.data
  },

  getByMemberId: async (memberId: number): Promise<Equipment[]> => {
    const response = await apiClient.get<Equipment[]>(`/equipment/member/${memberId}`)
    return response.data
  },

  create: async (equipment: Omit<Equipment, 'id'>): Promise<Equipment> => {
    const response = await apiClient.post<Equipment>('/equipment', equipment)
    return response.data
  },

  update: async (id: number, equipment: Partial<Equipment>): Promise<void> => {
    await apiClient.put(`/equipment/${id}`, { ...equipment, id })
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/equipment/${id}`)
  },
}

