import type { FormEvent } from 'react'
import { FormField } from '../../../../shared/components/FormField'
import { TextInput } from '../../../../shared/components/TextInput'
import { Select } from '../../../../shared/components/Select'
import { Button } from '../../../../shared/components/Button'
import type { SubmissionDetailsForm } from '../../types'

const onboardingLabels: Record<string, string> = {
  ContractToHire: 'Contract-to-Hire',
  EmployerPayroll: 'Employer Payroll',
}

interface SubmissionDetailsStepProps {
  clientName: string | null
  requirementId: string
  onboarding: string | null
  value: SubmissionDetailsForm
  onChange: (value: SubmissionDetailsForm) => void
  onNext: () => void
  onBack: () => void
  isSubmitting: boolean
  errorMessage: string | null
}

export function SubmissionDetailsStep({
  clientName,
  requirementId,
  onboarding,
  value,
  onChange,
  onNext,
  onBack,
  isSubmitting,
  errorMessage,
}: SubmissionDetailsStepProps) {
  function set<K extends keyof SubmissionDetailsForm>(key: K, fieldValue: SubmissionDetailsForm[K]) {
    onChange({ ...value, [key]: fieldValue })
  }

  function handleSubmit(event: FormEvent) {
    event.preventDefault()
    onNext()
  }

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4">
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="Client name" htmlFor="clientName">
          <TextInput id="clientName" readOnly disabled value={clientName ?? '—'} className="bg-slate-50 text-slate-500" />
        </FormField>
        <FormField label="Requirement ID" htmlFor="requirementId">
          <TextInput id="requirementId" readOnly disabled value={requirementId} className="bg-slate-50 text-slate-500" />
        </FormField>
      </div>

      <FormField label="Onboarding type" htmlFor="onboardingType">
        <TextInput
          id="onboardingType"
          readOnly
          disabled
          value={onboarding ? (onboardingLabels[onboarding] ?? onboarding) : '—'}
          className="bg-slate-50 text-slate-500"
        />
      </FormField>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="Work type" htmlFor="workType">
          <Select id="workType" required value={value.workType} onChange={(e) => set('workType', e.target.value as SubmissionDetailsForm['workType'])}>
            <option value="" disabled>
              Select
            </option>
            <option value="Remote">Remote</option>
            <option value="WorkFromOffice">Work From Office</option>
            <option value="Hybrid">Hybrid</option>
          </Select>
        </FormField>
        <FormField label="Interview type" htmlFor="interviewType">
          <Select
            id="interviewType"
            required
            value={value.interviewType}
            onChange={(e) => set('interviewType', e.target.value as SubmissionDetailsForm['interviewType'])}
          >
            <option value="" disabled>
              Select
            </option>
            <option value="Virtual">Virtual</option>
            <option value="FaceToFace">Face-to-Face</option>
            <option value="Telephonic">Telephonic</option>
          </Select>
        </FormField>
      </div>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="Current CTC (annual)" htmlFor="currentCTC">
          <TextInput
            id="currentCTC"
            type="number"
            min={0}
            step="0.01"
            required
            value={value.currentCTC}
            onChange={(e) => set('currentCTC', e.target.value)}
          />
        </FormField>
        <FormField label="Expected CTC (annual)" htmlFor="expectedCTC">
          <TextInput
            id="expectedCTC"
            type="number"
            min={0}
            step="0.01"
            required
            value={value.expectedCTC}
            onChange={(e) => set('expectedCTC', e.target.value)}
          />
        </FormField>
      </div>

      <FormField label="UAN number" htmlFor="uanNumber">
        <TextInput
          id="uanNumber"
          required
          inputMode="numeric"
          value={value.uanNumber}
          onChange={(e) => set('uanNumber', e.target.value)}
        />
      </FormField>

      {errorMessage && (
        <p role="alert" className="text-sm text-red-600">
          {errorMessage}
        </p>
      )}

      <div className="flex justify-between">
        <Button type="button" variant="ghost" onClick={onBack}>
          Back
        </Button>
        <Button type="submit" isLoading={isSubmitting}>
          Next
        </Button>
      </div>
    </form>
  )
}
