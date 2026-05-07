/**
 * The backend stores member-type colors as 8-character ARGB hex strings (e.g. "#FF0288D1").
 * MUI / CSS expect either #RRGGBB or #RRGGBBAA (RGBA), so this helper normalizes
 * the value so it can be passed to `sx`, `style`, etc.
 */
export function normalizeMemberTypeColor(color?: string | null): string | undefined {
  if (!color) return undefined
  const trimmed = color.trim()
  if (!trimmed.startsWith('#')) return trimmed
  const hex = trimmed.slice(1)

  // #ARGB (rare) or #AARRGGBB -> reorder to #RRGGBB / #RRGGBBAA
  if (hex.length === 8) {
    const a = hex.slice(0, 2)
    const rgb = hex.slice(2)
    return `#${rgb}${a}`
  }
  return trimmed
}

/** Convenience wrapper that always returns a usable color, falling back to MUI grey. */
export function memberTypeColorOrFallback(color?: string | null, fallback = '#9E9E9E'): string {
  return normalizeMemberTypeColor(color) ?? fallback
}
