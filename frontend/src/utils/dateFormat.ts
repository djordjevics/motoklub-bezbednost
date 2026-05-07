/** ISO date or datetime string → DD/MM/YYYY for display. */
export function formatIsoDateDdMmYyyy(value?: string | null): string {
  if (!value) return ''
  const part = value.split('T')[0]
  const m = /^(\d{4})-(\d{2})-(\d{2})$/.exec(part)
  if (!m) return part
  return `${m[3]}/${m[2]}/${m[1]}`
}

export function formatIsoDateDdMmYyyyOrDash(value?: string | null): string {
  const s = formatIsoDateDdMmYyyy(value)
  return s || '-'
}

/** Local calendar date → YYYY-MM-DD for APIs and inputs. */
export function todayIsoDateString(): string {
  const d = new Date()
  const y = d.getFullYear()
  const mo = String(d.getMonth() + 1).padStart(2, '0')
  const da = String(d.getDate()).padStart(2, '0')
  return `${y}-${mo}-${da}`
}

/** Strip time from ISO datetime → YYYY-MM-DD. */
export function toIsoDatePart(value?: string | null): string {
  if (!value) return ''
  return value.split('T')[0]
}

/** Full ISO datetime → DD/MM/YYYY HH:mm (local). */
export function formatDateTimeDdMmYyyyHm(value?: string | null): string {
  if (!value) return ''
  const d = new Date(value)
  if (Number.isNaN(d.getTime())) return value
  const dd = String(d.getDate()).padStart(2, '0')
  const mm = String(d.getMonth() + 1).padStart(2, '0')
  const yyyy = d.getFullYear()
  const hh = String(d.getHours()).padStart(2, '0')
  const min = String(d.getMinutes()).padStart(2, '0')
  return `${dd}/${mm}/${yyyy} ${hh}:${min}`
}
