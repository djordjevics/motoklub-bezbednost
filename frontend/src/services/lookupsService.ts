import apiClient from './apiClient'
import type { MemberType } from '../types/MemberType'
import type { Level } from '../types/Level'
import type { PaymentType } from '../types/MembershipPayment'

export const lookupsService = {
  getMemberTypes: async (): Promise<MemberType[]> => {
    const response = await apiClient.get<MemberType[]>('/membertypes')
    return response.data
  },
  getLevels: async (): Promise<Level[]> => {
    const response = await apiClient.get<Level[]>('/levels')
    return response.data
  },
  getPaymentTypes: async (): Promise<PaymentType[]> => {
    const response = await apiClient.get<PaymentType[]>('/paymenttypes')
    return response.data
  },
}
