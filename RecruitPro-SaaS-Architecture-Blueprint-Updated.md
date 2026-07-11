# RecruitPro --- Solution Architecture Blueprint

**Source:** Converted from the uploaded PDF.

------------------------------------------------------------------------

## Page 1

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 1 of 30 RECRUITPRO Internal Recruitment Management Platform
Solution Architecture & Implementation Blueprint Clean Architecture ·
CQRS · ASP.NET Core 8 · PostgreSQL · React/Vite · Prepared for
Coventine Digital Private Limited July 2026

------------------------------------------------------------------------

## Page 2

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 2 of 30 Table of Contents Executive Summary
...................................................................................................................................................
3 Objectives
...............................................................................................................................................................
3 Why Phase It This Way
............................................................................................................................................
3 Part 1 --- Core Platform
Architecture.......................................................................................................................
5 High-Level System Architecture
...............................................................................................................................
5 Clean Architecture Folder Structure
........................................................................................................................
7 Domain Model & Database Schema Design
.........................................................................................................
10 Authentication & Authorization Design
................................................................................................................
13 CQRS + MediatR Implementation
Strategy...........................................................................................................
15 API Design
.................................................................................................................................................................
16 Data Management
...................................................................................................................................................
17 Security Best Practices
............................................................................................................................................
19 AWS Supabase Storage File Storage Design
....................................................................................................................................
20 Docker & Deployment Architecture
......................................................................................................................
21 CI/CD Workflow
.......................................................................................................................................................
23 Testing Strategy
.......................................................................................................................................................
24 Non-Functional Requirements
...............................................................................................................................
25 Core Development Roadmap
.................................................................................................................................
26 Part 2 --- Scaling & Enterprise Extensions
.............................................................................................................
27 Caching
Strategy....................................................................................................................................................
27 Background Processing
..........................................................................................................................................
27 Scalability & Horizontal Scaling
..............................................................................................................................
28 Audit, Compliance & Observability
........................................................................................................................
28 Infrastructure as Code: Terraform
.........................................................................................................................
28 Suggested Third-Party Integrations
........................................................................................................................
29 Risks, Trade-Offs & Architectural Decisions --- Summary
........................................................................................
29 Closing Notes
............................................................................................................................................................
30

------------------------------------------------------------------------

## Page 3

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 3 of 30 Executive Summary RecruitPro is Coventine Digital
Private Limited's internal recruitment management system, evolving from
a single- tenant SQL Server-backed application into a modern,
single-organization platform built on ASP.NET Core 8, PostgreSQL, and a
React/Vite front end. It exists to run Coventine's own recruitment
operations --- job requisitions, candidate sourcing, the interview
pipeline, offers, and reporting --- for Coventine's internal recruiters,
delivery managers, and leadership. It is not built to be resold or
white-labelled to outside companies, and this document does not include
any multi-tenant, tenant-isolation, or per-customer billing concerns;
every design decision below assumes one organization, one deployment,
and one shared database that belongs entirely to Coventine. This
document is intentionally phased. Internal recruitment platforms fail
more often from over-engineering at day one than from under-engineering
--- teams sink months into Kubernetes, microservices, and event sourcing
before a single recruiter has logged in. The blueprint is split into two
parts: • Part 1 --- Core Platform Architecture: the minimum
production-grade slice that gets a secure, reliable recruitment platform
live for Coventine's team. Modular monolith, single PostgreSQL database,
JWT authentication, the full recruitment workflow, deployed on a single
MonsterASP.NET instance behind a load balancer with Vercel for the front end. •
Part 2 --- Scaling & Enterprise Extensions: the upgrades you reach for
only when a specific, named trigger is hit --- Redis caching,
background-job infrastructure, read replicas, horizontal scaling,
advanced audit/compliance tooling, and Terraform-managed infrastructure.
Each extension lists the trigger condition that justifies adopting it,
so nothing here gets built speculatively. Objectives • Replace the
existing SQL Server single-tenant database with a PostgreSQL schema that
keeps all 41 existing tables' meaning and data intact while adding the
audit, concurrency, and indexing conventions this document defines. •
Give Coventine's recruitment team one authoritative system of record for
jobs, candidates, applications, interviews, and offers, replacing
spreadsheet- and email-based tracking. • Establish a codebase that a
small engineering team (1--3 developers) can operate confidently without
a dedicated DevOps hire, while leaving clean extension points for later
scale. • Keep monthly infrastructure spend predictable and low until
usage data justifies additional investment. Why Phase It This Way Every
architectural decision in this document is justified against three
questions: does it solve a problem Coventine has today, does it block a
problem the team will have in 6--12 months if skipped, and what does it
cost in development time and AWS spend. The table below previews how
that trade-off plays out across the decisions that most affect timeline
and cost. Decision Area Core Platform Choice Trigger to Revisit in Part
2 Architecture style Modular monolith (Clean Architecture layers, one
deployable) A single module's scaling needs diverge sharply (e.g.,
resume parsing needs GPU, the API doesn't) --- extract that module as a
service

------------------------------------------------------------------------

## Page 4

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 4 of 30 Decision Area Core Platform Choice Trigger to Revisit in
Part 2 Caching In-memory (IMemoryCache) for hot lookups only More than
one API instance running, or p95 latency on read-heavy endpoints exceeds
budget --- add Redis Background jobs Hangfire with PostgreSQL storage
(no separate infra) Job volume or processing time requires dedicated
workers --- move to SQS- backed workers Hosting Single MonsterASP.NET instance (or
ECS with 1 task), Supabase PostgreSQL single-AZ Uptime requirements demand
multi-AZ, or traffic requires horizontal scaling --- move to ECS
Fargate + Multi-AZ RDS Environment strategy Two environments (staging,
production), manual promotion after review A third environment is
needed, or release cadence increases enough that manual promotion
becomes a bottleneck --- automate via CI/CD gates Infra-as-code Manual
console setup + a documented runbook More than one engineer is
provisioning infrastructure, or environment drift is discovered ---
adopt Terraform

