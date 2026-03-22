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

