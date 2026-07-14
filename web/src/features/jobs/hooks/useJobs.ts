import { useQuery } from '@tanstack/react-query'
import { getJobs } from '../api/jobsApi'

export function useJobs(page: number, pageSize: number) {
  return useQuery({
    queryKey: ['jobs', page, pageSize],
    queryFn: () => getJobs(page, pageSize),
  })
}