How to read this document Part 1 is written to be built. Part 2 is
written to be referenced --- read it once now to understand the seams to
leave in the core platform code (interfaces, abstractions, naming
conventions) so that adopting a Part 2 extension later is a swap, not a
rewrite.

------------------------------------------------------------------------

## Page 5

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 5 of 30 Part 1 --- Core Platform Architecture Everything in this
part is scoped to ship a real, secure, single-organization recruitment
platform for Coventine with paying-customer-grade reliability ---
without infrastructure the team isn't yet big enough to operate.
High-Level System Architecture The core platform is a modular monolith:
one ASP.NET Core Web API process containing clearly separated modules
(Identity, Recruitment,  Reporting), one PostgreSQL
database, and one React SPA. This keeps deployment, debugging, and
transaction management simple while preserving internal module
boundaries so the system can be split into services later if a specific
module's load profile demands it. Because RecruitPro serves a single
organization, there is no tenant-resolution layer, no per-organization
routing, and no discriminator column running through every table. Every
row in the database belongs to Coventine outright. This removes an
entire category of application-layer and database-layer complexity that
a multi-tenant SaaS design would otherwise require --- no tenant claim
to thread through every request, no global query filter to keep in sync
with a Row-Level Security policy, and no risk class of cross-tenant data
leakage to defend against. Isolation of concern is instead handled the
ordinary way: role-based access control scoped to what a given
recruiter, hiring manager, or admin is allowed to see and do within the
one organization.

Figure 1. Component-level system architecture --- request path from the
browser through to storage and messaging. Component responsibilities
Component Responsibility Why this choice React + Vite (Vercel) SPA:
recruiter dashboard, candidate pipeline, job postings, admin console
Vercel's free/pro tier handles CDN, preview deployments, and CI on git
push with zero infra setup Application Load Balancer (ALB) TLS
termination, health checks, future multi- instance routing Even a
single-MonsterASP.NET deployment benefits from an ALB now --- adding a second
instance later is a target- group change, not a re-architecture ASP.NET
Core 8 API (MonsterASP.NET) All business logic: authentication, recruitment
workflow, notifications, reporting One deployable simplifies CI/CD,
debugging, and transactions across modules during early iteration RDS
PostgreSQL System of record for the entire recruitment domain A single
managed instance is cheap, backed up automatically, and

------------------------------------------------------------------------

## Page 6

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 6 of 30 Component Responsibility Why this choice comfortably
handles Coventine's data volume for years Supabase Storage Resumes, candidate
documents, company assets Durable, cheap, and natively supports signed
URLs for secure, time-boxed access 
Transactional email and  notifications Both are pay-per-use with
no standing infrastructure cost

Deliberately deferred No Redis, no message queue, no container
orchestration, and no Terraform in the core platform. Each is addressed
in Part 2 with the specific signal that tells you it's time to add it.

Request lifecycle walkthrough To make the architecture concrete, here is
what happens end-to-end when a recruiter publishes a job posting from
the React SPA:

Figure 2. Request lifecycle for a single write operation, from browser
to UI cache invalidation.

------------------------------------------------------------------------

## Page 7

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 7 of 30 1. The browser sends POST /api/v1/jobs/{id}/publish with
the recruiter's JWT access token in the Authorization header. 2. The ALB
terminates TLS and forwards the request to the MonsterASP.NET instance running the
API container. 3. ASP.NET Core's authentication middleware validates the
JWT signature and expiry, then populates the ClaimsPrincipal used by the
rest of the pipeline. 4. Authorization middleware checks the caller
holds the Recruitment.Job.Publish permission before the request reaches
MediatR. 5. The PublishJobCommand is dispatched through MediatR; the
ValidationBehavior, LoggingBehavior, and PerformanceBehavior pipeline
behaviors run in order around the handler (see "CQRS + MediatR
Implementation Strategy"). 6. The handler loads the Job aggregate via EF
Core, calls its domain method Publish() (which enforces invariants such
as "must have a salary range before publishing"), and calls
SaveChangesAsync. 7. A SaveChangesInterceptor stamps
ModifiedAt/ModifiedBy and writes an AuditLog row automatically, without
the handler needing to know about auditing. 8. The handler returns a
Result`<JobDto>`{=html}; the API layer maps it to the standard response
envelope described in "API Design" and returns 200 OK. 9. The SPA's
TanStack Query cache is invalidated for the jobs list, and the UI
reflects the published status without a full page reload. Clean
Architecture Folder Structure Four layers, each with a one-way
dependency rule: Domain depends on nothing; Application depends on
Domain; Infrastructure depends on Application and Domain; API depends on
all three. This is what makes the modular monolith splittable later ---
a module's Infrastructure can be swapped (for example, the EF Core
repository for a different data store) without touching Domain or
Application.

Figure 3. Layer dependency direction --- arrows point from a layer to
what it depends on. Domain depends on nothing.

------------------------------------------------------------------------

