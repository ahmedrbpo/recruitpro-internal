import { apiClient } from '../../../shared/lib/apiClient'
import type { ApiResponse } from '../../../shared/types/api'

export interface AuthTokens {
  accessToken: string
  accessTokenExpiresAt: string
}

export async function login(email: string, password: string): Promise<AuthTokens> {
  const response = await apiClient.post<ApiResponse<AuthTokens>>('/auth/login', { email, password })
  if (!response.data.data) throw new Error(response.data.error?.message ?? 'Login failed.')
  return response.data.data
}

export async function logout(): Promise<void> {
  await apiClient.post('/auth/logout')
}
