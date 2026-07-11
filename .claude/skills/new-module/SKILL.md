---
name: new-module
description: Scaffold a new Clean Architecture module/feature slice for RecruitPro (Domain entity, Application command/query + handler, Infrastructure EF config, Api controller endpoint), following the layering and conventions in CLAUDE.md and the architecture blueprint. Use when adding a new bounded-context entity or command/query (e.g. "add a new module for Interviews", "scaffold the Offer entity").
---

Scaffold a new feature slice across RecruitPro's four layers, following the conventions in
`CLAUDE.md` and `RecruitPro-SaaS-Architecture-Blueprint-Updated.md`. Ask the user which
bounded context this belongs to (Identity & Access / Recruitment / ) if it
isn't obvious from the request.

Build in this order, one layer at a time, respecting the one-way dependency rule
(Domain → Application → Infrastructure → Api):

1. **Domain** (`RecruitPro.Domain/<Context>/`)
   - Entity in `Entities/`, inheriting the shared `BaseEntity` (`Id`, `CreatedAt`, `CreatedBy`,
     `IsDeleted`, `RowVersion`).
   - Value objects in `ValueObjects/` if the entity has any (e.g. `SalaryRange`).
   - Domain events in `Events/` for anything other modules need to react to.
   - Put invariants as methods on the entity itself (e.g. `Publish()` throwing a
     `DomainException` subclass if a required field is missing) — never let an Application
     handler mutate state that violates an invariant.

2. **Application** (`RecruitPro.Application/<Context>/<Feature>/`)
   - A `Command` or `Query` record implementing `IRequest<Result<TDto>>`.
   - A matching `Handler` class that loads via `IApplicationDbContext`, calls the domain
     method, and calls `SaveChangesAsync`.
   - A `FluentValidation` validator for the command/query if it takes user input — this runs
     automatically via the existing `ValidationBehavior` pipeline behavior, no wiring needed.
   - Do not add try/catch for validation — that's the pipeline behavior's job.

3. **Infrastructure** (`RecruitPro.Infrastructure/Persistence/Configurations/`)
   - An `IEntityTypeConfiguration<T>` for the new entity: map `RowVersion` as a concurrency
     token, add the `IsDeleted` global query filter, add indexes per the blueprint's indexing
     strategy (foreign keys, common filter columns, partial `WHERE is_deleted = false` indexes
     on high-traffic tables).
   - Generate the EF Core migration (`dotnet ef migrations add <Name>`) once the configuration
     is in place — do not hand-write migration SQL.

4. **Api** (`RecruitPro.Api/Controllers/`)
   - A controller action that dispatches the command/query through MediatR and maps the
     `Result<TDto>` to the standard response envelope (`{ success, data, error, meta }`).
   - Add `[RequirePermission("<Context>.<Entity>.<Action>")]` — permission string, not a role
     name. Ask the user what the permission string should be if it isn't obvious.

After scaffolding, remind the user to add a unit test for the domain invariant and a handler
test with mocked dependencies (xUnit + FluentAssertions + NSubstitute), per the blueprint's
testing strategy — but don't write the tests unprompted unless asked.
