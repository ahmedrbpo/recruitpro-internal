import { useState } from 'react'
import { FormField } from '../../../../shared/components/FormField'
import { TextInput } from '../../../../shared/components/TextInput'
import { Select } from '../../../../shared/components/Select'
import { Button } from '../../../../shared/components/Button'
import { emptyEmploymentForm, type CandidateEmploymentHistory, type EmploymentForm } from '../../types'

interface EmploymentStepProps {
  employments: CandidateEmploymentHistory[]
  onAdd: (form: EmploymentForm) => Promise<void>
  onRemove: (employmentId: string) => void
  onNext: () => void
  onBack: () => void
  isAdding: boolean
  errorMessage: string | null
}

export function EmploymentStep({ employments, onAdd, onRemove, onNext, onBack, isAdding, errorMessage }: EmploymentStepProps) {
  const [form, setForm] = useState<EmploymentForm>(emptyEmploymentForm)

  function set<K extends keyof EmploymentForm>(key: K, value: EmploymentForm[K]) {
    setForm({ ...form, [key]: value })
  }

  async function handleAdd() {
    await onAdd(form)
    setForm(emptyEmploymentForm)
  }

  return (
    <div className="flex flex-col gap-6">
      {employments.length > 0 && (
        <ul className="flex flex-col gap-2">
          {employments.map((employment) => (
            <li
              key={employment.id}
              className="flex items-center justify-between rounded-md border border-slate-200 px-3 py-2 text-sm"
            >
              <span>
                <span className="font-medium text-slate-900">{employment.designation}</span>
                {' · '}
                {employment.company}
              </span>
              <Button type="button" variant="ghost" size="xs" onClick={() => onRemove(employment.id)}>
                Remove
              </Button>
            </li>
          ))}
        </ul>
      )}

      <div className="rounded-lg border border-dashed border-slate-300 p-4">
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <FormField label="Payroll company" htmlFor="payrollCompany">
            <TextInput id="payrollCompany" value={form.payrollCompany} onChange={(e) => set('payrollCompany', e.target.value)} />
          </FormField>
          <FormField label="Company name" htmlFor="company">
            <TextInput id="company" value={form.company} onChange={(e) => set('company', e.target.value)} />
          </FormField>
        </div>
        <div className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-2">
          <FormField label="Designation" htmlFor="designation">
            <TextInput id="designation" value={form.designation} onChange={(e) => set('designation', e.target.value)} />
          </FormField>
          <FormField label="Employment type" htmlFor="employmentHistoryType">
            <Select
              id="employmentHistoryType"
              value={form.type}
              onChange={(e) => set('type', e.target.value as EmploymentForm['type'])}
            >
              <option value="" disabled>
                Select
              </option>
              <option value="FullTime">Full-Time</option>
              <option value="PartTime">Part-Time</option>
              <option value="Contract">Contract</option>
              <option value="Freelance">Freelance</option>
            </Select>
          </FormField>
        </div>
        <div className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-3">
          <FormField label="Start date" htmlFor="empStartDate">
            <TextInput id="empStartDate" type="month" value={form.startDate} onChange={(e) => set('startDate', e.target.value)} />
          </FormField>
          <FormField label="End date" htmlFor="empEndDate">
            <TextInput id="empEndDate" type="month" value={form.endDate} onChange={(e) => set('endDate', e.target.value)} />
          </FormField>
          <FormField label="Work location" htmlFor="empLocation">
            <TextInput id="empLocation" value={form.location} onChange={(e) => set('location', e.target.value)} />
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
            disabled={!form.company || !form.designation || !form.type || !form.startDate}
            onClick={handleAdd}
          >
            Add employment
          </Button>
        </div>
      </div>

      <div className="flex justify-between">
        <Button type="button" variant="ghost" onClick={onBack}>
          Back
        </Button>
        <Button type="button" disabled={employments.length === 0} onClick={onNext}>
          Next
        </Button>
      </div>
    </div>
  )
}
