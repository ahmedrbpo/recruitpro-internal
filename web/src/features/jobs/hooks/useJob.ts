import { useQuery } from '@tanstack/react-query'
import { getJob } from '../api/jobsApi'

export function useJob(jobId: string | undefined) {
  return useQuery({
    queryKey: ['jobs', jobId],
    queryFn: () => getJob(jobId!),
    enabled: !!jobId,
  })
}
