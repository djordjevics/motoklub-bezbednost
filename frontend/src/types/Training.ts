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
  member?: any
  motorcycle?: any
  trainingSession?: TrainingSession
}

export interface Level {
  id: number
  name?: string
  note?: string
}

