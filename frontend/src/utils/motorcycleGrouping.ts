import type { Member, Motorcycle } from '../types/Member'

/** Normalize for grouping equality (brand + model). */
export function motorcycleGroupNormKey(brandName?: string | null, modelName?: string | null): string {
  const b = (brandName ?? '').trim().toLowerCase()
  const m = (modelName ?? '').trim().toLowerCase()
  return `${b}\u241e${m}`
}

/** URL segment: empty parts use "-" placeholder. */
export function encodeMotorcycleGroupSegment(part: string): string {
  const t = part.trim()
  return t === '' ? '-' : encodeURIComponent(t)
}

export function decodeMotorcycleGroupSegment(seg: string): string {
  return seg === '-' ? '' : decodeURIComponent(seg)
}

export interface MotorcycleWithOwner {
  motorcycle: Motorcycle
  member: Member
}

/** One row per distinct brand + model among all members’ motorcycles. */
export interface MotorcycleModelGroupSummary {
  normKey: string
  /** Display strings from first occurrence */
  brandName: string
  modelName: string
  memberCount: number
}

export interface MotorcycleModelGroupDetail extends MotorcycleModelGroupSummary {
  items: MotorcycleWithOwner[]
}

function displayBrandModel(brand: string, model: string): { brandName: string; modelName: string } {
  return {
    brandName: brand.trim() === '' ? '—' : brand.trim(),
    modelName: model.trim() === '' ? '—' : model.trim(),
  }
}

export function buildMotorcycleModelGroups(members: Member[]): MotorcycleModelGroupDetail[] {
  const map = new Map<
    string,
    {
      brandName: string
      modelName: string
      memberIds: Set<number>
      items: MotorcycleWithOwner[]
    }
  >()

  for (const member of members ?? []) {
    for (const motorcycle of member.motorcycles ?? []) {
      const rawB = motorcycle.brandName ?? ''
      const rawM = motorcycle.modelName ?? ''
      const k = motorcycleGroupNormKey(rawB, rawM)
      const disp = displayBrandModel(rawB, rawM)

      let g = map.get(k)
      if (!g) {
        g = {
          brandName: disp.brandName,
          modelName: disp.modelName,
          memberIds: new Set<number>(),
          items: [],
        }
        map.set(k, g)
      }
      g.memberIds.add(member.id)
      g.items.push({ motorcycle, member })
    }
  }

  return [...map.entries()]
    .map(([normKey, g]) => ({
      normKey,
      brandName: g.brandName,
      modelName: g.modelName,
      memberCount: g.memberIds.size,
      items: g.items.sort((a, b) =>
        `${a.member.surname} ${a.member.name}`.localeCompare(`${b.member.surname} ${b.member.name}`),
      ),
    }))
    .sort((a, b) =>
      `${a.brandName} ${a.modelName}`.localeCompare(`${b.brandName} ${b.modelName}`, undefined, {
        sensitivity: 'base',
      }),
    )
}

export function findMotorcycleModelGroup(
  groups: MotorcycleModelGroupDetail[],
  brandDecoded: string,
  modelDecoded: string,
): MotorcycleModelGroupDetail | undefined {
  const k = motorcycleGroupNormKey(brandDecoded, modelDecoded)
  return groups.find((g) => g.normKey === k)
}