## Page 8

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 8 of 30 Backend project structure RecruitPro.sln ├── src/ │ ├──
RecruitPro.Domain/ \# Layer 1 --- no dependencies │ │ ├── Identity/ │ │
│ └── Entities/ (ApplicationUser, Role, Permission) │ │ ├── Recruitment/
│ │ │ ├── Entities/ (Job, Candidate, Application, Interview, Offer) │ │
│ ├── ValueObjects/ (SalaryRange, ApplicationStage) │ │ │ └── Events/
(ApplicationStageChangedEvent, OfferExtendedEvent) │ │ ├──
/ │ │ │ └── Entities/ (NotificationTemplate,
Log) │ │ └── Common/ │ │ ├── Entities/BaseEntity.cs (Id,
CreatedAt, CreatedBy, IsDeleted, RowVersion) │ │ ├── IDomainEvent.cs │ │
└── Exceptions/ (DomainException and subclasses) │ │ │ ├──
RecruitPro.Application/ \# Layer 2 --- depends on Domain │ │ ├──
Identity/ │ │ │ └── Commands\|Queries (Login, RefreshToken,
ResetPassword, AssignRole) │ │ ├── Recruitment/ │ │ │ ├── Jobs/
(CreateJob, PublishJob, GetJobsPaginated) │ │ │ ├── Candidates/
(UploadResume, GetCandidateProfile) │ │ │ ├── Applications/
(MoveApplicationStage, GetPipeline) │ │ │ └── Interviews/
(ScheduleInterview, RecordFeedback) │ │ ├── / │ │ │ └──
Commands\|Queries (SendNotification, GetHistory) │ │ ├──
Common/ │ │ │ ├── Behaviors/ (ValidationBehavior, LoggingBehavior,
PerformanceBehavior) │ │ │ ├── Interfaces/ (IApplicationDbContext,
ICurrentUserService, IDateTimeProvider) │ │ │ └── Models/
(PaginatedList`<T>`{=html}, Result`<T>`{=html}) │ │ └──
DependencyInjection.cs (registers MediatR, FluentValidation, AutoMapper)
│ │ │ ├── RecruitPro.Infrastructure/ \# Layer 3 --- depends on
Application + Domain │ │ ├── Persistence/ │ │ │ ├──
ApplicationDbContext.cs │ │ │ ├── Configurations/ (EF Core
IEntityTypeConfiguration per entity) │ │ │ ├── Interceptors/
(AuditableEntitySaveChangesInterceptor, SoftDeleteInterceptor) │ │ │ └──
Migrations/ │ │ ├── Identity/ (JwtTokenService, PasswordHasher wiring) │
│ ├── Files/ (Supabase StorageFileStorageService : IFileStorageService) │ │ ├──
/ (SesEmailService, Service) │ │ ├──
BackgroundJobs/ (Hangfire job definitions) │ │ └──
DependencyInjection.cs │ │ │ └── RecruitPro.Api/ \# Layer 4 ---
composition root │ ├── Controllers/ (JobsController,
CandidatesController, ...) │ ├── Middleware/
(ExceptionHandlingMiddleware, RequestLoggingMiddleware) │ ├── Filters/ │
├── Program.cs │ └── appsettings.json │ └── tests/ ├──
RecruitPro.Domain.Tests/ ├── RecruitPro.Application.Tests/ └──
RecruitPro.Api.IntegrationTests/ (Testcontainers-backed Postgres)

------------------------------------------------------------------------

## Page 9

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 9 of 30 Frontend project structure src/ ├── app/ │ ├──
router.tsx \# route definitions, lazy-loaded pages │ ├── store.ts \#
Zustand store composition │ └── queryClient.ts \# TanStack Query client
config │ ├── features/ │ ├── auth/ │ │ ├── api/ (login, refreshToken,
resetPassword) │ │ ├── hooks/ (useAuth, useCurrentUser) │ │ └── pages/
(LoginPage, ForgotPasswordPage) │ ├── jobs/ │ │ ├── api/ hooks/
components/ pages/ │ ├── candidates/ │ ├── pipeline/ \# kanban-style
application tracking board │ ├── interviews/ │ ├── reporting/ │ └──
admin/ \# user & role management console │ ├── shared/ │ ├── components/
\# Button, DataTable, Modal, FormField (design system) │ ├── hooks/ \#
usePagination, useDebounce, usePermission │ ├── lib/ │ │ ├──
apiClient.ts \# axios instance, interceptors for JWT + refresh │ │ └──
permissions.ts \# RBAC permission-check helpers │ └── types/ \# shared
TS interfaces mirroring API DTOs │ ├── styles/ │ └── tailwind.css │ ├──
App.tsx └── main.tsx

------------------------------------------------------------------------

## Page 10

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 10 of 30 Domain Model & Database Schema Design This reorganizes
the existing \~41-table RecruitPro schema around three bounded contexts,
each owning its own tables and foreign-keying into User where needed.
Because RecruitPro is single-tenant, no table carries a discriminator
column for ownership --- every row already belongs to Coventine, so the
schema is exactly as wide as the recruitment domain requires and no
wider. Bounded contexts Context Core entities Owns Identity & Access
ApplicationUser, Role, Permission, RolePermission, UserRole,
RefreshToken, AuditLog Authentication, RBAC, audit trail Recruitment
Job, JobSkill, Candidate, Resume, Application, ApplicationStageHistory,
Interview, InterviewFeedback, Offer The core hiring workflow --- this is
most of the existing 41 tables  NotificationTemplate,
Log, EmailQueue, Log Outbound messaging and history

Entity-relationship overview At the center of the Recruitment context
sits Job, which owns many Applications; each Application links exactly
one Candidate to exactly one Job and carries a Stage that advances
through a fixed state machine (applied → screening → interview → offer →
hired/rejected). Interview and InterviewFeedback hang off Application,
one row per scheduled round and one feedback row per interviewer. Offer
is a 1:1 extension of an Application once it reaches the offer stage. On
the Identity side, ApplicationUser has a many-to-many relationship to
Role via UserRole, and Role has a many-to-many relationship to
Permission via RolePermission --- this indirection is what lets
Coventine redefine what a "Recruiter" or "Hiring Manager" is allowed to
do without a code change.  tables are intentionally
decoupled from Recruitment by foreign key only
(Log.RelatedEntityId + RelatedEntityType), so the messaging
subsystem can evolve independently.

