# AGENTS.md

Instructions for every AI agent working in this repository (Claude Code reads them through `CLAUDE.md`, GitHub Copilot directly). Path-scoped rules for Copilot live in `.github/instructions/`.

## Purpose

DomusMind is a real system with an existing design.
Preserve the documented architecture.
Do not redesign the domain.
Do not introduce generic boilerplate patterns that conflict with the docs.

## Working mode

Before changing code:
1. Read the relevant architecture and spec documents.
2. Identify the bounded context and slice involved.
3. Preserve layer boundaries.
4. Make the smallest safe change that keeps the solution coherent.
5. Prefer concrete code and file edits over theoretical advice.

## Source of truth

`docs/README.md` maps where each kind of truth lives. Read these first:

### Product
- `docs/product/model/`: the accepted product model (ProductShape). Validate with `npx @prodshape/cli@0.22.0 validate`; browse it with `npx @prodshape/cli@0.22.0 graph --format html`. It changes only through a Product Change under `docs/product/changes/`.
- `docs/design/`: UI guidance for the surfaces, subordinate to the model.

### Architecture
- `docs/architecture/`: arc42, twelve sections; start at `docs/architecture/README.md`.
  - Building blocks, modules and slices: `05-building-block-view.md`
  - Runtime flows (dispatcher, event log, sync, auth): `06-runtime-view.md`
  - Crosscutting rules (aggregates, mediator, mapping, persistence, events, IDs, auth, API conventions, testing): `08-crosscutting-concepts.md`
  - Known gaps between the docs, the model and the code: `11-risks-and-technical-debt.md`

### Decisions
- `docs/adr/`: architecture decision records. `docs/adr/README.md` lists every record with its status; `docs/architecture/09-architecture-decisions.md` maps the ones in effect to the views. Only `Accepted` records bind a change.
  - `0001-use-an-internal-application-mediator.md` (the mediator rule below)
  - `0002-keep-authentication-local-and-separate-from-member-identity.md`
  - `0010-use-guid-backed-strongly-typed-identifiers.md` (identifiers; supersedes the ULID rule of 0009)

### Behaviour specs
- `openspec/specs/*/spec.md`, bound to the product model by citations (`npx @prodshape/cli@0.22.0 citations verify --provider openspec`).

### Operations
- `docs/devops.md`: versioning, release, CI workflows and required checks.

## Mandatory architectural rules

- DomusMind is a domain-centric, API-first modular monolith.
- The domain model is the stable center.
- The API uses ASP.NET Core Controllers, not Minimal APIs.
- The API is REST-based and documented with Swagger/OpenAPI.
- API contracts use explicit models under `Model.*`.
- Domain entities must never be exposed directly through the API.
- Mapping is explicit. Do not introduce AutoMapper.
- EF Core is used directly. Do not introduce generic repositories.
- Read queries should prefer projection-based queries and `AsNoTracking()`.
- Commands and queries are explicit.
- One command modifies one aggregate only.
- Cross-module collaboration happens through domain events.
- Domain events are persisted in an append-only event log.
- Authentication is local to the modular monolith.
- Authentication identity and family/member domain identity are separate concepts.
- The domain must remain framework-agnostic.
- Do not push infrastructure concerns into the domain.
- Do not collapse the modular monolith into a single project.
- Do not redesign into generic Clean Architecture boilerplate.

## Current backend structure

Backend root:
- `src/backend/DomusMind.Api`
- `src/backend/DomusMind.Application`
- `src/backend/DomusMind.Contracts`
- `src/backend/DomusMind.Domain`
- `src/backend/DomusMind.Infrastructure`

Tests live under:
- `tests/backend/*`

## Layer responsibilities

### DomusMind.Domain
Contains:
- aggregates
- entities
- value objects
- domain events
- domain rules

Must not contain:
- EF Core
- ASP.NET Core
- JWT/auth framework code
- infrastructure services
- API contracts

### DomusMind.Application
Contains:
- commands
- queries
- handlers
- validators
- dispatcher abstractions
- orchestration across domain + persistence boundaries

### DomusMind.Contracts
Contains:
- API request models
- API response models
- shared API error models

### DomusMind.Infrastructure
Contains:
- EF Core DbContext
- persistence mappings
- migrations
- event log persistence
- auth implementation
- current-user accessors
- clock/system services
- dispatcher implementations

### DomusMind.Api
Contains:
- controllers
- HTTP transport mapping
- auth/swagger wiring
- exception and problem-details mapping

## Slice rules

Represent capabilities as vertical slices under bounded contexts.

Examples:
- `Features/Family/CreateFamily`
- `Features/Family/AddMember`
- `Features/Responsibilities/AssignPrimaryOwner`
- `Features/Calendar/ScheduleEvent`
- `Features/Tasks/CreateTask`

Each slice should contain only what it needs, typically:
- command or query
- validator
- handler
- response or mapping when needed

Controllers stay thin.
Handlers do not call other handlers.
Cross-module reactions happen through domain events.

## Internal mediator rule

Use the internal application mediator from ADR-001.
Do not add MediatR or similar frameworks unless the user explicitly changes the ADR.

Core contracts:
- `ICommand<TResponse>`
- `IQuery<TResponse>`
- `ICommandHandler<TCommand, TResponse>`
- `IQueryHandler<TQuery, TResponse>`
- `IDomainEvent`
- `IDomainEventHandler<TEvent>`
- `ICommandDispatcher`
- `IQueryDispatcher`
- `IDomainEventDispatcher`

## Execution discipline

When asked to implement something:
1. Locate the relevant docs.
2. State which docs govern the change.
3. Identify the target bounded context and slice.
4. Preserve existing names and concepts from the ubiquitous language.
5. Make minimal edits.
6. Keep code compilable.
7. Do not invent missing business rules when the docs are silent; stop and report the gap.

## Output expectations

When proposing work:
- be concrete
- name exact files
- show minimal code
- avoid broad refactors
- avoid placeholder abstractions not required by the docs

When uncertain:
- read more docs first
- report the uncertainty clearly
- do not guess domain behavior

## Working rules

- Prefer extending existing code over introducing new abstractions.
- Keep changes small and localized; keep files, methods and components small and single-purpose (target under 300 lines per file).
- Avoid generic "service", "manager" or "helper" types unless the pattern already exists.
- Web app: split components that mix data, state and rendering; reuse existing API and state patterns; do not duplicate forms or logic across features.
- Do not invent terminology: use the product model's terms (`docs/product/model/domain/terms/`). When behaviour changes, update the owning document (product model through a Product Change, OpenSpec spec, or architecture section) rather than adding a parallel explanation.

## Validation

Before reporting a task as done:
- Backend: `dotnet build DomusMind.slnx --configuration Release` succeeds and `dotnet test DomusMind.slnx` passes.
- Web app or public site: `npm run build` succeeds in the affected project, and its lint and tests pass.
- Product or docs changes: `npx -y @prodshape/cli@0.22.0 validate`, `citations verify --provider openspec` and `citations verify docs/architecture` pass.
- Never call a task finished when its checks have not been run and seen passing.

## Database migrations

```bash
dotnet ef migrations add <MigrationName> --project src/backend/DomusMind.Infrastructure --startup-project src/backend/DomusMind.Api --output-dir Persistence/Migrations
```

Under Aspire, make sure the database is running before generating a migration.
