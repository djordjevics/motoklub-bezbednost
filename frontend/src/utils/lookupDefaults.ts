import type { Level } from '../types/Level'
import type { MemberType } from '../types/MemberType'
import type { PaymentType } from '../types/MembershipPayment'

function norm(s?: string): string {
  return (s ?? '').trim().toLowerCase()
}

export function defaultMemberTypeId(types: MemberType[] | undefined): number | undefined {
  if (!types?.length) return undefined
  const aktiv = types.find((t) => norm(t.typeName) === 'aktiv')
  return aktiv?.id ?? types[0]?.id
}

export function defaultPaymentTypeId(types: PaymentType[] | undefined): number | undefined {
  if (!types?.length) return undefined
  const cash = types.find((t) => norm(t.type) === 'gotovina')
  return cash?.id ?? types[0]?.id
}

export function defaultLevelIdString(levels: Level[] | undefined): string {
  if (!levels?.length) return ''
  const osnovni = levels.find((l) => norm(l.name) === 'osnovni')
  const id = osnovni?.id ?? levels[0]?.id
  return id != null ? String(id) : ''
}
