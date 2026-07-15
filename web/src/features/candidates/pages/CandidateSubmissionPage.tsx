import { useState } from 'react'
import { Link, Navigate, useParams } from 'react-router-dom'
import { useJob } from '../../jobs/hooks/useJob'
import { StepIndicator } from '../components/StepIndicator'
import { PersonalInfoStep } from '../components/steps/PersonalInfoStep'
import { EducationStep } from '../components/steps/EducationStep'
import { EmploymentStep } from '../components/steps/EmploymentStep'
import { SubmissionDetailsStep } from '../components/steps/SubmissionDetailsStep'
import { ResumeUploadStep } from '../components/steps/ResumeUploadStep'
import { ReviewStep } from '../components/steps/ReviewStep'
import { Button } from '../../../shared/components/Button'
import { getApiErrorMessage } from '../../../shared/lib/errors'
import {
  useAddEducation,
  useAddEmployment,
  useCreateApplication,
  useCreateCandidate,
  useRemoveEducation,
  useRemoveEmployment,
  useSetSubmissionDetails,
  useSubmissionDetails,
  useUpdateCandidate,
  useUploadResume,
} from '../hooks/useCandidateSubmission'
import {
  emptyPersonalInfoForm,
  emptySubmissionDetailsForm,
  type CandidateEducation,
  type CandidateEmploymentHistory,
  type EducationForm,
  type EmploymentForm,
  type PersonalInfoForm,
  type Resume,
  type SubmissionDetailsForm,
} from '../types'

// <input type="month"> yields "YYYY-MM"; the API's DateOnly fields need a full date, with the
// day fixed to 1 per CandidateEducation/CandidateEmploymentHistory's month/year granularity.
function toApiDate(monthValue: string): string {
  return `${monthValue}-01`
}

