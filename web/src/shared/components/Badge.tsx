import type { ReactNode } from 'react'

type Tone = 'blue' | 'green' | 'red' | 'yellow' | 'gray' | 'brand'

interface BadgeProps {
  tone?: Tone
  children: ReactNode
}

// Status color coding per design.md §4: blue = submitted/in-flight, green = active/processed,
// red = rejected, yellow = on hold/paused, gray = draft/closed/no response.
const toneClasses: Record<Tone, string> = {
  blue: 'bg-sky-100 text-sky-700',
  green: 'bg-emerald-100 text-emerald-700',
  red: 'bg-red-100 text-red-700',
  yellow: 'bg-amber-100 text-amber-700',
  gray: 'bg-slate-100 text-slate-600',
  brand: 'bg-brand-100 text-brand-700',
}

export function Badge({ tone = 'gray', children }: BadgeProps) {
  return (
    <span className={`inline-flex items-center rounded-full px-2 py-0.5 text-xs font-medium ${toneClasses[tone]}`}>
      {children}
    </span>
  )
}
