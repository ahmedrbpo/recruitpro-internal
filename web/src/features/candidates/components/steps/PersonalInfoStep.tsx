import type { FormEvent } from 'react'
import { FormField } from '../../../../shared/components/FormField'
import { TextInput } from '../../../../shared/components/TextInput'
import { Select } from '../../../../shared/components/Select'
import { Button } from '../../../../shared/components/Button'
import type { PersonalInfoForm } from '../../types'

interface PersonalInfoStepProps {
  role: string
  value: PersonalInfoForm
  onChange: (value: PersonalInfoForm) => void
  onNext: () => void
  isSubmitting: boolean
  errorMessage: string | null
}

export function PersonalInfoStep({ role, value, onChange, onNext, isSubmitting, errorMessage }: PersonalInfoStepProps) {
  function set<K extends keyof PersonalInfoForm>(key: K, fieldValue: PersonalInfoForm[K]) {
    onChange({ ...value, [key]: fieldValue })
  }

  function handleSubmit(event: FormEvent) {
    event.preventDefault()
    onNext()
  }

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4">
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="First name" htmlFor="firstName">
          <TextInput id="firstName" required value={value.firstName} onChange={(e) => set('firstName', e.target.value)} />
        </FormField>
        <FormField label="Last name" htmlFor="lastName">
          <TextInput id="lastName" required value={value.lastName} onChange={(e) => set('lastName', e.target.value)} />
        </FormField>
      </div>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="Gender" htmlFor="gender">
          <Select id="gender" required value={value.gender} onChange={(e) => set('gender', e.target.value as PersonalInfoForm['gender'])}>
            <option value="" disabled>
              Select
            </option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </Select>
        </FormField>
        <FormField label="Date of birth" htmlFor="dateOfBirth">
          <TextInput
            id="dateOfBirth"
            type="date"
            required
            value={value.dateOfBirth}
            onChange={(e) => set('dateOfBirth', e.target.value)}
          />
        </FormField>
      </div>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="PAN number" htmlFor="pan">
          <TextInput
            id="pan"
            required
            placeholder="AAAAA9999A"
            pattern="[A-Z]{5}[0-9]{4}[A-Z]"
            title="Format: AAAAA9999A"
            value={value.pan}
            onChange={(e) => set('pan', e.target.value.toUpperCase())}
          />
        </FormField>
        <FormField label="Role" htmlFor="role">
          <TextInput id="role" readOnly disabled value={role} className="bg-slate-50 text-slate-500" />
        </FormField>
      </div>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="Email address" htmlFor="email">
          <TextInput id="email" type="email" required value={value.email} onChange={(e) => set('email', e.target.value)} />
        </FormField>
        <FormField label="Phone number" htmlFor="phone">
          <TextInput id="phone" type="tel" required value={value.phone} onChange={(e) => set('phone', e.target.value)} />
        </FormField>
      </div>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <FormField label="Total experience (yrs)" htmlFor="totalExperienceYears">
          <TextInput
            id="totalExperienceYears"
            type="number"
            min={0}
            step="0.1"
            required
            value={value.totalExperienceYears}
            onChange={(e) => set('totalExperienceYears', e.target.value)}
          />
        </FormField>
        <FormField label="Relevant experience (yrs)" htmlFor="relevantExperienceYears">
          <TextInput
            id="relevantExperienceYears"
            type="number"
            min={0}
            step="0.1"
            required
            value={value.relevantExperienceYears}
            onChange={(e) => set('relevantExperienceYears', e.target.value)}
          />
        </FormField>
      </div>

      <FormField label="Street address" htmlFor="streetAddress">
        <TextInput
          id="streetAddress"
          required
          value={value.streetAddress}
          onChange={(e) => set('streetAddress', e.target.value)}
        />
      </FormField>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
        <FormField label="City" htmlFor="city">
          <TextInput id="city" required value={value.city} onChange={(e) => set('city', e.target.value)} />
        </FormField>
        <FormField label="State" htmlFor="state">
          <TextInput id="state" required value={value.state} onChange={(e) => set('state', e.target.value)} />
        </FormField>
        <FormField label="Postal code" htmlFor="postalCode">
          <TextInput id="postalCode" required value={value.postalCode} onChange={(e) => set('postalCode', e.target.value)} />
        </FormField>
      </div>

      <FormField label="Preferred work location" htmlFor="workLocation">
        <TextInput
          id="workLocation"
          required
          value={value.workLocation}
          onChange={(e) => set('workLocation', e.target.value)}
        />
      </FormField>

      {errorMessage && (
        <p role="alert" className="text-sm text-red-600">
          {errorMessage}
        </p>
      )}

      <div className="flex justify-end">
        <Button type="submit" isLoading={isSubmitting}>
          Next
        </Button>
      </div>
    </form>
  )
}
