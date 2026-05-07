import type { Tag } from '../types/Tag'
import type { Member } from '../types/Member'

const stripDate = (value?: string): string => (value ? value.split('T')[0] : '')

/** True if the tag's validity interval includes today; null if validity is undefined. */
export function isTagActive(tag: Tag): boolean | null {
  if (!tag.validFrom && !tag.validTo) return null
  const today = new Date()
  today.setHours(0, 0, 0, 0)
  const from = tag.validFrom ? new Date(stripDate(tag.validFrom)) : null
  const to = tag.validTo ? new Date(stripDate(tag.validTo)) : null
  if (from && today < from) return false
  if (to && today > to) return false
  return true
}

/**
 * Today's active club tag formatted as `{memberTypeId} - {tagNumber}`.
 * When several tags are active, prefers the one with the latest ValidFrom (then highest id).
 */
export function formatActiveMemberTagLabel(member: Member): string | null {
  const tags = member.tags ?? []
  const actives = tags.filter((t) => isTagActive(t) === true)
  if (actives.length === 0) return null

  const sorted = [...actives].sort((a, b) => {
    const af = stripDate(a.validFrom)
    const bf = stripDate(b.validFrom)
    if (af !== bf) return bf.localeCompare(af)
    return b.id - a.id
  })
  const tag = sorted[0]
  const typePart = member.memberTypeId != null ? String(member.memberTypeId) : '—'
  const numPart = tag.tagNumber != null ? String(tag.tagNumber) : '—'
  return `${typePart} - ${numPart}`
}
