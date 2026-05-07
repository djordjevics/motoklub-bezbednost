import apiClient from './apiClient'
import type { Tag } from '../types/Tag'

export const tagService = {
  getByMember: async (memberId: number): Promise<Tag[]> => {
    const response = await apiClient.get<Tag[]>(`/tags/member/${memberId}`)
    return response.data
  },
  create: async (tag: Omit<Tag, 'id'>): Promise<Tag> => {
    const response = await apiClient.post<Tag>('/tags', tag)
    return response.data
  },
  update: async (id: number, tag: Partial<Tag>): Promise<void> => {
    await apiClient.put(`/tags/${id}`, { ...tag, id })
  },
  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/tags/${id}`)
  },
}
