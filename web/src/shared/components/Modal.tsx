import type { ReactNode } from 'react'

interface ModalProps {
  title: string
  isOpen: boolean
  onClose: () => void
  size?: 'sm' | 'md' | 'lg'
  children: ReactNode
}

const sizeClasses: Record<NonNullable<ModalProps['size']>, string> = {
  sm: 'max-w-sm',
  md: 'max-w-lg',
  lg: 'max-w-2xl',
}

export function Modal({ title, isOpen, onClose, size = 'md', children }: ModalProps) {
  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/50 px-4 py-8">
      <div className={`flex max-h-full w-full flex-col rounded-xl bg-white shadow-xl ${sizeClasses[size]}`}>
        <div className="flex items-center justify-between border-b border-slate-100 p-6 pb-4">
          <h2 className="text-lg font-semibold text-slate-900">{title}</h2>
          <button
            type="button"
            aria-label="Close"
            onClick={onClose}
            className="rounded p-1 text-slate-400 hover:bg-brand-50 hover:text-brand-600"
          >
            &times;
          </button>
        </div>
        <div className="overflow-y-auto p-6 pt-4">{children}</div>
      </div>
    </div>
  )
}
