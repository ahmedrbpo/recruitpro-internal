import { useMutation, useQuery } from '@tanstack/react-query'
import {
  addEducation,
  addEmployment,
  confirmResumeUpload,
  createCandidate,
  removeEducation,
  removeEmployment,
  requestResumeUploadUrl,
  updateCandidate,
  uploadFileToPresignedUrl,
} from '../api/candidatesApi'
import { createApplication, getSubmissionDetails, setSubmissionDetails } from '../api/applicationsApi'
import type { AddEducationRequest, AddEmploymentRequest, CreateCandidateRequest, SetSubmissionDetailsRequest, UpdateCandidateRequest } from '../types'

export function useCreateCandidate() {
  return useMutation({ mutationFn: (request: CreateCandidateRequest) => createCandidate(request) })
}

export function useUpdateCandidate() {
  return useMutation({
    mutationFn: (args: { candidateId: string; request: UpdateCandidateRequest }) =>
      updateCandidate(args.candidateId, args.request),
  })
}

export function useCreateApplication() {
  return useMutation({
    mutationFn: (args: { jobId: string; candidateId: string }) => createApplication(args.jobId, args.candidateId),
  })
}

export function useAddEducation() {
  return useMutation({
    mutationFn: (args: { candidateId: string; request: AddEducationRequest }) =>
      addEducation(args.candidateId, args.request),
  })
}

export function useRemoveEducation() {
  return useMutation({
    mutationFn: (args: { candidateId: string; educationId: string }) =>
      removeEducation(args.candidateId, args.educationId),
  })
}

export function useAddEmployment() {
  return useMutation({
    mutationFn: (args: { candidateId: string; request: AddEmploymentRequest }) =>
      addEmployment(args.candidateId, args.request),
  })
}

export function useRemoveEmployment() {
  return useMutation({
    mutationFn: (args: { candidateId: string; employmentId: string }) =>
      removeEmployment(args.candidateId, args.employmentId),
  })
}

export function useSubmissionDetails(applicationId: string | null) {
  return useQuery({
    queryKey: ['application-submission-details', applicationId],
    queryFn: () => getSubmissionDetails(applicationId!),
    enabled: !!applicationId,
  })
}

export function useSetSubmissionDetails() {
  return useMutation({
    mutationFn: (args: { applicationId: string; request: SetSubmissionDetailsRequest }) =>
      setSubmissionDetails(args.applicationId, args.request),
  })
}

/** Requests a presigned URL, uploads the file directly to storage, then confirms the upload —
 * the three-step flow the API's resume endpoints are designed around (files never pass through
 * the ASP.NET Core process). */
export function useUploadResume() {
  return useMutation({
    mutationFn: async (args: { candidateId: string; file: File }) => {
      const { resumeId, uploadUrl } = await requestResumeUploadUrl(args.candidateId, args.file)
      await uploadFileToPresignedUrl(uploadUrl, args.file)
      return confirmResumeUpload(args.candidateId, resumeId)
    },
  })
}