------------------------------------------------------------------------

## Page 11

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 11 of 30

Figure 4. Core entity relationships across the Recruitment, Identity,
and  contexts. Recruitment pipeline state machine The
Application.Stage field (introduced in the table above) is not a
free-text status --- it is a fixed state machine enforced in the domain
layer. A stage transition is only valid along the edges shown below; the
MoveApplicationStage command rejects any transition that skips a stage
or moves out of a terminal state, so the pipeline's shape is guaranteed
by code rather than by convention alone.

Figure 5. Valid application stage transitions. Rejected is reachable
from every non-terminal stage; Hired is reachable only from Offer. Core
table definitions (representative subset) The full schema retains
Coventine's existing \~41 tables; shown here are the tables that anchor
audit and concurrency conventions --- the patterns every other table
follows. CREATE TABLE jobs ( id UUID PRIMARY KEY DEFAULT
gen_random_uuid(), title VARCHAR(300) NOT NULL, status VARCHAR(30) NOT
NULL DEFAULT 'draft', salary_min NUMERIC(12,2), salary_max
NUMERIC(12,2), department_id UUID REFERENCES departments(id), created_at
TIMESTAMPTZ NOT NULL DEFAULT now(), created_by UUID,

------------------------------------------------------------------------

## Page 12

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 12 of 30 modified_at TIMESTAMPTZ, modified_by UUID, is_deleted
BOOLEAN NOT NULL DEFAULT false, row_version BYTEA NOT NULL DEFAULT
'`\x00`{=tex}' );

-- Common high-traffic index pattern: CREATE INDEX ix_jobs_status ON
jobs (status) WHERE is_deleted = false;

CREATE TABLE applications ( id UUID PRIMARY KEY DEFAULT
gen_random_uuid(), job_id UUID NOT NULL REFERENCES jobs(id),
candidate_id UUID NOT NULL REFERENCES candidates(id), stage VARCHAR(30)
NOT NULL DEFAULT 'applied', created_at TIMESTAMPTZ NOT NULL DEFAULT
now(), row_version BYTEA NOT NULL DEFAULT '`\x00`{=tex}' ); CREATE INDEX
ix_applications_job ON applications (job_id); CREATE INDEX
ix_applications_candidate ON applications (candidate_id); CREATE INDEX
ix_applications_stage ON applications (stage) WHERE stage NOT IN
('hired','rejected');

Migrating from the SQL Server schema The converted T-SQL file is the
source of truth for column-level fidelity (data types, defaults,
constraints). The migration work here is structural, not data-type: add
the four audit columns plus row_version wherever missing, and replace
SQL Server IDENTITY columns with UUID defaults (gen_random_uuid()) so
IDs are safely generatable client-side and don't leak row counts. No
tenant_id column is introduced anywhere in the schema --- every table
stays exactly as wide as the domain requires.

------------------------------------------------------------------------

## Page 13

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 13 of 30 Authentication & Authorization Design JWT + refresh
token flow Login exchanges a valid email/password pair for a short-lived
JWT access token (15 minutes) and a longer-lived, rotating refresh token
(7 days, stored hashed in the RefreshToken table). The access token
carries the user's Id, Email, and a flattened list of permission strings
as claims --- flattened at issuance so the API never needs a database
round-trip to check "can this user do X" on every request; it only needs
to read a claim already present on the validated token. 10. Client calls
POST /api/v1/auth/login with credentials. 11. API validates credentials
via ASP.NET Identity's password hasher, checks the account isn't locked
out, and on success issues an access token + refresh token pair. 12. The
refresh token is stored as an httpOnly, Secure, SameSite=Strict cookie;
the access token is held in memory on the client (never localStorage, to
reduce XSS exposure). 13. When the access token expires, the client
calls POST /api/v1/auth/refresh; the API validates the refresh token
against the stored hash, rotates it (issues a new refresh token and
revokes the old one --- refresh token reuse detection flags the account
if an already-rotated token is replayed), and returns a new access
token. 14. Logout revokes the current refresh token server-side
immediately, rather than relying on client-side token deletion alone.

Figure 6. JWT issuance, use, expiry, and rotation --- including the
reuse-detection path that revokes all sessions. RBAC + permission-based
authorization Roles group permissions for usability, but every
authorization check at the API layer is against a permission string
(e.g., Recruitment.Job.Publish), not a role name --- this is what lets
an administrator redefine what a role can do

------------------------------------------------------------------------

## Page 14

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 14 of 30 later without a code change. A custom
\[RequirePermission("Recruitment.Job.Publish")\] attribute wraps ASP.NET
Core's policy-based authorization and reads the flattened permission
claims from the validated JWT. Roles and permissions Role Typical
permissions Administrator Full access: user management, all recruitment
modules, system settings, audit log viewer Recruitment Manager
Create/publish jobs, manage the full pipeline, view reports, cannot
manage system users or settings Recruiter Manage assigned jobs and
candidates, schedule interviews, cannot see other recruiters' pipelines
unless granted Hiring Manager Read-only on assigned jobs, can leave
interview feedback, approve/reject at offer stage Candidate-facing
(external) No platform login --- interacts via emailed links to a public
application portal

