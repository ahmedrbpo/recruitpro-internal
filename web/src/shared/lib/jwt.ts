interface JwtClaims {
  email: string
  permission: string[] | string | undefined
}

/** Decodes the JWT payload client-side to read the email/permission claims the API already
 * flattens into the token at issuance — no extra "current user" round-trip needed, and nothing
 * here is secret (the same claims gate every server-side authorization check anyway). */
export function decodeAccessToken(accessToken: string): { email: string; permissions: string[] } {
  const payloadSegment = accessToken.split('.')[1]
  const json = atob(payloadSegment.replace(/-/g, '+').replace(/_/g, '/'))
  const claims = JSON.parse(json) as JwtClaims

  const permission = claims.permission
  const permissions = Array.isArray(permission) ? permission : permission ? [permission] : []

  return { email: claims.email, permissions }
}
