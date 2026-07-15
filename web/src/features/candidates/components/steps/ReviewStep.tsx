import { useState, type ReactNode } from 'react'
import { Button } from '../../../../shared/components/Button'
import type {
  CandidateEducation,
  CandidateEmploymentHistory,
  PersonalInfoForm,
  Resume,
  SubmissionDetailsForm,
} from '../../types'

interface ReviewStepProps {
  personalInfo: PersonalInfoForm
  educations: CandidateEducation[]
  employments: CandidateEmploymentHistory[]
  submissionDetails: SubmissionDetailsForm
  resume: Resume | null
  onBack: () => void
  onSubmit: () => void
  isSubmitting: boolean
}

function ReviewSection({ title, children }: { title: string; children: ReactNode }) {
  return (
    <div className="rounded-lg border border-slate-200 p-4">
      <h3 className="mb-3 text-sm font-semibold text-brand-700">{title}</h3>
      {children}
    </div>
  )
}

function ReviewRow({ label, value }: { label: string; value: string }) {
  return (
    <div className="flex justify-between gap-4 py-1 text-sm">
      <span className="text-slate-500">{label}</span>
      <span className="text-right text-slate-900">{value || '—'}</span>
    </div>
  )
}

export function ReviewStep({
  personalInfo,
  educations,
  employments,
  submissionDetails,
  resume,
  onBack,
  onSubmit,
  isSubmitting,
}: ReviewStepProps) {
  const [declared, setDeclared] = useState(false)

  return (
    <div className="flex flex-col gap-4">
      <ReviewSection title="Personal information">
        <ReviewRow label="Name" value={`${personalInfo.firstName} ${personalInfo.lastName}`} />
        <ReviewRow label="Gender" value={personalInfo.gender} />
        <ReviewRow label="Date of birth" value={personalInfo.dateOfBirth} />
        <ReviewRow label="PAN" value={personalInfo.pan} />
        <ReviewRow label="Email" value={personalInfo.email} />
        <ReviewRow label="Phone" value={personalInfo.phone} />
        <ReviewRow label="Total experience" value={`${personalInfo.totalExperienceYears} yrs`} />
        <ReviewRow label="Relevant experience" value={`${personalInfo.relevantExperienceYears} yrs`} />
        <ReviewRow
          label="Address"
          value={[personalInfo.streetAddress, personalInfo.city, personalInfo.state, personalInfo.postalCode]
            .filter(Boolean)
            .join(', ')}
        />
        <ReviewRow label="Preferred work location" value={personalInfo.workLocation} />
      </ReviewSection>

      <ReviewSection title={`Education (${educations.length})`}>
        {educations.map((education) => (
          <ReviewRow key={education.id} label={education.degree} value={`${education.college}${education.stream ? ` · ${education.stream}` : ''}`} />
        ))}
      </ReviewSection>

      <ReviewSection title={`Employment history (${employments.length})`}>
        {employments.map((employment) => (
          <ReviewRow key={employment.id} label={employment.designation} value={employment.company} />
        ))}
      </ReviewSection>

      <ReviewSection title="Client & submission details">
        <ReviewRow label="Work type" value={submissionDetails.workType} />
        <ReviewRow label="Interview type" value={submissionDetails.interviewType} />
        <ReviewRow label="Current CTC" value={submissionDetails.currentCTC} />
        <ReviewRow label="Expected CTC" value={submissionDetails.expectedCTC} />
        <ReviewRow label="UAN number" value={submissionDetails.uanNumber} />
      </ReviewSection>

      <ReviewSection title="Resume">
        <ReviewRow label="File" value={resume?.originalFileName ?? '—'} />
      </ReviewSection>

      <label className="flex items-start gap-2 text-sm text-slate-700">
        <input
          type="checkbox"
          checked={declared}
          onChange={(e) => setDeclared(e.target.checked)}
          className="mt-0.5 h-4 w-4 rounded border-slate-300 text-brand-600 focus:ring-brand-500"
        />
        I confirm that all provided information is accurate.
      </label>

      <div className="flex justify-between">
        <Button type="button" variant="ghost" onClick={onBack}>
          Back
        </Button>
        <Button type="button" disabled={!declared} isLoading={isSubmitting} onClick={onSubmit}>
          Submit
        </Button>
      </div>
    </div>
  )
}