Password policy & account lockout Control Setting Minimum length 10
characters, checked against a common-password blocklist (top 10k
breached passwords) Complexity At least 3 of: upper, lower, digit,
symbol --- enforced via ASP.NET Identity options Lockout 5 failed
attempts → 15-minute lockout, exponential backoff on repeated lockouts
Password reset Time-boxed signed token (1-hour expiry) emailed via ,
single use Storage ASP.NET Identity's PBKDF2 hasher (default) ---
sufficient; no custom hashing

------------------------------------------------------------------------

## Page 15

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 15 of 30 CQRS + MediatR Implementation Strategy Every
state-changing operation is a Command, every read is a Query, both
dispatched through MediatR. This isn't about performance --- the
platform doesn't need separate read/write data stores --- it's about
keeping Application-layer code organized as the number of operations
grows past what a handful of "service classes" can hold cleanly, and
about getting cross-cutting concerns (validation, logging, performance
monitoring) for free on every handler via pipeline behaviors. // Command
public record PublishJobCommand(Guid JobId) :
IRequest\<Result`<JobDto>`{=html}\>;

public class PublishJobCommandHandler :
IRequestHandler\<PublishJobCommand, Result`<JobDto>`{=html}\> { private
readonly IApplicationDbContext \_db;

    public async Task<Result<JobDto>> Handle( 
        PublishJobCommand request, CancellationToken ct) 
    { 
        var job = await _db.Jobs.FindAsync(request.JobId, ct); 
        if (job is null) return Result<JobDto>.NotFound(); 

        job.Publish(); // domain method enforces invariants (e.g., must have a salary range) 
        await _db.SaveChangesAsync(ct); 

        return Result<JobDto>.Success(JobDto.FromEntity(job)); 
    } 

}

// Pipeline behaviors run around every handler, in this order:
services.AddMediatR(cfg =\> {
cfg.RegisterServicesFromAssembly(typeof(PublishJobCommand).Assembly);
cfg.AddOpenBehavior(typeof(ValidationBehavior\<,\>)); // throws on
invalid input cfg.AddOpenBehavior(typeof(LoggingBehavior\<,\>)); //
structured request/response logging
cfg.AddOpenBehavior(typeof(PerformanceBehavior\<,\>)); // warns on
handlers \> 500ms });

No separate read database CQRS as a pattern (command/query separation in
code) does not require CQRS as infrastructure (separate read/write
databases). The platform uses one PostgreSQL database for both ---
queries simply use AsNoTracking() and project directly to DTOs for
performance, without the operational overhead of a second data store or
eventual-consistency bugs.

------------------------------------------------------------------------

## Page 16

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 16 of 30 API Design Concern Convention Versioning URL segment:
/api/v1/... --- simplest to reason about, visible in every log line and
Swagger doc Response envelope { success, data, error: { code, message,
details }, meta: { page, pageSize, totalCount } } Error handling Global
ExceptionHandlingMiddleware maps exceptions to RFC 7807 ProblemDetails
with consistent error codes Validation FluentValidation per
command/query, executed in the MediatR ValidationBehavior before the
handler runs Pagination Offset pagination (page, pageSize) --- simplest,
fine until result sets exceed roughly 100k rows Filtering & sorting
Query string convention: ?status=open&sortBy=createdAt&sortDir=desc,
parsed into a shared QueryParameters object Documentation Swashbuckle
(Swagger/OpenAPI), auto-generated from XML doc comments and DTO
annotations

Sample request/response POST
/api/v1/jobs/3fa85f64-5717-4562-b3fc-2c963f66afa6/publish Authorization:
Bearer `<access_token>`{=html}

HTTP/1.1 200 OK { "success": true, "data": { "id":
"3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "Senior .NET
Developer", "status": "published", "salaryMin": 1500000, "salaryMax":
2200000 }, "error": null, "meta": null }

------------------------------------------------------------------------

## Page 17

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 17 of 30 Data Management Soft delete Every entity inherits
IsDeleted (bool) and is filtered out via an EF Core global query filter.
A SaveChangesInterceptor converts any DbContext.Remove() call into
setting IsDeleted = true instead of issuing a DELETE --- so application
code never has to remember to soft-delete manually, and accidental data
loss from a hard delete is not possible through the ORM. Audit fields &
audit logging Audit fields (CreatedAt/By, ModifiedAt/By) are set
automatically via the same interceptor pattern, reading the current user
from ICurrentUserService. For compliance-grade history beyond "who
touched it last," a separate AuditLog table records every command
execution --- entity type, entity id, action, before/after JSON diff,
user, timestamp --- written from the MediatR LoggingBehavior so it's
automatic for every command without per- handler boilerplate.
Concurrency handling Every table has a row_version column mapped as an
EF Core concurrency token. Two recruiters editing the same candidate
record simultaneously will produce a DbUpdateConcurrencyException on the
second save, which the API translates into a 409 Conflict with the
current server-side version so the client can show a merge/refresh
prompt rather than silently overwriting data. Migration strategy Stage
Approach Local dev EF Core Migrations (dotnet ef migrations add)
committed to source control, applied via dotnet ef database update CI
Migrations run against an ephemeral Postgres container as part of
integration tests --- catches broken migrations before merge Production
Migrations applied via a one-off ECS/MonsterASP.NET task or GitHub Actions step
before the new app version is deployed (migrate-then-deploy, not
migrate-on-startup, to avoid race conditions across multiple app
instances)

Indexing strategy • Composite indexes on the common filter column of
every high-traffic table --- e.g., (status) on jobs, (stage) on
applications. • Partial indexes WHERE is_deleted = false on tables where
soft-deleted rows would otherwise bloat the index and rarely get
queried. • Foreign key columns are indexed by default convention (EF
Core does this automatically); verify with EXPLAIN ANALYZE on the
heaviest dashboard queries before launch. Backup & recovery Control
Setting Automated backups RDS automated daily snapshots, 7-day retention
initially --- increase to 35 days once the system is in daily production
use Point-in-time recovery Enabled by default on RDS --- restore to any
second within the retention window

