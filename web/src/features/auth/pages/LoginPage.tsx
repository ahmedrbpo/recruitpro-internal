import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { Button } from '../../../shared/components/Button'
import { FormField } from '../../../shared/components/FormField'
import { getApiErrorMessage } from '../../../shared/lib/errors'

export function LoginPage() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [formError, setFormError] = useState<string | null>(null)
  const { login, isLoggingIn } = useAuth()
  const navigate = useNavigate()

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setFormError(null)
    try {
      await login({ email, password })
      navigate('/jobs', { replace: true })
    } catch (error) {
      setFormError(getApiErrorMessage(error, 'Login failed.'))
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-brand-50 via-white to-brand-100 px-4">
      <form onSubmit={handleSubmit} className="w-full max-w-sm rounded-xl bg-white p-8 shadow-lg shadow-brand-900/5">
        <h1 className="mb-6 text-xl font-semibold text-brand-700">RecruitPro</h1>

        <div className="flex flex-col gap-4">
          <FormField label="Email" htmlFor="email">
            <input
              id="email"
              type="email"
              required
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>

          <FormField label="Password" htmlFor="password">
            <input
              id="password"
              type="password"
              required
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              className="rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-100"
            />
          </FormField>

          {formError && (
            <p role="alert" className="text-sm text-red-600">
              {formError}
            </p>
          )}

          <Button type="submit" isLoading={isLoggingIn} className="w-full">
            {isLoggingIn ? 'Signing in…' : 'Sign in'}
          </Button>
        </div>
      </form>
    </div>
  )
}