export function CandidateSubmissionPage() {
  const { jobId } = useParams<{ jobId: string }>()
  const { data: job } = useJob(jobId)

  const [step, setStep] = useState(1)
  const [candidateId, setCandidateId] = useState<string | null>(null)
  const [applicationId, setApplicationId] = useState<string | null>(null)
  const [isSubmitted, setIsSubmitted] = useState(false)

  const [personalInfo, setPersonalInfo] = useState<PersonalInfoForm>(emptyPersonalInfoForm)
  const [educations, setEducations] = useState<CandidateEducation[]>([])
  const [employments, setEmployments] = useState<CandidateEmploymentHistory[]>([])
  const [submissionDetails, setSubmissionDetails] = useState<SubmissionDetailsForm>(emptySubmissionDetailsForm)
  const [resume, setResume] = useState<Resume | null>(null)

  const createCandidate = useCreateCandidate()
  const updateCandidate = useUpdateCandidate()
  const createApplication = useCreateApplication()
  const addEducation = useAddEducation()
  const removeEducation = useRemoveEducation()
  const addEmployment = useAddEmployment()
  const removeEmployment = useRemoveEmployment()
  const submissionDetailsQuery = useSubmissionDetails(applicationId)
  const setSubmissionDetailsMutation = useSetSubmissionDetails()
  const uploadResume = useUploadResume()

  if (!jobId) return <Navigate to="/jobs" />

  async function handlePersonalInfoNext() {
    const candidate = await createCandidate.mutateAsync({
      firstName: personalInfo.firstName,
      lastName: personalInfo.lastName,
      email: personalInfo.email,
      phone: personalInfo.phone || null,
    })

    await updateCandidate.mutateAsync({
      candidateId: candidate.id,
      request: {
        gender: personalInfo.gender || null,
        dateOfBirth: personalInfo.dateOfBirth || null,
        pan: personalInfo.pan || null,
        totalExperienceYears: personalInfo.totalExperienceYears ? Number(personalInfo.totalExperienceYears) : null,
        relevantExperienceYears: personalInfo.relevantExperienceYears
          ? Number(personalInfo.relevantExperienceYears)
          : null,
        streetAddress: personalInfo.streetAddress || null,
        city: personalInfo.city || null,
        state: personalInfo.state || null,
        postalCode: personalInfo.postalCode || null,
        workLocation: personalInfo.workLocation || null,
      },
    })

    const application = await createApplication.mutateAsync({ jobId: jobId!, candidateId: candidate.id })

    setCandidateId(candidate.id)
    setApplicationId(application.id)
    setStep(2)
  }

  async function handleAddEducation(form: EducationForm) {
    if (!candidateId || !form.type) return
    const education = await addEducation.mutateAsync({
      candidateId,
      request: {
        college: form.college,
        degree: form.degree,
        stream: form.stream || null,
        type: form.type,
        startDate: toApiDate(form.startDate),
        endDate: form.endDate ? toApiDate(form.endDate) : null,
        location: form.location || null,
      },
    })
    setEducations((prev) => [...prev, education])
  }

  async function handleRemoveEducation(educationId: string) {
    if (!candidateId) return
    await removeEducation.mutateAsync({ candidateId, educationId })
    setEducations((prev) => prev.filter((e) => e.id !== educationId))
  }

  async function handleAddEmployment(form: EmploymentForm) {
    if (!candidateId || !form.type) return
    const employment = await addEmployment.mutateAsync({
      candidateId,
      request: {
        payrollCompany: form.payrollCompany,
        company: form.company,
        designation: form.designation,
        type: form.type,
        startDate: toApiDate(form.startDate),
        endDate: form.endDate ? toApiDate(form.endDate) : null,
        location: form.location || null,
      },
    })
    setEmployments((prev) => [...prev, employment])
  }

  async function handleRemoveEmployment(employmentId: string) {
    if (!candidateId) return
    await removeEmployment.mutateAsync({ candidateId, employmentId })
    setEmployments((prev) => prev.filter((e) => e.id !== employmentId))
  }

  async function handleSubmissionDetailsNext() {
    if (!applicationId || !submissionDetails.workType || !submissionDetails.interviewType) return
    await setSubmissionDetailsMutation.mutateAsync({
      applicationId,
      request: {
        workType: submissionDetails.workType,
        interviewType: submissionDetails.interviewType,
        currentCTC: Number(submissionDetails.currentCTC),
        expectedCTC: Number(submissionDetails.expectedCTC),
        uanNumber: submissionDetails.uanNumber || null,
      },
    })
    setStep(5)
  }

  async function handleResumeUpload(file: File) {
    if (!candidateId) return
    const uploaded = await uploadResume.mutateAsync({ candidateId, file })
    setResume(uploaded)
  }

  const submissionErrorMessage = setSubmissionDetailsMutation.isError
    ? getApiErrorMessage(setSubmissionDetailsMutation.error, 'Failed to save submission details.')
    : null

  if (isSubmitted) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-50 p-8">
        <div className="w-full max-w-md rounded-xl bg-white p-8 text-center shadow-sm">
          <h1 className="mb-2 text-xl font-semibold text-brand-700">Candidate submitted</h1>
          <p className="mb-6 text-sm text-slate-500">
            {personalInfo.firstName} {personalInfo.lastName} has been submitted for {job?.title ?? 'this role'}.
          </p>
          <Link to="/jobs">
            <Button>Back to jobs</Button>
          </Link>
        </div>
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-slate-50 p-8">
      <div className="mx-auto max-w-3xl">
        <div className="mb-6">
          <Link to="/jobs" className="text-sm text-brand-600 hover:underline">
            ← Back to jobs
          </Link>
          <h1 className="mt-1 text-2xl font-semibold text-brand-700">Submit candidate</h1>
          {job && <p className="text-sm text-slate-500">For {job.title} ({job.jobCode})</p>}
        </div>

        <div className="rounded-lg bg-white p-6 shadow-sm">
          <StepIndicator currentStep={step} />

          {step === 1 && (
            <PersonalInfoStep
              role={job?.title ?? ''}
              value={personalInfo}
              onChange={setPersonalInfo}
              onNext={handlePersonalInfoNext}
              isSubmitting={createCandidate.isPending || updateCandidate.isPending || createApplication.isPending}
              errorMessage={
                createCandidate.isError
                  ? getApiErrorMessage(createCandidate.error, 'Failed to create candidate.')
                  : updateCandidate.isError
                    ? getApiErrorMessage(updateCandidate.error, 'Failed to save personal details.')
                    : createApplication.isError
                      ? getApiErrorMessage(createApplication.error, 'Failed to create application.')
                      : null
              }
            />
          )}

          {step === 2 && (
            <EducationStep
              educations={educations}
              onAdd={handleAddEducation}
              onRemove={handleRemoveEducation}
              onNext={() => setStep(3)}
              onBack={() => setStep(1)}
              isAdding={addEducation.isPending}
              errorMessage={addEducation.isError ? getApiErrorMessage(addEducation.error, 'Failed to add education.') : null}
            />
          )}

          {step === 3 && (
            <EmploymentStep
              employments={employments}
              onAdd={handleAddEmployment}
              onRemove={handleRemoveEmployment}
              onNext={() => setStep(4)}
              onBack={() => setStep(2)}
              isAdding={addEmployment.isPending}
              errorMessage={
                addEmployment.isError ? getApiErrorMessage(addEmployment.error, 'Failed to add employment history.') : null
              }
            />
          )}

          {step === 4 && (
            <SubmissionDetailsStep
              clientName={submissionDetailsQuery.data?.clientName ?? null}
              requirementId={submissionDetailsQuery.data?.requirementId ?? job?.jobCode ?? ''}
              onboarding={submissionDetailsQuery.data?.onboarding ?? null}
              value={submissionDetails}
              onChange={setSubmissionDetails}
              onNext={handleSubmissionDetailsNext}
              onBack={() => setStep(3)}
              isSubmitting={setSubmissionDetailsMutation.isPending}
              errorMessage={submissionErrorMessage}
            />
          )}

          {step === 5 && (
            <ResumeUploadStep
              resume={resume}
              onUpload={handleResumeUpload}
              onNext={() => setStep(6)}
              onBack={() => setStep(4)}
              isUploading={uploadResume.isPending}
              errorMessage={uploadResume.isError ? getApiErrorMessage(uploadResume.error, 'Failed to upload resume.') : null}
            />
          )}

          {step === 6 && (
            <ReviewStep
              personalInfo={personalInfo}
              educations={educations}
              employments={employments}
              submissionDetails={submissionDetails}
              resume={resume}
              onBack={() => setStep(5)}
              onSubmit={() => setIsSubmitted(true)}
              isSubmitting={false}
            />
          )}
        </div>
      </div>
    </div>
  )
}