------------------------------------------------------------------------

## Page 18

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 18 of 30 Control Setting Manual snapshot cadence Before every
major migration or schema change, taken explicitly in addition to
automated backups Recovery testing Quarterly: restore latest snapshot to
a scratch RDS instance and verify the application boots against it

------------------------------------------------------------------------

## Page 19

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 19 of 30 Security Best Practices Layer Control Transport TLS
1.2+ enforced at the ALB; HSTS header on all responses Headers
Content-Security-Policy, X-Content-Type-Options: nosniff,
X-Frame-Options: DENY, Referrer- Policy: strict-origin-when-cross-origin
--- set via middleware, not per-controller Rate limiting ASP.NET Core's
built-in rate limiter: per-IP sliding window on auth endpoints
(stricter), per- user token bucket on general API endpoints Input
validation FluentValidation on every command/query; parameterized
queries only (EF Core handles this) --- no raw SQL string concatenation
anywhere Secrets AWS Secrets Manager for DB credentials and API keys ---
never in appsettings.json or source control File upload safety Resume
uploads validated by content-type sniffing (not just extension),
size-capped, scanned for type mismatches before Supabase Storage upload OWASP Top 10
coverage Covered by the above: injection (parameterized queries), broken
auth (JWT+refresh+lockout), sensitive data exposure (TLS+Secrets
Manager), XXE/SSRF (no XML parsing, Supabase Storage signed URLs only), security
misconfiguration (headers+least-privilege IAM)

------------------------------------------------------------------------

## Page 20

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 20 of 30 AWS Supabase Storage File Storage Design • Bucket layout:
s3://recruitpro-prod/resumes/{candidateId}/{fileId}.pdf --- simple, flat
organization by candidate since there is no tenant prefix to manage. •
Upload flow: client requests a presigned PUT URL from the API (which
validates permission first), uploads directly to Supabase Storage, then confirms
completion to the API --- files never pass through the ASP.NET Core
server, keeping it stateless and avoiding upload size limits on the app
tier. • Read flow: presigned GET URLs, 5-minute expiry, generated per
request --- never store permanent public Supabase Storage URLs. • Bucket policy: block
all public access at the bucket level; access only via presigned URLs
issued by the API after a permission check. • Encryption: SSE-Supabase Storage
(AES-256) enabled by default on the bucket; consider SSE-KMS if a future
audit requirement demands customer-managed keys.

------------------------------------------------------------------------

## Page 21

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 21 of 30 Docker & Deployment Architecture Dockerfile strategy
(multi-stage build) FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src COPY . . RUN dotnet restore
"RecruitPro.Api/RecruitPro.Api.csproj" RUN dotnet publish
"RecruitPro.Api/RecruitPro.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime WORKDIR /app COPY
--from=build /app/publish . EXPOSE 8080 ENTRYPOINT \["dotnet",
"RecruitPro.Api.dll"\]

docker-compose (local development) services: api: build: . ports:
\["8080:8080"\] environment: -
ConnectionStrings\_\_Default=Host=db;Database=recruitpro;Username=postgres;Password=devpassword
depends_on: \[db\] db: image: postgres:16 environment: POSTGRES_DB:
recruitpro POSTGRES_PASSWORD: devpassword ports: \["5432:5432"\]
volumes: \["pgdata:/var/lib/postgresql/data"\] volumes: pgdata:

Production deployment topology The core platform runs one Docker
container on a single MonsterASP.NET instance (t3.medium is a reasonable starting
size), managed by a simple systemd unit or Docker's own restart policy
--- no ECS/Kubernetes yet. This is deliberately the least operationally
complex option that still gets containerized, reproducible deploys.
Resource Sizing Monthly cost (ap-south-1, approx.) MonsterASP.NET (API) t3.medium
(2 vCPU, 4GB) \~\$30 Supabase PostgreSQL db.t3.micro, single-AZ, 20GB storage
\~\$15--20 Supabase Storage Pay-per-use, negligible at current volume \~\$1--5 ALB
Standard ALB pricing \~\$18  Pay-per-email, first 62k free if sent
from MonsterASP.NET \~\$0--5 Vercel (frontend) Hobby/Pro tier \$0--20 Total

\~\$65--90/month

------------------------------------------------------------------------

## Page 22

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 22 of 30

Cost-conscious by design This topology intentionally avoids NAT gateways
(\~\$32/mo each), Multi-AZ RDS (2x cost), and managed Kubernetes (EKS
control plane alone is \~\$73/mo) until traffic or SLA commitments
justify them. See Part 2 for exactly when to add each.

------------------------------------------------------------------------

## Page 23

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 23 of 30 CI/CD Workflow Frontend deploys via Vercel's native
GitHub integration --- every push to main triggers a production build,
every pull request gets a preview URL automatically, no custom pipeline
needed. Backend CI runs on GitHub Actions: on every pull request,
restore → build → run unit tests → run integration tests against an
ephemeral Postgres container → run dotnet format --verify-no-changes. On
merge to main, the same pipeline additionally builds and pushes a Docker
image to a registry, applies pending EF Core migrations against the
production database as a discrete step, then triggers a rolling restart
of the MonsterASP.NET service via SSM Run Command. \# .github/workflows/ci.yml
(abridged) name: CI on: \[pull_request, push\] jobs: build-and-test:
runs-on: ubuntu-latest services: postgres: image: postgres:16 env: {
POSTGRES_PASSWORD: test } ports: \["5432:5432"\] steps: - uses:
actions/checkout@v4 - uses: actions/setup-dotnet@v4 with: {
dotnet-version: '8.0.x' } - run: dotnet restore - run: dotnet build
--no-restore - run: dotnet test --no-build

