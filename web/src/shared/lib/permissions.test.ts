import { describe, expect, it } from 'vitest'
import { hasPermission } from './permissions'

describe('hasPermission', () => {
  it('returns true when the permission is present', () => {
    expect(hasPermission(['Recruitment.Job.Create', 'Recruitment.Job.View'], 'Recruitment.Job.Create')).toBe(true)
  })

  it('returns false when the permission is missing', () => {
    expect(hasPermission(['Recruitment.Job.View'], 'Recruitment.Job.Publish')).toBe(false)
  })

  it('returns false for an empty permission list', () => {
    expect(hasPermission([], 'Recruitment.Job.View')).toBe(false)
  })
})
