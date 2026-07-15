import { useState } from 'react'
import { FormField } from '../../../../shared/components/FormField'
import { TextInput } from '../../../../shared/components/TextInput'
import { Select } from '../../../../shared/components/Select'
import { Button } from '../../../../shared/components/Button'
import { emptyEducationForm, type CandidateEducation, type EducationForm } from '../../types'

interface EducationStepProps {
  educations: CandidateEducation[]
  onAdd: (form: EducationForm) => Promise<void>
  onRemove: (educationId: string) => void
  onNext: () => void
  onBack: () => void
  isAdding: boolean
  errorMessage: string | null
}

export function EducationStep({ educations, onAdd, onRemove, onNext, onBack, isAdding, errorMessage }: EducationStepProps) {
  const [form, setForm] = useState<EducationForm>(emptyEducationForm)

  function set<K extends keyof EducationForm>(key: K, value: EducationForm[K]) {
    setForm({ ...form, [key]: value })
  }

  async function handleAdd() {
    await onAdd(form)
    setForm(emptyEducationForm)
  }

  return (
    <div className="flex flex-col gap-6">
      {educations.length > 0 && (
        <ul className="flex flex-col gap-2">
          {educations.map((education) => (
            <li
              key={education.id}
              className="flex items-center justify-between rounded-md border border-slate-200 px-3 py-2 text-sm"
            >
              <span>
                <span className="font-medium text-slate-900">{education.degree}</span>
                {' · '}
                {education.college}
                {education.stream && ` · ${education.stream}`}
              </span>
              <Button type="button" variant="ghost" size="xs" onClick={() => onRemove(education.id)}>
                Remove
              </Button>
            </li>
          ))}
        </ul>
      )}

      <div className="rounded-lg border border-dashed border-slate-300 p-4">
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <FormField label="College / University" htmlFor="college">
            <TextInput id="college" value={form.college} onChange={(e) => set('college', e.target.value)} />
          </FormField>
          <FormField label="Degree" htmlFor="degree">
            <TextInput id="degree" value={form.degree} onChange={(e) => set('degree', e.target.value)} />
          </FormField>
        </div>
        <div className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-2">
          <FormField label="Stream / Specialization" htmlFor="stream">
            <TextInput id="stream" value={form.stream} onChange={(e) => set('stream', e.target.value)} />
          </FormField>
          <FormField label="Education type" htmlFor="educationType">
            <Select id="educationType" value={form.type} onChange={(e) => set('type', e.target.value as EducationForm['type'])}>
              <option value="" disabled>
                Select
              </option>
              <option value="FullTime">Full-Time</option>
              <option value="PartTime">Part-Time</option>
              <option value="Distance">Distance</option>
            </Select>
          </FormField>
        </div>
        <div className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-3">
          <FormField label="Start date" htmlFor="eduStartDate">
            <TextInput id="eduStartDate" type="month" value={form.startDate} onChange={(e) => set('startDate', e.target.value)} />
          </FormField>
          <FormField label="End date" htmlFor="eduEndDate">
            <TextInput id="eduEndDate" type="month" value={form.endDate} onChange={(e) => set('endDate', e.target.value)} />
          </FormField>
          <FormField label="Location" htmlFor="eduLocation">
            <TextInput id="eduLocation" value={form.location} onChange={(e) => set('location', e.target.value)} />
          </FormField>
        </div>

        {errorMessage && (
          <p role="alert" className="mt-3 text-sm text-red-600">
            {errorMessage}
          </p>
        )}

        <div className="mt-4 flex justify-end">
          <Button
            type="button"
            variant="secondary"
            size="sm"
            isLoading={isAdding}
            disabled={!form.college || !form.degree || !form.type || !form.startDate}
            onClick={handleAdd}
          >
            Add education
          </Button>
        </div>
      </div>

      <div className="flex justify-between">
        <Button type="button" variant="ghost" onClick={onBack}>
          Back
        </Button>
        <Button type="button" disabled={educations.length === 0} onClick={onNext}>
          Next
        </Button>
      </div>
    </div>
  )
}
