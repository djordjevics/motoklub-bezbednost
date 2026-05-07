import { MemberType } from './MemberType'
import { MembershipPayment } from './MembershipPayment'
import { Comment } from './Comment'
import { Tag } from './Tag'
import type { Training } from './Training'

export interface Member {
  id: number
  name: string
  surname: string
  jmbg?: string
  dateOfBirth?: string
  workplace?: string
  mobilePhone?: string
  emergencyContact?: string
  emergencyContactPhone?: string
  email?: string
  address?: string
  registeredOn?: string
  memberTypeId?: number
  membershipExemptManual?: boolean
  /** Aktiv + 65+: no membership fee obligation; person may still be an active member roster-wise. */
  isMembershipPaymentExemptDueToAge?: boolean
  /**
   * Derived by API: membership payment is required (group + age rules),
   * and always false for inactive/archived members.
   */
  isMembershipPaymentRequired?: boolean
  note?: string
  motorcycles?: Motorcycle[]
  equipment?: Equipment
  trainings?: Training[]
  membershipPayments?: MembershipPayment[]
  comments?: Comment[]
  tags?: Tag[]
  memberType?: MemberType
}

export interface Motorcycle {
  id: number
  memberId: number
  brandName?: string
  commercialName?: string
  modelName?: string
  engineDisplacment?: number
  enginePower?: number
  color?: string
  registerPlate?: string
}

export interface Equipment {
  id: number
  memberId: number
  pants: boolean
  jacket: boolean
  vest: boolean
  workShirt: boolean
  formalShirt: boolean
  note?: string
}
