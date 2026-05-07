import type { Motorcycle } from '../types/Member'

/** "brand model licencePlate" for dropdowns and tables. */
export function formatMotorcycleBrandModelPlate(m?: Motorcycle | null): string {
  if (!m) return '-'
  const brand = (m.brandName ?? '').trim()
  const model = (m.modelName ?? '').trim()
  const plate = (m.registerPlate ?? '').trim()
  const parts = [brand, model, plate].filter(Boolean)
  return parts.length ? parts.join(' ') : `#${m.id}`
}
