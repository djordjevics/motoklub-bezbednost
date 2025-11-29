export interface MembershipPayment {
  id: number
  memberId: number
  amount?: number
  paymentDate?: string
  paymentForYear?: number
  paymentTypeId?: number
  note?: string
  paymentType?: PaymentType
}

export interface PaymentType {
  id: number
  type?: string
  description?: string
}

