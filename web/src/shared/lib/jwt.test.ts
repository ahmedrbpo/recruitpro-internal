import { describe, expect, it } from 'vitest'
import { decodeAccessToken } from './jwt'

function fakeJwt(payload: Record<string, unknown>): string {
  const base64Url = (value: string) => btoa(value).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '')
  const header = base64Url(JSON.stringify({ alg: 'HS256', typ: 'JWT' }))
  const body = base64Url(JSON.stringify(payload))
  return `${header}.${body}.signature`
}

describe('decodeAccessToken', () => {
  it('extracts email and a permission array', () => {
    const token = fakeJwt({ email: 'a@example.com', permission: ['Recruitment.Job.View', 'Recruitment.Job.Create'] })

    const claims = decodeAccessToken(token)

    expect(claims.email).toBe('a@example.com')
    expect(claims.permissions).toEqual(['Recruitment.Job.View', 'Recruitment.Job.Create'])
  })

  it('normalizes a single string permission claim into an array', () => {
    const token = fakeJwt({ email: 'b@example.com', permission: 'Recruitment.Job.View' })

    const claims = decodeAccessToken(token)

    expect(claims.permissions).toEqual(['Recruitment.Job.View'])
  })

  it('returns an empty array when there is no permission claim', () => {
    const token = fakeJwt({ email: 'c@example.com' })

    const claims = decodeAccessToken(token)

    expect(claims.permissions).toEqual([])
  })
})
