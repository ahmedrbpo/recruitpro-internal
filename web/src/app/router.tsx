import { createBrowserRouter, Navigate } from 'react-router-dom'
import { LoginPage } from '../features/auth/pages/LoginPage'
import { JobsListPage } from '../features/jobs/pages/JobsListPage'
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
])
