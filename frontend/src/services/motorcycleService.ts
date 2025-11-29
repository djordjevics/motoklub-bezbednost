import apiClient from './apiClient'
import { Motorcycle } from '../types/Motorcycle'

export const motorcycleService = {
  getAll: async (): Promise<Motorcycle[]> => {
    const response = await apiClient.get<Motorcycle[]>('/motorcycles')
    return response.data
  },

  getById: async (id: number): Promise<Motorcycle> => {
    const response = await apiClient.get<Motorcycle>(`/motorcycles/${id}`)
    return response.data
  },

  getByMemberId: async (memberId: number): Promise<Motorcycle[]> => {
    const response = await apiClient.get<Motorcycle[]>(`/motorcycles/member/${memberId}`)
    return response.data
  },

  create: async (motorcycle: Omit<Motorcycle, 'id'>): Promise<Motorcycle> => {
    const response = await apiClient.post<Motorcycle>('/motorcycles', motorcycle)
    return response.data
  },

  update: async (id: number, motorcycle: Partial<Motorcycle>): Promise<void> => {
    await apiClient.put(`/motorcycles/${id}`, { ...motorcycle, id })
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/motorcycles/${id}`)
  },
}

