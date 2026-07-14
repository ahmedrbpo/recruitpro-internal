import axios, { type InternalAxiosRequestConfig } from 'axios'
import { useAuthStore } from '../../app/store'
import { decodeAccessToken } from './jwt'
import type { ApiResponse } from '../types/api'

interface RetryableRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean
}

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7098/api/v1'

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  // Sends the httpOnly, Secure, SameSite=Strict refresh-token cookie the API sets on login.
  withCredentials: true,
})

apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().accessToken
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

interface RefreshResponseData {
  accessToken: string
  accessTokenExpiresAt: string
}

let refreshPromise: Promise<string | null> | null = null

/** Coalesces concurrent 401s into a single /auth/refresh call instead of one per failed
 * request. Also called once on app bootstrap (see App.tsx) to recover a session across a page
 * reload, since the access token itself only ever lives in memory. */
export function refreshAccessToken(): Promise<string | null> {
  refreshPromise ??= axios
    .post<ApiResponse<RefreshResponseData>>(`${API_BASE_URL}/auth/refresh`, null, { withCredentials: true })
    .then((response) => {
      const data = response.data.data
      if (!data) return null

      const { email, permissions } = decodeAccessToken(data.accessToken)
      useAuthStore.getState().setAuth({
        accessToken: data.accessToken,
        accessTokenExpiresAt: data.accessTokenExpiresAt,
        email,
        permissions,
      })
      return data.accessToken
    })
    .catch(() => {
      useAuthStore.getState().clearAuth()
      return null
    })
    .finally(() => {
      refreshPromise = null
    })

  return refreshPromise
}

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config as RetryableRequestConfig | undefined

    if (error.response?.status === 401 && originalRequest && !originalRequest._retry && !originalRequest.url?.includes('/auth/')) {
      originalRequest._retry = true
      const newToken = await refreshAccessToken()
      if (newToken) {
        originalRequest.headers.Authorization = `Bearer ${newToken}`
        return apiClient(originalRequest)
      }
    }

    return Promise.reject(error)
  },
)
