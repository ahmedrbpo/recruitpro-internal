import { AxiosError, AxiosHeaders } from 'axios'
import { describe, expect, it } from 'vitest'
import { getApiErrorMessage } from './errors'

function axiosErrorWithBody(data: unknown): AxiosError {
  return new AxiosError('Request failed', 'ERR_BAD_REQUEST', undefined, undefined, {
    data,
    status: 400,
    statusText: 'Bad Request',
    headers: {},
    config: { headers: new AxiosHeaders() },
  })
}

describe('getApiErrorMessage', () => {
  it('reads the message from the {success,data,error} envelope used by Result<T> failures', () => {
    const error = axiosErrorWithBody({
      success: false,
      data: null,
      error: { code: 'Conflict', message: "Skill 'C#' already exists." },
      meta: null,
    })

    expect(getApiErrorMessage(error, 'fallback')).toBe("Skill 'C#' already exists.")
  })

  it('reads the title from an RFC 7807 ProblemDetails body used for thrown DomainExceptions', () => {
    const error = axiosErrorWithBody({
      type: 'https://httpstatuses.io/400',
      title: 'A job must have a salary range before it can be published.',
      status: 400,
      errorCode: 'domain_rule_violation',
    })

    expect(getApiErrorMessage(error, 'fallback')).toBe('A job must have a salary range before it can be published.')
  })

  it('falls back to the detail field when a ProblemDetails body has no title', () => {
    const error = axiosErrorWithBody({ detail: 'Something went wrong.' })

    expect(getApiErrorMessage(error, 'fallback')).toBe('Something went wrong.')
  })

  it('returns the fallback for a non-axios error', () => {
    expect(getApiErrorMessage(new Error('boom'), 'fallback')).toBe('fallback')
  })

  it('returns the fallback when the response body matches neither shape', () => {
    const error = axiosErrorWithBody({ unexpected: true })

    expect(getApiErrorMessage(error, 'fallback')).toBe('fallback')
  })
})
