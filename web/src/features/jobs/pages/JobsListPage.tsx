import { useState } from 'react'
import { useJobs } from '../hooks/useJobs'
import { usePublishJob } from '../hooks/usePublishJob'
import { DataTable } from '../../../shared/components/DataTable'
import { Button } from '../../../shared/components/Button'
import { CreateJobModal } from '../components/CreateJobModal'
import { usePermission } from '../../../shared/hooks/usePermission'
import { useAuth } from '../../auth/hooks/useAuth'
import { getApiErrorMessage } from '../../../shared/lib/errors'
import type { Job } from '../types'

const PAGE_SIZE = 20

const statusBadgeClasses: Record<Job['status'], string> = {
  Draft: 'bg-slate-100 text-slate-700',
  PendingApproval: 'bg-amber-100 text-amber-700',
  Published: 'bg-emerald-100 text-emerald-700',
  OnHold: 'bg-amber-100 text-amber-700',
  Closed: 'bg-slate-200 text-slate-600',
  Archived: 'bg-slate-200 text-slate-600',
  Cancelled: 'bg-red-100 text-red-700',
}

export function JobsListPage() {
  const [page, setPage] = useState(1);
  const [isCreateOpen, setIsCreateOpen] = useState(false)
  const { data, isLoading, isError } = useJobs(page, PAGE_SIZE)
  const publishJob = usePublishJob()
  const canCreate = usePermission('Recruitment.Job.Create')
  const canPublish = usePermission('Recruitment.Job.Publish')
  const { email, logout } = useAuth()

  const totalPages = data ? Math.max(1, Math.ceil(data.totalCount / data.pageSize)) : 1

  const publishErrorMessage = publishJob.isError
    ? getApiErrorMessage(publishJob.error, 'Failed to publish job.')
    : null

  return (
    <div className="min-h-screen bg-slate-50 p-8">
      <div className="mx-auto max-w-5xl">
        <div className="mb-6 flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-semibold text-slate-900">Jobs</h1>
            <p className="text-sm text-slate-500">Signed in as {email}</p>
          </div>
          <div className="flex gap-2">
            {canCreate && <Button onClick={() => setIsCreateOpen(true)}>New job</Button>}
            <Button variant="secondary" onClick={() => logout()}>
              Sign out
            </Button>
          </div>
        </div>

        {publishErrorMessage && (
          <p role="alert" className="mb-4 rounded-md bg-red-50 px-3 py-2 text-sm text-red-700">
            {publishErrorMessage}
          </p>
        )}

        <div className="rounded-lg bg-white shadow-sm">
          {isLoading && <p className="p-6 text-sm text-slate-500">Loading…</p>}
          {isError && <p className="p-6 text-sm text-red-600">Failed to load jobs.</p>}
          {data && (
            <DataTable
              columns={[
                { key: 'jobCode', header: 'Code', render: (job) => job.jobCode },
                { key: 'title', header: 'Title', render: (job) => job.title },
                {
                  key: 'status',
                  header: 'Status',
                  render: (job) => (
                    <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${statusBadgeClasses[job.status]}`}>
                      {job.status}
                    </span>
                  ),
                },
                { key: 'employmentType', header: 'Type', render: (job) => job.employmentType },
                { key: 'workMode', header: 'Mode', render: (job) => job.workMode },
                { key: 'skills', header: 'Skills', render: (job) => job.skills.join(', ') || '—' },
                {
                  key: 'actions',
                  header: '',
                  render: (job) =>
                    canPublish && job.status === 'Draft' ? (
                      <Button
                        variant="secondary"
                        disabled={publishJob.isPending}
                        onClick={() => publishJob.mutate(job.id)}
                      >
                        Publish
                      </Button>
                    ) : null,
                },
              ]}
              rows={data.items}
              rowKey={(job) => job.id}
              emptyMessage="No jobs yet."
            />
          )}
        </div>

        {data && data.totalCount > 0 && (
          <div className="mt-4 flex items-center justify-between text-sm text-slate-600">
            <span>
              Page {data.page} of {totalPages} · {data.totalCount} total
            </span>
            <div className="flex gap-2">
              <Button variant="secondary" disabled={page <= 1} onClick={() => setPage((p) => p - 1)}>
                Previous
              </Button>
              <Button variant="secondary" disabled={page >= totalPages} onClick={() => setPage((p) => p + 1)}>
                Next
              </Button>
            </div>
          </div>
        )}
      </div>

      <CreateJobModal isOpen={isCreateOpen} onClose={() => setIsCreateOpen(false)} />
    </div>
  )
}
