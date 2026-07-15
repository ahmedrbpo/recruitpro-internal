import { createBrowserRouter, Navigate } from 'react-router-dom'
import { LoginPage } from '../features/auth/pages/LoginPage'
import { JobsListPage } from '../features/jobs/pages/JobsListPage'
import { CandidateSubmissionPage } from '../features/candidates/pages/CandidateSubmissionPage'
import { ProtectedRoute } from './ProtectedRoute'

export const router = createBrowserRouter([
  { path: '/', element: <Navigate to="/jobs" replace /> },
  { path: '/login', element: <LoginPage /> },
  {
    path: '/jobs',
    element: (
      <ProtectedRoute>
        <JobsListPage />
      </ProtectedRoute>
    ),
  },
  {
    path: '/jobs/:jobId/candidates/new',
    element: (
      <ProtectedRoute>
        <CandidateSubmissionPage />
      </ProtectedRoute>
    ),
  },
])
