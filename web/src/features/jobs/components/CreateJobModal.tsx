import { useState, type FormEvent } from 'react'
import { Modal } from '../../../shared/components/Modal'
import { FormField } from '../../../shared/components/FormField'
import { Button } from '../../../shared/components/Button'
import { useCreateJob } from '../hooks/useCreateJob'
import { getApiErrorMessage } from '../../../shared/lib/errors'
import type { EmploymentType, WorkMode } from '../types'

const employmentTypes: EmploymentType[] = [
  'FullTime',
  'Contract',
  'ContractToHire',
  'Internship',
  'PartTime',
  'Freelance',
  'Temporary',
]

const workModes: WorkMode[] = ['Onsite', 'Hybrid', 'Remote']

interface CreateJobModalProps {
  isOpen: boolean
  onClose: () => void
}

export function CreateJobModal({ isOpen, onClose }: CreateJobModalProps) {
  const [jobCode, setJobCode] = useState('')
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [employmentType, setEmploymentType] = useState<EmploymentType>('FullTime')
  const [workMode, setWorkMode] = useState<WorkMode>('Onsite')
  const [experienceMin, setExperienceMin] = useState('0')
  const [experienceMax, setExperienceMax] = useState('1')
  const [currencyCode, setCurrencyCode] = useState('INR')
  const [salaryMin, setSalaryMin] = useState('')
  const [salaryMax, setSalaryMax] = useState('')
  const [skills, setSkills] = useState('')
  const [formError, setFormError] = useState<string | null>(null)

  const createJob = useCreateJob()

  function resetAndClose() {
    setJobCode('')
    setTitle('')
    setDescription('')
    setEmploymentType('FullTime')
    setWorkMode('Onsite')
    setExperienceMin('0')
    setExperienceMax('1')
    setCurrencyCode('INR')
    setSalaryMin('')
    setSalaryMax('')
    setSkills('')
    setFormError(null)
    onClose()
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setFormError(null)
    try {
      await createJob.mutateAsync({
        jobCode,
        title,
        description,
        employmentType,
        workMode,
        experienceMin: Number(experienceMin),
        experienceMax: Number(experienceMax),
        currencyCode,
        salaryMin: salaryMin ? Number(salaryMin) : null,
        salaryMax: salaryMax ? Number(salaryMax) : null,
        skills: skills
          .split(',')
          .map((skill) => skill.trim())
          .filter(Boolean),
      })
      resetAndClose()
    } catch (error) {
      setFormError(getApiErrorMessage(error, 'Failed to create job.'))
    }
  }

  return (
    <Modal title="Create job" isOpen={isOpen} onClose={resetAndClose} size="lg">
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <div className="grid grid-cols-2 gap-4">
          <FormField label="Job code" htmlFor="jobCode">
            <input
              id="jobCode"
              required
              value={jobCode}
              onChange={(event) => setJobCode(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>
          <FormField label="Currency" htmlFor="currencyCode">
            <input
              id="currencyCode"
              required
              value={currencyCode}
              onChange={(event) => setCurrencyCode(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>
        </div>

        <FormField label="Title" htmlFor="title">
          <input
            id="title"
            required
            value={title}
            onChange={(event) => setTitle(event.target.value)}
            className="rounded-md border border-slate-300 px-3 py-2 text-sm"
          />
        </FormField>

        <FormField label="Description" htmlFor="description">
          <textarea
            id="description"
            required
            rows={3}
            value={description}
            onChange={(event) => setDescription(event.target.value)}
            className="rounded-md border border-slate-300 px-3 py-2 text-sm"
          />
        </FormField>

        <div className="grid grid-cols-2 gap-4">
          <FormField label="Employment type" htmlFor="employmentType">
            <select
              id="employmentType"
              value={employmentType}
              onChange={(event) => setEmploymentType(event.target.value as EmploymentType)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            >
              {employmentTypes.map((type) => (
                <option key={type} value={type}>
                  {type}
                </option>
              ))}
            </select>
          </FormField>
          <FormField label="Work mode" htmlFor="workMode">
            <select
              id="workMode"
              value={workMode}
              onChange={(event) => setWorkMode(event.target.value as WorkMode)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            >
              {workModes.map((mode) => (
                <option key={mode} value={mode}>
                  {mode}
                </option>
              ))}
            </select>
          </FormField>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <FormField label="Experience min (yrs)" htmlFor="experienceMin">
            <input
              id="experienceMin"
              type="number"
              min={0}
              required
              value={experienceMin}
              onChange={(event) => setExperienceMin(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>
          <FormField label="Experience max (yrs)" htmlFor="experienceMax">
            <input
              id="experienceMax"
              type="number"
              min={0}
              required
              value={experienceMax}
              onChange={(event) => setExperienceMax(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <FormField label="Salary min (optional)" htmlFor="salaryMin">
            <input
              id="salaryMin"
              type="number"
              min={0}
              value={salaryMin}
              onChange={(event) => setSalaryMin(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>
          <FormField label="Salary max (optional)" htmlFor="salaryMax">
            <input
              id="salaryMax"
              type="number"
              min={0}
              value={salaryMax}
              onChange={(event) => setSalaryMax(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>
        </div>

        <FormField label="Skills (comma-separated, optional)" htmlFor="skills">
          <input
            id="skills"
            value={skills}
            onChange={(event) => setSkills(event.target.value)}
            className="rounded-md border border-slate-300 px-3 py-2 text-sm"
          />
        </FormField>

        {formError && (
          <p role="alert" className="text-sm text-red-600">
            {formError}
          </p>
        )}

        <div className="flex justify-end gap-2">
          <Button type="button" variant="secondary" onClick={resetAndClose}>
            Cancel
          </Button>
          <Button type="submit" isLoading={createJob.isPending}>
            {createJob.isPending ? 'Creating…' : 'Create job'}
          </Button>
        </div>
      </form>
    </Modal>
  )
}
