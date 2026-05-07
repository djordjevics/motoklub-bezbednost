import apiClient from './apiClient'
import type { MembershipPayment } from '../types/MembershipPayment'

export const membershipPaymentService = {
  getByMember: async (memberId: number): Promise<MembershipPayment[]> => {
    const response = await apiClient.get<MembershipPayment[]>(`/membershippayments/member/${memberId}`)
    return response.data
  },
  create: async (payment: Omit<MembershipPayment, 'id' | 'paymentType'>): Promise<MembershipPayment> => {
    const response = await apiClient.post<MembershipPayment>('/membershippayments', payment)
    return response.data
  },
  update: async (id: number, payment: Omit<MembershipPayment, 'id' | 'paymentType'>): Promise<void> => {
    await apiClient.put(`/membershippayments/${id}`, { ...payment, id })
  },
  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/membershippayments/${id}`)
  },
}