------------------------------------------------------------------------

## Page 24

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 24 of 30 Testing Strategy A layered test strategy keeps
confidence high without slowing delivery. Each layer targets a different
kind of defect and runs at a different point in the pipeline. Layer
Scope Tooling Runs Unit tests Domain methods (e.g., Job.Publish()
invariants), command/query handlers with mocked dependencies xUnit,
FluentAssertions, NSubstitute On every commit, locally and in CI
Integration tests Full request pipeline against a real Postgres
instance, including EF Core migrations and MediatR behaviors
Testcontainers for Postgres, WebApplicationFactory On every pull request
in CI Contract tests API response shape matches the documented OpenAPI
schema Swashbuckle schema snapshot comparison On every pull request in
CI End-to-end tests Critical user flows through the real UI: login,
publish a job, move an application through pipeline stages, schedule an
interview Playwright Nightly against staging, and before a production
release Load testing Confirms the API holds up under expected concurrent
recruiter usage before major releases k6 Before Phase 6 launch, and
before any significant traffic- pattern change

What gets covered first • Domain invariants (a job cannot publish
without a salary range; an application cannot skip pipeline stages) ---
these protect data integrity and are cheap to test in isolation. •
Authentication and authorization paths --- a failure here is a security
incident, not just a bug. • The full job-to-hire happy path as an
end-to-end test, so a broken deploy is caught before recruiters hit it.

------------------------------------------------------------------------

## Page 25

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 25 of 30 Non-Functional Requirements Category Target Notes
Availability 99.0% monthly uptime for the core platform Single-AZ at
this stage; see "Scalability & Horizontal Scaling" for the Multi-AZ
trigger API latency p95 \< 300ms for read endpoints, p95 \< 800ms for
write endpoints Measured via the PerformanceBehavior warning threshold
and CloudWatch Concurrent users Comfortably supports Coventine's
internal recruiter headcount plus admin/reporting users Re-evaluate MonsterASP.NET
sizing if concurrent active sessions regularly exceed roughly 50 Data
durability No data loss beyond the last completed transaction RDS
automated backups + point-in-time recovery Recovery point objective
(RPO) ≤ 5 minutes Backed by RDS PITR Recovery time objective (RTO) ≤ 4
hours Manual restore-and-redeploy runbook until Multi-AZ is adopted
Browser support Latest two versions of Chrome, Edge, Firefox, Safari No
IE11 support required

------------------------------------------------------------------------

## Page 26

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 26 of 30 Core Development Roadmap Phase Scope Approx. duration
0. Foundations Solution structure, EF Core + Postgres setup, identity
schema, JWT auth, CI pipeline skeleton 2--3 weeks 1. Core recruitment
Job CRUD + publish, candidate + resume upload (Supabase Storage), application tracking
with stage transitions 3--4 weeks 2. Workflow completion Interview
scheduling, feedback capture, offer management, RBAC enforcement across
all endpoints 2--3 weeks 3.  Email notifications () for
key lifecycle events, notification templates,  integration for
candidate updates 2 weeks 4. Frontend buildout React SPA for all of the
above --- can run partially in parallel with phases 1-- 3 once API
contracts are stable 4--6 weeks (parallel) 5. Reporting + admin
Recruitment metrics dashboard, admin console, audit log viewer 2 weeks
6. Hardening + launch Security headers, rate limiting, load testing,
backup verification, Swagger polish, first production rollout to
Coventine's recruiters 1--2 weeks

------------------------------------------------------------------------

## Page 27

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 27 of 30 Part 2 --- Scaling & Enterprise Extensions None of the
following should be built speculatively. Each subsection states the
trigger condition first --- build it when the trigger fires, not before.
Building these early is the single most common way internal platform
rebuilds burn months without shipping a feature the recruitment team
actually asked for. Caching Strategy Until the trigger below fires,
ASP.NET Core's IMemoryCache covers hot, rarely-changing lookups
(permission sets, notification templates, system settings) with a short
TTL (60--300s) and explicit invalidation on writes. When the trigger
fires: ElastiCache Redis Trigger More than one API instance running
(in-memory cache would no longer be consistent across instances), or p95
latency on read-heavy endpoints exceeds budget.

Cache target Strategy System settings & feature flags Cache-aside,
invalidate on settings update, TTL 5 min as a backstop Permission
lookups Cache-aside keyed by userId, invalidated on role/permission
change Dashboard aggregates (e.g., pipeline counts) Cache with short TTL
(30--60s) --- acceptable staleness for a dashboard, large reduction in
DB load Session/refresh token denylist Redis is also the natural store
for revoked-token tracking once adopted, replacing a DB table lookup on
every request

Query optimization (do this before reaching for Redis) • Run EXPLAIN
ANALYZE on every endpoint backing a dashboard or list view; add missing
composite indexes before adding a cache layer that just papers over a
missing index. • Replace N+1 EF Core queries with explicit
.Include()/.AsSplitQuery() or projection directly to DTOs via .Select()
--- the latter is usually faster since it avoids materializing full
entities. • For genuinely heavy aggregate reporting queries, consider a
materialized view refreshed on a schedule rather than computing
aggregates live on every request. Background Processing Early stage:
Hangfire backed by the same PostgreSQL database (no new infrastructure)
handles scheduled and fire-and-forget jobs --- sending notification
emails, generating weekly reports, cleaning up expired refresh tokens.
Trigger Job volume or processing time grows large enough that background
work measurably competes with the primary database for resources, or job
latency becomes user-visible.

