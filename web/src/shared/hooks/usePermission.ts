import { useAuthStore } from '../../app/store'
import { hasPermission } from '../lib/permissions'

export function usePermission(required: string): boolean {
  const permissions = useAuthStore((state) => state.permissions)
  return hasPermission(permissions, required)
}
