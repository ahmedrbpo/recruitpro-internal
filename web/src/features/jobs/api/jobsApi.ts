import { apiClient } from '../../../shared/lib/apiClient'
import type { ApiResponse } from '../../../shared/types/api'
import type { CreateJobRequest, Job } from '../types'

export interface JobsPage {
  items: Job[]
  page: number
  pageSize: number
  totalCount: number
}

export async function getJobs(page: number, pageSize: number): Promise<JobsPage> {
  const response = await apiClient.get<ApiResponse<Job[]>>('/jobs', { params: { page, pageSize } })
  return {
    items: response.data.data ?? [],
    page: response.data.meta?.page ?? page,
    pageSize: response.data.meta?.pageSize ?? pageSize,
    totalCount: response.data.meta?.totalCount ?? 0,
  }
}

export async function getJob(id: string): Promise<Job> {
  const response = await apiClient.get<ApiResponse<Job>>(`/jobs/${id}`)
  if (!response.data.data) throw new Error(response.data.error?.message ?? 'Failed to load job.')
  return response.data.data
}

export async function createJob(request: CreateJobRequest): Promise<Job> {
  const response = await apiClient.post<ApiResponse<Job>>('/jobs', request)
  if (!response.data.data) throw new Error(response.data.error?.message ?? 'Failed to create job.')
  return response.data.data
}

export async function publishJob(id: string): Promise<Job> {
  const response = await apiClient.post<ApiResponse<Job>>(`/jobs/${id}/publish`)
  if (!response.data.data) throw new Error(response.data.error?.message ?? 'Failed to publish job.')
  return response.data.data
}
