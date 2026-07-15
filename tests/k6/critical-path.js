// Load test for RecruitPro's read-heavy critical path, per CLAUDE.md's testing strategy
// ("k6 — Before Phase 6 launch, and before any significant traffic-pattern change") and its
// NFR table (p95 < 300ms for read endpoints).
//
// Usage:
//   k6 run tests/k6/critical-path.js
//   k6 run -e BASE_URL=https://recruitpro.runasp.net -e LOGIN_EMAIL=... -e LOGIN_PASSWORD=... tests/k6/critical-path.js
//
// Defaults target the local dev API with the seeded administrator account (see
// RbacSeedData.cs / the seeded coventine.com accounts) — override via -e for other environments.
//
// Scoped to read endpoints only, deliberately: the bearer access token from a single setup()
// login is shared across all VUs (valid — JWTs aren't tied to a connection or cookie jar), but
// write endpoints are excluded because (a) repeatedly creating real jobs/candidates under load
// pollutes whichever database this runs against, and (b) /auth/login itself is now rate-limited
// to 10 requests/minute per IP (see Program.cs's AuthPolicy) — a load test hammering it from one
// machine would just generate 429s, not useful latency data. Write-path load testing needs its
// own seeded-data setup and is intentionally left for a follow-up script.

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

const BASE_URL = __ENV.BASE_URL || 'https://localhost:7098';
const LOGIN_EMAIL = __ENV.LOGIN_EMAIL || 'administrator@coventine.com';
const LOGIN_PASSWORD = __ENV.LOGIN_PASSWORD || 'Passw0rd!2026';

const readLatency = new Trend('read_endpoint_duration', true);

export const options = {
  scenarios: {
    critical_path: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '30s', target: 20 },
        { duration: '2m', target: 20 },
        { duration: '30s', target: 0 },
      ],
    },
  },
  thresholds: {
    // Matches the NFR table in CLAUDE.md / the architecture blueprint.
    read_endpoint_duration: ['p(95)<300'],
    http_req_failed: ['rate<0.01'],
  },
  insecureSkipTLSVerify: true,
};

export function setup() {
  const loginRes = http.post(
    `${BASE_URL}/api/v1/auth/login`,
    JSON.stringify({ email: LOGIN_EMAIL, password: LOGIN_PASSWORD }),
    { headers: { 'Content-Type': 'application/json' } },
  );

  check(loginRes, { 'login succeeded': (r) => r.status === 200 });

  return { accessToken: loginRes.json('data.accessToken') };
}

export default function (data) {
  const authHeaders = { headers: { Authorization: `Bearer ${data.accessToken}` } };

  // Paginated job list — the highest-traffic dashboard-style endpoint per the blueprint's own
  // indexing-strategy note ("verify with EXPLAIN ANALYZE on the heaviest dashboard queries").
  const jobsRes = http.get(`${BASE_URL}/api/v1/jobs?page=1&pageSize=20`, authHeaders);
  check(jobsRes, { 'jobs list: 200': (r) => r.status === 200 });
  readLatency.add(jobsRes.timings.duration);

  // Reporting dashboard aggregate.
  const dashboardRes = http.get(`${BASE_URL}/api/v1/reporting/dashboard`, authHeaders);
  check(dashboardRes, { 'dashboard: 200': (r) => r.status === 200 });
  readLatency.add(dashboardRes.timings.duration);

  // A single job, if the list returned any.
  const jobs = jobsRes.json('data');
  if (Array.isArray(jobs) && jobs.length > 0) {
    const jobRes = http.get(`${BASE_URL}/api/v1/jobs/${jobs[0].id}`, authHeaders);
    check(jobRes, { 'job by id: 200': (r) => r.status === 200 });
    readLatency.add(jobRes.timings.duration);
  }

  sleep(1);
}
