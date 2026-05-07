import apiClient from './apiClient'
import { Member } from '../types/Member'

export const memberService = {
  getAll: async (): Promise<Member[]> => {
    const response = await apiClient.get<Member[]>('/members')
    return response.data
  },

  getById: async (id: number): Promise<Member> => {
    const response = await apiClient.get<Member>(`/members/${id}`)
    return response.data
  },

  create: async (member: Omit<Member, 'id'>): Promise<Member> => {
    const response = await apiClient.post<Member>('/members', member)
    return response.data
  },

  update: async (id: number, member: Partial<Member>): Promise<void> => {
    await apiClient.put(`/members/${id}`, { ...member, id })
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/members/${id}`)
  },

  search: async (query: string): Promise<Member[]> => {
    const response = await apiClient.get<Member[]>(`/members/search?query=${encodeURIComponent(query)}`)
    return response.data
  },
}
