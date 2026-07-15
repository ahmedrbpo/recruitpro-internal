import { render, screen } from '@testing-library/react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { MemoryRouter } from 'react-router-dom'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { JobsListPage } from './JobsListPage'
import { useJobs } from '../hooks/useJobs'
import { usePublishJob } from '../hooks/usePublishJob'
import { usePermission } from '../../../shared/hooks/usePermission'
import { useAuth } from '../../auth/hooks/useAuth'
import type { Job } from '../types'

vi.mock('../hooks/useJobs')
vi.mock('../hooks/usePublishJob')
vi.mock('../../../shared/hooks/usePermission')
vi.mock('../../auth/hooks/useAuth')

const mockedUseJobs = vi.mocked(useJobs)
const mockedUsePublishJob = vi.mocked(usePublishJob)
const mockedUsePermission = vi.mocked(usePermission)
const mockedUseAuth = vi.mocked(useAuth)

const sampleJob: Job = {
  id: 'job-1',
  jobCode: 'RP-2026-000001',
  title: 'Senior .NET Developer',
  description: 'desc',
  status: 'Draft',
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
  skills: ['C#', 'EF Core'],
}

function renderJobsListPage() {
  const queryClient = new QueryClient({ defaultOptions: { queries: { retry: false } } })
  render(
    <QueryClientProvider client={queryClient}>
      <MemoryRouter>
        <JobsListPage />
      </MemoryRouter>
    </QueryClientProvider>,
  )
}

describe('JobsListPage', () => {
  beforeEach(() => {
    mockedUseAuth.mockReturnValue({
      isAuthenticated: true,
      email: 'recruiter@coventine.com',
      permissions: [],
      login: vi.fn(),
      isLoggingIn: false,
      loginError: null,
      logout: vi.fn(),
    })
    mockedUsePublishJob.mockReturnValue({ mutate: vi.fn(), isPending: false } as unknown as ReturnType<typeof usePublishJob>)
  })

  it('renders jobs returned by the query', () => {
    mockedUseJobs.mockReturnValue({
      data: { items: [sampleJob], page: 1, pageSize: 20, totalCount: 1 },
      isLoading: false,
      isError: false,
    } as unknown as ReturnType<typeof useJobs>)
    mockedUsePermission.mockReturnValue(false)

    renderJobsListPage()

    expect(screen.getByText('RP-2026-000001')).toBeInTheDocument()
    expect(screen.getByText('Senior .NET Developer')).toBeInTheDocument()
    expect(screen.getByText('C#, EF Core')).toBeInTheDocument()
  })

  it('hides the Publish button without the publish permission', () => {
    mockedUseJobs.mockReturnValue({
      data: { items: [sampleJob], page: 1, pageSize: 20, totalCount: 1 },
      isLoading: false,
      isError: false,
    } as unknown as ReturnType<typeof useJobs>)
    mockedUsePermission.mockReturnValue(false)

    renderJobsListPage()

    expect(screen.queryByRole('button', { name: /publish/i })).not.toBeInTheDocument()
  })

  it('shows the Publish button with the publish permission on a draft job', () => {
    mockedUseJobs.mockReturnValue({
      data: { items: [sampleJob], page: 1, pageSize: 20, totalCount: 1 },
      isLoading: false,
      isError: false,
    } as unknown as ReturnType<typeof useJobs>)
    mockedUsePermission.mockReturnValue(true)

    renderJobsListPage()

    expect(screen.getByRole('button', { name: /publish/i })).toBeInTheDocument()
  })

  it('shows an empty message when there are no jobs', () => {
    mockedUseJobs.mockReturnValue({
      data: { items: [], page: 1, pageSize: 20, totalCount: 0 },
      isLoading: false,
      isError: false,
    } as unknown as ReturnType<typeof useJobs>)
    mockedUsePermission.mockReturnValue(false)

    renderJobsListPage()

    expect(screen.getByText('No jobs yet.')).toBeInTheDocument()
  })
})
