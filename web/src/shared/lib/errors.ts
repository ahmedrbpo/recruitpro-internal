import axios from 'axios'
import type { ApiResponse } from '../types/api'

/** A ProblemDetails body (RFC 7807), as returned by ExceptionHandlingMiddleware for thrown
 * DomainExceptions — a different shape from the {success,data,error,meta} envelope that
 * Result<T> failures (validation, conflict, not-found) use. Both are 4xx JSON bodies, so a
 * single helper normalizes whichever one a given endpoint happened to produce. */
interface ProblemDetails {
  title?: string
  detail?: string
}

export function getApiErrorMessage(error: unknown, fallback: string): string {
  if (!axios.isAxiosError<ApiResponse<unknown> | ProblemDetails>(error)) return fallback

  const data = error.response?.data
  if (!data) return fallback

  if ('error' in data && data.error?.message) return data.error.message
  if ('title' in data && data.title) return data.title
  if ('detail' in data && data.detail) return data.detail

  return fallback
}
