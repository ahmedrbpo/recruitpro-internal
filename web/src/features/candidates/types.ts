export type Gender = 'Male' | 'Female' | 'Other'
export type EducationType = 'FullTime' | 'PartTime' | 'Distance'
export type EmploymentHistoryType = 'FullTime' | 'PartTime' | 'Contract' | 'Freelance'
export type ApplicationWorkType = 'Remote' | 'WorkFromOffice' | 'Hybrid'
export type ApplicationInterviewType = 'Virtual' | 'FaceToFace' | 'Telephonic'
export type OnboardingType = 'ContractToHire' | 'EmployerPayroll'

export interface Resume {
  id: string
  originalFileName: string
  contentType: string
  sizeBytes: number
  isConfirmed: boolean
}

export interface CandidateEducation {
  id: string
  college: string
  degree: string
  stream: string | null
  type: EducationType
  startDate: string
  endDate: string | null
  location: string | null
}

export interface CandidateEmploymentHistory {
  id: string
  payrollCompany: string
  company: string
  designation: string
  type: EmploymentHistoryType
  startDate: string
  endDate: string | null
  location: string | null
}

export interface Candidate {
  id: string
  firstName: string
  lastName: string
  email: string
  phone: string | null
  gender: Gender | null
  dateOfBirth: string | null
  pan: string | null
  totalExperienceYears: number | null
  relevantExperienceYears: number | null
  streetAddress: string | null
  city: string | null
  state: string | null
  postalCode: string | null
  workLocation: string | null
  resumes: Resume[]
  educations: CandidateEducation[]
  employmentHistory: CandidateEmploymentHistory[]
}

export interface CreateCandidateRequest {
  firstName: string
  lastName: string
  email: string
  phone?: string | null
}

export interface UpdateCandidateRequest {
  gender: Gender | null
  dateOfBirth: string | null
  pan: string | null
  totalExperienceYears: number | null
  relevantExperienceYears: number | null
  streetAddress: string | null
  city: string | null
  state: string | null
  postalCode: string | null
  workLocation: string | null
}

export interface AddEducationRequest {
  college: string
  degree: string
  stream?: string | null
  type: EducationType
  startDate: string
  endDate?: string | null
  location?: string | null
}

export interface AddEmploymentRequest {
  payrollCompany: string
  company: string
  designation: string
  type: EmploymentHistoryType
  startDate: string
  endDate?: string | null
  location?: string | null
}

export interface ApplicationSubmissionDetails {
  role: string
  clientName: string | null
  requirementId: string
  onboarding: OnboardingType | null
  workType: ApplicationWorkType | null
  interviewType: ApplicationInterviewType | null
  currentCTC: number | null
  expectedCTC: number | null
  uanNumber: string | null
}

export interface SetSubmissionDetailsRequest {
  workType: ApplicationWorkType
  interviewType: ApplicationInterviewType
  currentCTC: number
  expectedCTC: number
  uanNumber?: string | null
}

export interface Application {
  id: string
  jobId: string
  candidateId: string
  stage: string
}

// Form-state shapes for the submission wizard — string-typed fields (dates, numbers) so every
// input stays a controlled <input>; converted to the typed request shapes above on submit.
export interface PersonalInfoForm {
  firstName: string
  lastName: string
  gender: Gender | ''
  dateOfBirth: string
  pan: string
  email: string
  phone: string
  totalExperienceYears: string
  relevantExperienceYears: string
  streetAddress: string
  city: string
  state: string
  postalCode: string
  workLocation: string
}

export const emptyPersonalInfoForm: PersonalInfoForm = {
  firstName: '',
  lastName: '',
  gender: '',
  dateOfBirth: '',
  pan: '',
  email: '',
  phone: '',
  totalExperienceYears: '',
  relevantExperienceYears: '',
  streetAddress: '',
  city: '',
  state: '',
  postalCode: '',
  workLocation: '',
}

export interface EducationForm {
  college: string
  degree: string
  stream: string
  type: EducationType | ''
  startDate: string
  endDate: string
  location: string
}

export const emptyEducationForm: EducationForm = {
  college: '',
  degree: '',
  stream: '',
  type: '',
  startDate: '',
  endDate: '',
  location: '',
}

export interface EmploymentForm {
  payrollCompany: string
  company: string
  designation: string
  type: EmploymentHistoryType | ''
  startDate: string
  endDate: string
  location: string
}

export const emptyEmploymentForm: EmploymentForm = {
  payrollCompany: '',
  company: '',
  designation: '',
  type: '',
  startDate: '',
  endDate: '',
  location: '',
}

export interface SubmissionDetailsForm {
  workType: ApplicationWorkType | ''
  interviewType: ApplicationInterviewType | ''
  currentCTC: string
  expectedCTC: string
  uanNumber: string
}

export const emptySubmissionDetailsForm: SubmissionDetailsForm = {
  workType: '',
  interviewType: '',
  currentCTC: '',
  expectedCTC: '',
  uanNumber: '',
}
