import axios from 'axios'
import { apiClient } from '../../../shared/lib/apiClient'
import type { ApiResponse } from '../../../shared/types/api'
import type {
  AddEducationRequest,
  AddEmploymentRequest,
  Candidate,
  CandidateEducation,
  CandidateEmploymentHistory,
  CreateCandidateRequest,
  Resume,
  UpdateCandidateRequest,
} from '../types'

function unwrap<T>(data: ApiResponse<T>, fallbackMessage: string): T {
  if (!data.data) throw new Error(data.error?.message ?? fallbackMessage)
  return data.data
}

export async function createCandidate(request: CreateCandidateRequest): Promise<Candidate> {
  const response = await apiClient.post<ApiResponse<Candidate>>('/candidates', request)
  return unwrap(response.data, 'Failed to create candidate.')
}

export async function updateCandidate(candidateId: string, request: UpdateCandidateRequest): Promise<Candidate> {
  const response = await apiClient.put<ApiResponse<Candidate>>(`/candidates/${candidateId}`, request)
  return unwrap(response.data, 'Failed to update candidate.')
}

export async function getCandidate(candidateId: string): Promise<Candidate> {
  const response = await apiClient.get<ApiResponse<Candidate>>(`/candidates/${candidateId}`)
  return unwrap(response.data, 'Failed to load candidate.')
}

export async function addEducation(candidateId: string, request: AddEducationRequest): Promise<CandidateEducation> {
  const response = await apiClient.post<ApiResponse<CandidateEducation>>(`/candidates/${candidateId}/education`, request)
  return unwrap(response.data, 'Failed to add education.')
}

export async function removeEducation(candidateId: string, educationId: string): Promise<void> {
  await apiClient.delete(`/candidates/${candidateId}/education/${educationId}`)
}

export async function addEmployment(
  candidateId: string,
  request: AddEmploymentRequest,
): Promise<CandidateEmploymentHistory> {
  const response = await apiClient.post<ApiResponse<CandidateEmploymentHistory>>(
    `/candidates/${candidateId}/employment`,
    request,
  )
  return unwrap(response.data, 'Failed to add employment history.')
}

export async function removeEmployment(candidateId: string, employmentId: string): Promise<void> {
  await apiClient.delete(`/candidates/${candidateId}/employment/${employmentId}`)
}

export async function requestResumeUploadUrl(
  candidateId: string,
  file: File,
): Promise<{ resumeId: string; uploadUrl: string }> {
  const response = await apiClient.post<ApiResponse<{ resumeId: string; uploadUrl: string }>>(
    `/candidates/${candidateId}/resumes/upload-url`,
    { originalFileName: file.name, contentType: file.type, sizeBytes: file.size },
  )
  return unwrap(response.data, 'Failed to request a resume upload URL.')
}

// Uploads direct to Supabase Storage via the presigned URL — never through the API — so this
// intentionally uses a bare axios call, bypassing apiClient's baseURL/auth-header interceptors.
export async function uploadFileToPresignedUrl(uploadUrl: string, file: File): Promise<void> {
  await axios.put(uploadUrl, file, { headers: { 'Content-Type': file.type } })
}

export async function confirmResumeUpload(candidateId: string, resumeId: string): Promise<Resume> {
  const response = await apiClient.post<ApiResponse<Resume>>(`/candidates/${candidateId}/resumes/${resumeId}/confirm`)
  return unwrap(response.data, 'Failed to confirm resume upload.')
}
