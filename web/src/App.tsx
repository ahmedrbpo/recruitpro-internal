import { useEffect, useState } from 'react'
import { QueryClientProvider } from '@tanstack/react-query'
import { RouterProvider } from 'react-router-dom'
import { queryClient } from './app/queryClient'
import { router } from './app/router'
import { refreshAccessToken } from './shared/lib/apiClient'

function App() {
  // A page reload clears the in-memory access token (by design — never localStorage). Try one
  // silent refresh against the httpOnly cookie before deciding whether ProtectedRoute should
  // bounce the user to /login, so a valid session survives a reload.
  const [isBootstrapping, setIsBootstrapping] = useState(true)

  useEffect(() => {
    refreshAccessToken().finally(() => setIsBootstrapping(false))
  }, [])

  if (isBootstrapping) {
    return <div className="flex min-h-screen items-center justify-center text-sm text-slate-400">Loading…</div>
  }

  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  )
}

export default App
