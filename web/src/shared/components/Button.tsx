import type { ButtonHTMLAttributes } from 'react'
import { Spinner } from './Spinner'

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'danger' | 'ghost'
  size?: 'xs' | 'sm' | 'md' | 'lg'
  isLoading?: boolean
}

const variantClasses: Record<NonNullable<ButtonProps['variant']>, string> = {
  primary: 'bg-brand-600 text-white hover:bg-brand-700 focus-visible:outline-brand-600',
  secondary:
    'bg-white text-slate-900 border border-slate-300 hover:bg-slate-50 focus-visible:outline-brand-600',
  danger: 'bg-red-600 text-white hover:bg-red-500 focus-visible:outline-red-600',
  ghost: 'text-slate-600 hover:bg-slate-100 focus-visible:outline-brand-600',
}

const sizeClasses: Record<NonNullable<ButtonProps['size']>, string> = {
  xs: 'px-2 py-1 text-xs gap-1',
  sm: 'px-2.5 py-1.5 text-sm gap-1.5',
  md: 'px-3 py-2 text-sm gap-2',
  lg: 'px-4 py-2.5 text-base gap-2',
}

export function Button({
  variant = 'primary',
  size = 'md',
  isLoading = false,
  className = '',
  disabled,
  children,
  ...rest
}: ButtonProps) {
  return (
    <button
      className={`inline-flex items-center justify-center rounded-md font-medium transition-colors focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 disabled:cursor-not-allowed disabled:opacity-50 ${variantClasses[variant]} ${sizeClasses[size]} ${className}`}
      disabled={disabled || isLoading}
      {...rest}
    >
      {isLoading && <Spinner className="h-4 w-4" />}
      {children}
    </button>
  )
}
