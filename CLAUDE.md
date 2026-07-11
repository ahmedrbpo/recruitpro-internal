# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project status

No code exists yet — this repo currently contains only the architecture blueprint
(`RecruitPro-SaaS-Architecture-Blueprint-Updated.md`). The next step is Phase 0 of the
roadmap: solution structure, EF Core + Postgres setup, identity schema, JWT auth end-to-end,
and a CI pipeline skeleton, before any recruitment feature work begins.

Full architecture detail: @RecruitPro-SaaS-Architecture-Blueprint-Updated.md

## What RecruitPro is

An internal, **single-tenant** recruitment management platform for Coventine Digital
(job requisitions → candidate sourcing → interview pipeline → offers → reporting). It is
not multi-tenant and never will be within this design — **never add a `tenant_id` or
discriminator column to any table**; every row belongs to Coventine outright.

## Stack

- Backend: ASP.NET Core 8, EF Core, PostgreSQL, MediatR, FluentValidation, AutoMapper
- Frontend: React + Vite, TanStack Query, Zustand, Tailwind, axios
- Hosting: MonsterASP.NET (API), Supabase (PostgreSQL + file storage)
- Background jobs: Hangfire on the same Postgres instance (no separate infra)

The blueprint document contains leftover AWS-specific terminology in places (ECS, RDS, S3,
ALB, CloudWatch, SES, X-Ray, GuardDuty, DynamoDB, Terraform `backend.tf` in Supabase Storage) —
these are stale artifacts from an earlier draft. Treat them as their MonsterASP.NET/Supabase
equivalents, not as the actual target infra.

## Architecture rules

- **Clean Architecture, one-way dependencies**: Domain → (nothing) ← Application ← Infrastructure ← Api.
  Domain has zero dependencies. Business rules and invariants live in domain methods
  (e.g. `Job.Publish()` enforces "must have a salary range"), not in handlers.
- **CQRS via MediatR**: every state change is a `Command`, every read is a `Query`. Both
  dispatched through MediatR with `ValidationBehavior` → `LoggingBehavior` → `PerformanceBehavior`
  pipeline behaviors running in that order. One PostgreSQL database for both reads and writes —
  do not introduce a separate read store.
- **Authorization is permission-string based, not role-based.** Check
  `[RequirePermission("Recruitment.Job.Publish")]`-style permission strings flattened into JWT
  claims at issuance, never role names directly — roles are just a UI grouping of permissions.
- **Soft delete only.** Every entity has `IsDeleted`; a `SaveChangesInterceptor` converts
  `DbContext.Remove()` into `IsDeleted = true`. Never issue a hard `DELETE`.
- **Every table** carries `CreatedAt/By`, `ModifiedAt/By`, `IsDeleted`, and `row_version`
  (EF Core concurrency token, mapped to a `bytea` column) — set automatically via interceptors,
  not manually in handlers.
- **UUID primary keys** via `gen_random_uuid()`, not SQL Server `IDENTITY` columns.
- **API convention**: `/api/v1/...`, response envelope `{ success, data, error, meta }`,
  errors mapped to RFC 7807 `ProblemDetails`, offset pagination (`page`, `pageSize`).
- **File uploads** go direct to Supabase Storage via presigned URLs (requested from the API
  after a permission check) — files never pass through the ASP.NET Core process.

## Deliberately deferred (do not build speculatively)

Redis, message queues, container orchestration (ECS/Kubernetes), Terraform, read replicas,
and multi-AZ failover are all **Part 2** extensions in the blueprint, each gated behind a named
trigger condition (e.g. "more than one API instance running" for Redis). Do not introduce
these ahead of an actual observed trigger — check the blueprint's trigger tables before adding
any of them.

## Testing (per blueprint, once code exists)

xUnit + FluentAssertions + NSubstitute for unit tests; Testcontainers-backed Postgres +
`WebApplicationFactory` for integration tests; Playwright for E2E; k6 for load testing before
major releases.

## Subdirectory CLAUDE.md files

Once the backend (`src/RecruitPro.*`) and frontend (`src/`) trees exist, consider adding
module-specific CLAUDE.md files in subdirectories — they load automatically when Claude works
in that directory.
