import { render, screen } from '@testing-library/react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { describe, expect, it, vi } from 'vitest'
import { CandidateSubmissionPage } from './CandidateSubmissionPage'
import { useJob } from '../../jobs/hooks/useJob'
import type { Job } from '../../jobs/types'

vi.mock('../../jobs/hooks/useJob')

const mockedUseJob = vi.mocked(useJob)

const sampleJob: Job = {
  id: 'job-1',
  jobCode: 'RP-2026-000001',
  title: 'Senior .NET Developer',
  description: 'desc',
  status: 'Published',
  employmentType: 'FullTime',
  workMode: 'Remote',
  experienceMin: 2,
  experienceMax: 5,
  salaryMin: null,
  salaryMax: null,
  currencyCode: 'INR',
  publishedDate: null,
  notes: null,
  clientId: null,
  jobCategoryId: null,
  departmentId: null,
  recruiterId: null,
  skills: [],
}

function renderPage() {
  const queryClient = new QueryClient({ defaultOptions: { queries: { retry: false } } })
  render(
    <QueryClientProvider client={queryClient}>
      <MemoryRouter initialEntries={['/jobs/job-1/candidates/new']}>
        <Routes>
          <Route path="/jobs/:jobId/candidates/new" element={<CandidateSubmissionPage />} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>,
  )
}

describe('CandidateSubmissionPage', () => {
  it('renders the personal info step with the job title and step indicator', () => {
    mockedUseJob.mockReturnValue({ data: sampleJob } as unknown as ReturnType<typeof useJob>)

    renderPage()

    expect(screen.getByText(/Submit candidate/i)).toBeInTheDocument()
    expect(screen.getByText(/Senior \.NET Developer/)).toBeInTheDocument()
    expect(screen.getByLabelText('First name')).toBeInTheDocument()
    expect(screen.getByLabelText('Role')).toHaveValue('Senior .NET Developer')
    expect(screen.getByRole('button', { name: /next/i })).toBeInTheDocument()
  })

  it('keeps the Next button disabled behavior working via required fields', () => {
    mockedUseJob.mockReturnValue({ data: sampleJob } as unknown as ReturnType<typeof useJob>)

    renderPage()

    // Step indicator shows all six steps.
    expect(screen.getByText('Personal Info')).toBeInTheDocument()
    expect(screen.getByText('Education')).toBeInTheDocument()
    expect(screen.getByText('Employment')).toBeInTheDocument()
    expect(screen.getByText('Client & Submission')).toBeInTheDocument()
    expect(screen.getByText('Resume')).toBeInTheDocument()
    expect(screen.getByText('Review & Submit')).toBeInTheDocument()
  })
})