Past the trigger: move to SQS + dedicated worker processes (a separate
ECS service or MonsterASP.NET Auto Scaling Group), decoupled from the API tier so a
backlog of jobs never degrades API response times.

------------------------------------------------------------------------

## Page 28

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 28 of 30 Scalability & Horizontal Scaling Trigger Move past
single-instance MonsterASP.NET when sustained CPU/memory utilization exceeds
roughly 70%, when zero- downtime deploys are needed, or when uptime
requirements demand surviving an availability-zone failure.

From To What changes Single MonsterASP.NET instance ECS Fargate service, 2+ tasks
behind the existing ALB Stateless API design (already true --- no in-
memory session state) makes this a config change, not a code change
Single-AZ RDS Multi-AZ RDS with automatic failover No application code
changes; roughly 2x RDS cost for the availability guarantee No read
replicas RDS read replica for reporting queries Reporting/analytics
queries point at the replica connection string; write traffic stays on
primary IMemoryCache ElastiCache Redis (see Caching Strategy) Swap the
ICacheService implementation --- if the core platform code used the
interface consistently, this is a DI registration change Audit,
Compliance & Observability Capability Trigger to add Centralized
structured logging (CloudWatch Logs Insights or an ELK/OpenSearch stack)
More than one instance running --- SSH-ing into individual boxes to grep
logs stops working Distributed tracing (AWS X-Ray or OpenTelemetry) Once
any module is split into a separate service, or latency debugging across
MediatR pipeline behaviors becomes hard to reason about from logs alone
Immutable audit log export (e.g., to Supabase Storage with object lock) Coventine's
leadership or a client's compliance review asks for it --- don't build
this speculatively Anomaly/security alerting (e.g., AWS GuardDuty, login
anomaly detection) Once the system is business-critical enough that
manual security monitoring isn't feasible Infrastructure as Code:
Terraform When the trigger from the Executive Summary's decision table
fires, structure Terraform state by environment and avoid one giant root
module. infra/ ├── modules/ │ ├── networking/ \# VPC, subnets, security
groups │ ├── compute/ \# MonsterASP.NET/ECS, ALB, launch templates │ ├── database/
\# RDS instance, parameter groups, backups │ └── storage/ \# Supabase Storage buckets,
bucket policies ├── environments/ │ ├── staging/ │ │ └── main.tf \#
wires modules together with staging-sized inputs │ └── production/

------------------------------------------------------------------------

## Page 29

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 29 of 30 │ └── main.tf \# wires modules together with
production-sized inputs └── backend.tf \# remote state in Supabase Storage + DynamoDB
lock table Suggested Third-Party Integrations Capability Suggested
service Notes Email delivery AWS  Already in the core stack; scales
with no architecture change  messaging  Business
Platform (via Meta or a BSP like Twilio/Gupshup) A Business Solution
Provider is faster to integrate than Meta's direct API for an early
timeline Resume parsing Affinda, Rchilli, or an LLM-based extraction
pipeline Skip initially --- manual tagging is fine until volume
justifies the integration cost Calendar sync (interview scheduling)
Google Calendar / Microsoft Graph API High-value but defer until the
core ATS workflow is stable --- timezone and recurring- event edge cases
are deceptively time- consuming Error tracking Sentry Cheap to add
early; recommend wiring in during Phase 0 of the roadmap, not deferred
Risks, Trade-Offs & Architectural Decisions --- Summary Decision
Trade-off accepted Mitigation Modular monolith over microservices All
modules scale together; a spike in one module's load affects the whole
process Clean Architecture boundaries mean any module can be extracted
into its own service later without a rewrite --- only the
Infrastructure-layer wiring changes No Terraform initially Manual AWS
console setup risks configuration drift between environments A written,
versioned runbook documenting every manual step; adopt Terraform at the
documented trigger before drift becomes costly Single MonsterASP.NET instance,
single- AZ RDS No automatic failover; a brief outage during an AZ
failure or instance issue is possible Automated daily backups +
point-in-time recovery keep data loss risk low even though uptime isn't
HA; acceptable for the platform's current criticality Offset pagination
instead of cursor-based Performance degrades on very deep pagination
(page 5000+) and on tables with frequent inserts Not a real-world
concern at recruitment- platform list sizes (jobs, candidates) for years
of growth; revisit only if a specific list view's data volume proves
otherwise Hangfire-on-Postgres for background jobs Background job load
adds read/write pressure to the primary database Acceptable until job
volume is large enough to warrant the trigger in the Background
Processing section; monitor job table size and processing latency as the
leading indicator

------------------------------------------------------------------------

## Page 30

RecruitPro --- Solution Architecture Blueprint Coventine Digital Private
Limited 30 of 30 Closing Notes This blueprint translates Coventine's
existing recruitment domain (the converted 41-table schema, the
recruiter/candidate/application/interview workflow already modeled) into
a modern PostgreSQL and ASP.NET Core 8 shape without discarding that
work or introducing multi-tenant complexity the organization does not
need --- the schema migration in the Domain Model section is additive
(audit columns, row-versioning) rather than a rewrite. The single
highest-leverage next step is Phase 0 of the roadmap: stand up the
solution structure, the identity schema, and JWT authentication
end-to-end before building out recruitment features. Everything
downstream --- every command, every query, every API endpoint ---
depends on authentication working correctly first. Because this is a
single-organization platform, Coventine retains full control over how
quickly it scales: nothing in Part 2 needs to be adopted ahead of an
actual, observed trigger, which keeps both delivery timeline and AWS
spend aligned with real usage rather than speculative future
requirements.
