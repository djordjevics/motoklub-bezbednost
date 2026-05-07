import apiClient from './apiClient'
import type { Comment } from '../types/Comment'

export const commentService = {
  getByMember: async (memberId: number): Promise<Comment[]> => {
    const response = await apiClient.get<Comment[]>(`/comments/member/${memberId}`)
    return response.data
  },
  create: async (comment: Pick<Comment, 'memberId' | 'commentText'>): Promise<Comment> => {
    const response = await apiClient.post<Comment>('/comments', comment)
    return response.data
  },
  update: async (id: number, comment: Pick<Comment, 'commentText'>): Promise<void> => {
    await apiClient.put(`/comments/${id}`, { ...comment, id })
  },
  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/comments/${id}`)
  },
}
