import { create } from 'zustand'

interface AuthState {
  accessToken: string | null
  accessTokenExpiresAt: string | null
  email: string | null
  permissions: string[]
  setAuth: (auth: { accessToken: string; accessTokenExpiresAt: string; email: string; permissions: string[] }) => void
  clearAuth: () => void
}

// Access token lives in memory only, per the blueprint's XSS-reduction rule ("never
// localStorage") — a page refresh loses it, and the axios client's 401 interceptor calls
// /auth/refresh (backed by the httpOnly refresh cookie) to silently recover it.
export const useAuthStore = create<AuthState>((set) => ({
  accessToken: null,
  accessTokenExpiresAt: null,
  email: null,
  permissions: [],
  setAuth: ({ accessToken, accessTokenExpiresAt, email, permissions }) =>
    set({ accessToken, accessTokenExpiresAt, email, permissions }),
  clearAuth: () => set({ accessToken: null, accessTokenExpiresAt: null, email: null, permissions: [] }),
}))
