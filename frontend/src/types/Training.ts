import type { Level } from './Level'
import type { Member, Motorcycle } from './Member'

export interface TrainingSession {
  id: number
  theoryDate?: string
  polygonDate?: string
  city?: string
  levelId?: number
  price?: number
  instructors?: string
  note?: string
  level?: Level
}

export interface Training {
  id: number
  memberId: number
  motorcycleId: number
  trainingSessionId: number
  repeatingAttendance: boolean
  isCertificateIssued: boolean
  note?: string
  member?: Member
  motorcycle?: Motorcycle
  trainingSession?: TrainingSession
}
