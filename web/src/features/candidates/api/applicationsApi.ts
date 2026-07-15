import { apiClient } from '../../../shared/lib/apiClient'
import type { ApiResponse } from '../../../shared/types/api'
import type { Application, ApplicationSubmissionDetails, SetSubmissionDetailsRequest } from '../types'

function unwrap<T>(data: ApiResponse<T>, fallbackMessage: string): T {
  if (!data.data) throw new Error(data.error?.message ?? fallbackMessage)
  return data.data
}

export async function createApplication(jobId: string, candidateId: string): Promise<Application> {
  const response = await apiClient.post<ApiResponse<Application>>('/applications', { jobId, candidateId })
  return unwrap(response.data, 'Failed to create application.')
}

export async function setSubmissionDetails(
  applicationId: string,
  request: SetSubmissionDetailsRequest,
): Promise<Application> {
  const response = await apiClient.put<ApiResponse<Application>>(
    `/applications/${applicationId}/submission-details`,
    request,
  )
  return unwrap(response.data, 'Failed to save submission details.')
}

export async function getSubmissionDetails(applicationId: string): Promise<ApplicationSubmissionDetails> {
  const response = await apiClient.get<ApiResponse<ApplicationSubmissionDetails>>(
    `/applications/${applicationId}/submission-details`,
  )
  return unwrap(response.data, 'Failed to load submission details.')
}
