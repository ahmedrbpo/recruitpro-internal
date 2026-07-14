import { useMutation } from '@tanstack/react-query'
import { useAuthStore } from '../../../app/store'
import { decodeAccessToken } from '../../../shared/lib/jwt'
import { login as loginRequest, logout as logoutRequest } from '../api/authApi'

export function useAuth() {
  const { accessToken, email, permissions, setAuth, clearAuth } = useAuthStore()

  const loginMutation = useMutation({
    mutationFn: ({ email, password }: { email: string; password: string }) => loginRequest(email, password),
    onSuccess: (tokens) => {
      const claims = decodeAccessToken(tokens.accessToken)
      setAuth({ ...tokens, ...claims })
    },
  })

  const logoutMutation = useMutation({
    mutationFn: logoutRequest,
    onSettled: () => clearAuth(),
  })

  return {
    isAuthenticated: !!accessToken,
    email,
    permissions,
    login: loginMutation.mutateAsync,
    isLoggingIn: loginMutation.isPending,
    loginError: loginMutation.error,
    logout: logoutMutation.mutateAsync,
  }
}
