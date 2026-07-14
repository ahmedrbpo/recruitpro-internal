import { useMutation, useQueryClient } from '@tanstack/react-query'
import { publishJob } from '../api/jobsApi'

export function usePublishJob() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: publishJob,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['jobs'] })
    },
  })
}
