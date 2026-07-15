const steps = ['Personal Info', 'Education', 'Employment', 'Client & Submission', 'Resume', 'Review & Submit']

interface StepIndicatorProps {
  currentStep: number
}

export function StepIndicator({ currentStep }: StepIndicatorProps) {
  return (
    <ol className="mb-8 flex flex-wrap gap-x-6 gap-y-3">
      {steps.map((label, index) => {
        const stepNumber = index + 1
        const isComplete = stepNumber < currentStep
        const isCurrent = stepNumber === currentStep

        return (
          <li key={label} className="flex items-center gap-2 text-sm">
            <span
              className={`flex h-6 w-6 items-center justify-center rounded-full text-xs font-semibold ${
                isCurrent
                  ? 'bg-brand-600 text-white'
                  : isComplete
                    ? 'bg-brand-100 text-brand-700'
                    : 'bg-slate-100 text-slate-500'
              }`}
            >
              {stepNumber}
            </span>
            <span className={isCurrent ? 'font-medium text-brand-700' : 'text-slate-500'}>{label}</span>
          </li>
        )
      })}
    </ol>
  )
}
