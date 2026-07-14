import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { MemoryRouter } from 'react-router-dom'
import { LoginPage } from './LoginPage'
import { useAuth } from '../hooks/useAuth'

vi.mock('../hooks/useAuth')

const mockedUseAuth = vi.mocked(useAuth)

function renderLoginPage() {
  render(
    <MemoryRouter>
      <LoginPage />
    </MemoryRouter>,
  )
}

describe('LoginPage', () => {
  beforeEach(() => {
    mockedUseAuth.mockReset()
  })

  it('submits the entered email and password', async () => {
    const login = vi.fn().mockResolvedValue(undefined)
    mockedUseAuth.mockReturnValue({
      isAuthenticated: false,
      email: null,
      permissions: [],
      login,
      isLoggingIn: false,
      loginError: null,
      logout: vi.fn(),
    })

    renderLoginPage()

    await userEvent.type(screen.getByLabelText('Email'), 'user@example.com')
    await userEvent.type(screen.getByLabelText('Password'), 'Password123!')
    await userEvent.click(screen.getByRole('button', { name: /sign in/i }))

    await waitFor(() =>
      expect(login).toHaveBeenCalledWith({ email: 'user@example.com', password: 'Password123!' }),
    )
  })

  it('shows the API error message when login is rejected', async () => {
    const login = vi.fn().mockRejectedValue({
      isAxiosError: true,
      response: { data: { error: { message: 'Invalid email or password.' } } },
    })
    mockedUseAuth.mockReturnValue({
      isAuthenticated: false,
      email: null,
      permissions: [],
      login,
      isLoggingIn: false,
      loginError: null,
      logout: vi.fn(),
    })

    renderLoginPage()

    await userEvent.type(screen.getByLabelText('Email'), 'user@example.com')
    await userEvent.type(screen.getByLabelText('Password'), 'wrong')
    await userEvent.click(screen.getByRole('button', { name: /sign in/i }))

    expect(await screen.findByRole('alert')).toHaveTextContent('Invalid email or password.')
  })

  it('disables the submit button while logging in', () => {
    mockedUseAuth.mockReturnValue({
      isAuthenticated: false,
      email: null,
      permissions: [],
      login: vi.fn(),
      isLoggingIn: true,
      loginError: null,
      logout: vi.fn(),
    })

    renderLoginPage()

    expect(screen.getByRole('button', { name: /signing in/i })).toBeDisabled()
  })
})
