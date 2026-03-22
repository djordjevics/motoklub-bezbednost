const STORAGE_KEY = 'motoklub_access_token'

export function getAccessToken(): string | null {
  return localStorage.getItem(STORAGE_KEY)
}

export function setAccessToken(token: string): void {
  localStorage.setItem(STORAGE_KEY, token)
}

export function clearAccessToken(): void {
  localStorage.removeItem(STORAGE_KEY)
}
