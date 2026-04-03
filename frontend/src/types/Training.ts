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
  trainings?: Training[]
}

export interface Training {
  id: number
  memberId: number
  motorcycleId: number
  trainingSessionId: number
  isCertificateIssued: boolean
  note?: string
  member?: Member
  motorcycle?: Motorcycle
  trainingSession?: TrainingSession
}

export type TrainingRecord = Training

export interface Level {
  id: number
  name?: string
  note?: string
}

