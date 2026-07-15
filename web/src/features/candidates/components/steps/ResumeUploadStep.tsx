import { useRef, useState, type DragEvent } from 'react'
import { Button } from '../../../../shared/components/Button'
import type { Resume } from '../../types'

const ALLOWED_TYPES = [
  'application/pdf',
  'application/msword',
  'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
]
const MAX_SIZE_BYTES = 10 * 1024 * 1024

interface ResumeUploadStepProps {
  resume: Resume | null
  onUpload: (file: File) => Promise<void>
  onNext: () => void
  onBack: () => void
  isUploading: boolean
  errorMessage: string | null
}

export function ResumeUploadStep({ resume, onUpload, onNext, onBack, isUploading, errorMessage }: ResumeUploadStepProps) {
  const [isDragging, setIsDragging] = useState(false)
  const [localError, setLocalError] = useState<string | null>(null)
  const fileInputRef = useRef<HTMLInputElement>(null)

  function validateAndUpload(file: File) {
    if (!ALLOWED_TYPES.includes(file.type)) {
      setLocalError('Only PDF, DOC, or DOCX files are accepted.')
      return
    }
    if (file.size > MAX_SIZE_BYTES) {
      setLocalError('File exceeds the 10 MB limit.')
      return
    }
    setLocalError(null)
    void onUpload(file)
  }

  function handleDrop(event: DragEvent<HTMLDivElement>) {
    event.preventDefault()
    setIsDragging(false)
    const file = event.dataTransfer.files[0]
    if (file) validateAndUpload(file)
  }

  return (
    <div className="flex flex-col gap-4">
      <div
        onDragOver={(e) => {
          e.preventDefault()
          setIsDragging(true)
        }}
        onDragLeave={() => setIsDragging(false)}
        onDrop={handleDrop}
        onClick={() => fileInputRef.current?.click()}
        className={`flex cursor-pointer flex-col items-center justify-center gap-2 rounded-lg border-2 border-dashed px-6 py-12 text-center transition-colors ${
          isDragging ? 'border-brand-500 bg-brand-50' : 'border-slate-300 hover:border-brand-400'
        }`}
      >
        <input
          ref={fileInputRef}
          type="file"
          accept=".pdf,.doc,.docx"
          className="hidden"
          onChange={(e) => {
            const file = e.target.files?.[0]
            if (file) validateAndUpload(file)
          }}
        />
        {resume ? (
          <p className="text-sm text-slate-700">
            <span className="font-medium text-brand-700">{resume.originalFileName}</span> uploaded.
            Click or drop a file here to replace it.
          </p>
        ) : (
          <>
            <p className="text-sm font-medium text-slate-700">Drag and drop a resume here, or click to browse</p>
            <p className="text-xs text-slate-400">PDF, DOC, or DOCX — max 10 MB</p>
          </>
        )}
        {isUploading && <p className="text-sm text-brand-600">Uploading…</p>}
      </div>

      {(localError || errorMessage) && (
        <p role="alert" className="text-sm text-red-600">
          {localError ?? errorMessage}
        </p>
      )}

      <div className="flex justify-between">
        <Button type="button" variant="ghost" onClick={onBack}>
          Back
        </Button>
        <Button type="button" disabled={!resume} onClick={onNext}>
          Next
        </Button>
      </div>
    </div>
  )
}
