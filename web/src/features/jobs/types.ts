export type JobStatus = 'Draft' | 'PendingApproval' | 'Published' | 'OnHold' | 'Closed' | 'Archived' | 'Cancelled'

export type EmploymentType =
  | 'FullTime'
  | 'Contract'
  | 'ContractToHire'
  | 'Internship'
  | 'PartTime'
  | 'Freelance'
  | 'Temporary'

export type WorkMode = 'Onsite' | 'Hybrid' | 'Remote'

export interface Job {
  id: string
  jobCode: string
  title: string
  description: string
  status: JobStatus
  employmentType: EmploymentType
  workMode: WorkMode
  experienceMin: number
  experienceMax: number
  salaryMin: number | null
  salaryMax: number | null
  currencyCode: string
  publishedDate: string | null
  notes: string | null
  clientId: string | null
  jobCategoryId: string | null
  departmentId: string | null
  recruiterId: string | null
  skills: string[]
}

export interface CreateJobRequest {
  jobCode: string
  title: string
  description: string
  employmentType: EmploymentType
  workMode: WorkMode
  experienceMin: number
  experienceMax: number
  currencyCode: string
  clientId?: string | null
  jobCategoryId?: string | null
  departmentId?: string | null
  recruiterId?: string | null
  salaryMin?: number | null
  salaryMax?: number | null
  notes?: string | null
  skills?: string[] | null
}
