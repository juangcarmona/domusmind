---
title: Crosscutting Concepts
arc42-section: "08"
description: The shared mechanisms every DomusMind bounded context follows, covering the domain model, mediator, slices, mapping, persistence, events, identifiers, security, errors, validation, time, localization and configuration.
---

# Crosscutting Concepts

Each concept below applies to all six bounded contexts (Family, Responsibilities, Calendar, Tasks, Lists, Meal Planning) in the backend projects described in the [Building Block View](05-building-block-view.md). Where the code does not yet fully realize a concept, the gap is stated here and tracked in [Risks and Technical Debt](11-risks-and-technical-debt.md).

## Domain Model Conventions

### Scope

`DomusMind.Domain`, all contexts. Authentication users, refresh tokens, family access grants, the supported-language catalog and system initialization are infrastructure records, not aggregates.

### Mechanism

- Aggregate roots derive from `AggregateRoot<TId>`, which derives from `Entity<TId>` (identity equality). Value objects derive from `ValueObject` or are records. All of them live in [`Domain/Abstractions`](../../src/backend/DomusMind.Domain/Abstractions).
- State changes go through methods on the root. The root checks invariants and records domain events with `RaiseDomainEvent`. Value-object factories such as `FamilyName.Create` reject invalid values.
- **One command modifies one aggregate.** A command handler loads or creates one aggregate, calls it, and commits. Work that spans aggregates is separate commands. A handler may also write infrastructure records in the same commit, for example `CreateFamily` granting the creator family access. `RequestShoppingList` is a known exception: it creates a `SharedList` (Lists) and updates the `MealPlan` in the same command.
- Aggregates in other contexts refer to people and households by identifier only and never by object reference, so the Family context remains the only owner of household structure.
<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:661a8bfff912e490d64710e11b333119cb4143fdf597ca2672523e0bd7fae034" -->
- The domain project references no other project and no framework package.

### Evidence

[`DomusMind.Domain.csproj`](../../src/backend/DomusMind.Domain/DomusMind.Domain.csproj) has no references. The per-context aggregate folders are under [`src/backend/DomusMind.Domain`](../../src/backend/DomusMind.Domain). Invariant tests are in [`tests/backend/DomusMind.Domain.Tests`](../../tests/backend/DomusMind.Domain.Tests).

## Internal Application Mediator

### Scope

Every API request that reaches application behavior. The decision is recorded in [ADR-0001](../adr/0001-use-an-internal-application-mediator.md).

### Mechanism

Controllers call `ICommandDispatcher` or `IQueryDispatcher` with an `ICommand<TResponse>` or `IQuery<TResponse>`. The dispatchers in [`Infrastructure/Messaging`](../../src/backend/DomusMind.Infrastructure/Messaging) resolve the closed handler type from DI and invoke it by reflection. There is no pipeline or behavior chain, so validation, authorization and event logging happen inside each handler. Every handler is registered by hand in [`Application/DependencyInjection`](../../src/backend/DomusMind.Application/DependencyInjection), with no assembly scanning. A missing registration shows up only at runtime as a `HandlerResolutionException`, which the global exception handler returns as HTTP 500 with code `handler_resolution_failed`. Handlers never call other handlers.

### Evidence

[`Application/Abstractions/Messaging`](../../src/backend/DomusMind.Application/Abstractions/Messaging), [`CommandDispatcher.cs`](../../src/backend/DomusMind.Infrastructure/Messaging/CommandDispatcher.cs), [`Program.cs`](../../src/backend/DomusMind.Api/Program.cs).

## Vertical Slices and Thin Controllers

### Scope

All capabilities in `DomusMind.Application/Features/<Context>/<Capability>` and all controllers in `DomusMind.Api/Controllers`. The slice layout is described in the [Building Block View](05-building-block-view.md). The controller style is recorded in [ADR-0005](../adr/0005-expose-a-rest-api-through-aspnet-core-controllers.md).

### Mechanism

- A slice is named after the capability in domain language (`ScheduleEvent`, `AssignPrimaryOwner`), not after a storage operation such as `CreateEventRecord`. A slice holds its command or query and its handler. There is no shared service layer between slices and no generic repository. The few repositories that exist (`AuthUserRepository`, `SystemInitializationRepository`) are specific to authentication and setup infrastructure.
- Commands and queries are immutable `sealed record`s that carry only the inputs the capability needs, plus the requesting user's id from `ICurrentUser`.
- Handlers never call other handlers directly. The only handler that dispatches is the orchestration command `SyncMemberExternalCalendarConnections`, which fans out one aggregate-scoped `SyncExternalCalendarConnection` command per connection through `ICommandDispatcher`.
- Controllers are transport adapters. They bind the request model, build the command or query, dispatch it, and translate the result or the context exception into an HTTP response. They contain no domain or persistence logic.
- No slice imports another context's application code. Every `Features` namespace import stays inside its own context. However, all contexts share one `DbContext`, so handlers read other contexts' aggregates directly, for example Calendar and Lists handlers checking `Family` members. Cross-context reactions are meant to go through domain events (see [Domain Events and the Event Log](#domain-events-and-the-event-log)), but today they run as explicit commands.

### Evidence

[`Application/Features`](../../src/backend/DomusMind.Application/Features), [`FamiliesController.cs`](../../src/backend/DomusMind.Api/Controllers/FamiliesController.cs), [`SyncMemberExternalCalendarConnectionsCommandHandler.cs`](../../src/backend/DomusMind.Application/Features/Calendar/SyncMemberExternalCalendarConnections/SyncMemberExternalCalendarConnectionsCommandHandler.cs).

## Testing Strategy

### Scope

All backend projects, with one test project for each under [`tests/backend`](../../tests/backend). Every test project runs in `backend build` (see [DevOps](../devops.md#ci-workflows)).

### Mechanism

- **Domain tests** (`DomusMind.Domain.Tests`, organised by context) exercise aggregates directly. They assert state changes, rejected invariants and the domain events raised.
- **Application tests** (`DomusMind.Application.Tests`) run each handler against `DomusMindDbContext` on the EF Core in-memory provider (not PostgreSQL), testing state change, authorization denial, error codes and event-log writes. Separate folders cover the dispatchers and the event log.
- **Infrastructure tests** cover authentication services and the calendar integration. **API tests** cover controllers.
- No end-to-end test runs against the composed container image.

### Evidence

[`tests/backend`](../../tests/backend); domain and application tests assert on the raised `DomainEvents`.

## API Contracts and Explicit Mapping

### Scope

Every HTTP request and response body. Recorded in [ADR-0006](../adr/0006-map-explicitly-without-automapper.md).

### Mechanism

- API models live in `DomusMind.Contracts.<Context>` namespaces, grouped by context. No `Model.*` namespace exists. Request models end in `Request`, response models end in `Response`, and composite read items such as timeline entries may drop the suffix. The `Dto` suffix is not used. Domain entities are never serialized.
- Handlers return Contracts response types directly. Mapping is hand-written, either as a constructor call in the handler or as a LINQ `Select` into the response type. No mapping library is referenced.
- Contract evolution is additive: new optional fields may be added, existing fields keep their meaning, and clients must tolerate unknown fields. URL versioning is not used.

### Evidence

[`src/backend/DomusMind.Contracts`](../../src/backend/DomusMind.Contracts). There is no AutoMapper package in any `*.csproj`. `DomusMind.Contracts.csproj` references `DomusMind.Domain` even though no contract type uses it.

## Persistence and Read Projections

### Scope

All state in PostgreSQL through one EF Core context. Recorded in [ADR-0007](../adr/0007-use-ef-core-directly-without-generic-repositories.md).

### Mechanism

- A single `DomusMindDbContext` and a single database serve every context, including the authentication, family-access and system tables. The context is exposed to the application as `IDomusMindDbContext` (`Set<T>()`, `SaveChangesAsync`). There are no repositories over aggregates. Handlers query and add through `Set<T>()`.
- Mapping lives in one `IEntityTypeConfiguration` per type under [`Persistence/Configurations/<Context>`](../../src/backend/DomusMind.Infrastructure/Persistence/Configurations), applied from the assembly. Tables and columns use snake_case. Strongly typed IDs and value objects are stored through value conversions, and children are owned or mapped inside their aggregate's configuration.
- Reads use `AsNoTracking()` and project straight into response types. No separate read-model store exists.
- Migrations live in [`Persistence/Migrations`](../../src/backend/DomusMind.Infrastructure/Persistence/Migrations) and are applied at application startup (see [Deployment View](07-deployment-view.md)).
- No optimistic concurrency token is configured on any aggregate, so concurrent writes to the same aggregate are last-writer-wins.

### Evidence

[`DomusMindDbContext.cs`](../../src/backend/DomusMind.Infrastructure/Persistence/DomusMindDbContext.cs), [`IDomusMindDbContext.cs`](../../src/backend/DomusMind.Application/Abstractions/Persistence/IDomusMindDbContext.cs).

## Domain Events and the Event Log

### Scope

All command handlers that change an aggregate. Recorded in [ADR-0008](../adr/0008-collaborate-across-modules-through-persisted-domain-events.md). The runtime sequence is shown in the [Runtime View](06-runtime-view.md).

### Mechanism

- Domain events implement `IDomainEvent` (`OccurredAtUtc`) and are buffered on the aggregate root.
- After changing the aggregate, the handler passes `aggregate.DomainEvents` to `IEventLogWriter.WriteAsync` and then clears them. `EventLogWriter` adds one append-only `event_log` row per event, with the type name, the inferred module and the JSON payload, and calls `SaveChangesAsync`. In most handlers that one call also flushes the tracked aggregate changes, so the aggregate state and its events commit together. The external-calendar handlers, the Meal Planning handlers and the two Family handlers that create sign-in accounts (`LinkMemberAccount`, `ProvisionMemberAccess`) call `SaveChangesAsync` before writing the log, so their state and events commit in two separate transactions. The log is append-only by convention: nothing in the database or the code prevents updates or deletes.
- Some event-log columns are reserved but not populated yet. `AggregateType` and `AggregateId` are written as `unknown`, `Version` is always 1, correlation and causation are null, and module inference recognises only Family, Responsibilities, Calendar and Tasks. The event id is a `Guid`.
- `IDomainEventDispatcher` and `IDomainEventHandler<T>` exist, but no handler implements them and no code calls the dispatcher. Events are therefore recorded for audit but drive no in-process reaction yet. Behavior that spans contexts runs as separate commands, for example `CreateLinkedListForEvent`.

### Evidence

[`AggregateRoot.cs`](../../src/backend/DomusMind.Domain/Abstractions/AggregateRoot.cs), [`EventLogWriter.cs`](../../src/backend/DomusMind.Infrastructure/Events/EventLogWriter.cs), [`EventLogEntryConfiguration.cs`](../../src/backend/DomusMind.Infrastructure/Persistence/Configurations/EventLog/EventLogEntryConfiguration.cs), [`DomainEventDispatcher.cs`](../../src/backend/DomusMind.Infrastructure/Messaging/DomainEventDispatcher.cs).

## Identifiers

### Scope

All aggregate and entity identities. Recorded in [ADR-0010](../adr/0010-use-guid-backed-strongly-typed-identifiers.md), which supersedes the never-implemented ULID rule of [ADR-0009](../adr/0009-use-ulid-based-strongly-typed-identifiers.md).

### Mechanism

- Each aggregate has its own strongly typed ID, a `readonly record struct` such as `FamilyId(Guid Value)` with `New()` and `From(...)`. IDs of different types cannot be mixed up at compile time.
- The format is `Guid`. `New()` calls `Guid.NewGuid()`, PostgreSQL stores the value as `uuid` through EF value conversions, Contracts type the IDs as `Guid`, and routes constrain IDs with `{id:guid}`.
- Most create commands generate the ID on the server. The Meal Planning create commands (`CreateMealPlan`, `CreateRecipe`, `CreateWeeklyTemplate`, `CopyMealPlanFromPreviousWeek`) take a GUID generated by the web app.
- IDs are immutable and never encode mutable data. They are part of the long-lived data and API contract.

### Evidence

[`FamilyId.cs`](../../src/backend/DomusMind.Domain/Family/FamilyId.cs), [`FamilyConfiguration.cs`](../../src/backend/DomusMind.Infrastructure/Persistence/Configurations/Family/FamilyConfiguration.cs).

## Authentication, Authorization and Identity Separation

### Scope

Every API request. Recorded in [ADR-0002](../adr/0002-keep-authentication-local-and-separate-from-member-identity.md).

### Mechanism

- **Authentication** is local to the monolith. `AuthUser` records in Infrastructure store the email and a PBKDF2 password hash. Login issues a JWT bearer access token signed with a symmetric key (`Jwt:SigningKey`, 60-minute default lifetime) and a refresh token stored server-side, which is rotated on each refresh, with the old token revoked. `[Authorize]` is declared per controller; there is no fallback policy. The anonymous endpoints are auth health, register, login, refresh and logout, setup, the language catalog and `GET /api/system/ping`, whose controller carries no `[Authorize]`; registration is therefore open to anyone who can reach the instance.
- **Identity separation.** The authentication `User` is distinct from the domain `Member`. A member may be linked to an `AuthUserId`, and the `user_family_access` table grants a user access to a family. No auth framework type reaches `DomusMind.Domain`. `ICurrentUser` hands the application only the user's ID, email and roles from the token claims.
- **Authorization is family-scoped and enforced in handlers.** Every family-scoped handler calls `IFamilyAuthorizationService.CanAccessFamilyAsync(userId, familyId)` before it acts and throws the context's `AccessDenied` code if the check fails. Handlers then apply the context's role rules themselves, for example manager-only member administration. The household is the isolation boundary for all data.
<!-- pdac:cite id="CON-PRODUCT-HOUSEHOLD-FIRST" digest="sha256:9ffd7fd5d31817890e9263cea8286b25102bd41092769a0cfe3e0765792808b3" -->
- **First-run bootstrap.** A `system_initialization` record gates the anonymous setup endpoint, which can succeed only once. An optional configuration-driven bootstrap admin is disabled by default and does nothing once the system is initialized.

### Evidence

[`AuthInfrastructureExtensions.cs`](../../src/backend/DomusMind.Infrastructure/Auth/AuthInfrastructureExtensions.cs), [`FamilyAuthorizationService.cs`](../../src/backend/DomusMind.Infrastructure/Auth/FamilyAuthorizationService.cs), [`RefreshTokenStore.cs`](../../src/backend/DomusMind.Infrastructure/Auth/RefreshTokenStore.cs), [`AuthSeedService.cs`](../../src/backend/DomusMind.Infrastructure/Auth/AuthSeedService.cs). Every handler performs the family check except the auth, setup and language handlers and the two that resolve or create the caller's own family.

## API Conventions and Error Model

### Scope

The REST surface of `DomusMind.Api`.

### Mechanism

- Routes are `api/<plural-resource>` and are nested under `families/{familyId}` or `members/{memberId}` only when ownership is explicit. Queries use `GET` and commands use `POST`, `PUT`, `PATCH` or `DELETE`. Swagger (Swashbuckle) documents every endpoint with a bearer security scheme and is enabled in all environments.
- **Errors do not use RFC 7807 problem details.** Each context defines one exception type with an error-code enum, for example `FamilyException` with `FamilyErrorCode`. Each controller maps codes to statuses: invalid input becomes 400, access denied 403, not found 404, and duplicates or invalid state 409. Calendar also uses 422, 502 and 503. Most error bodies are `{ "error": "<message>" }`, setup returns `{ "code", "message" }`, and unhandled exceptions return 500 with `{ "code", "message", "traceId" }`. Only model-binding failures produce ASP.NET's default `ValidationProblemDetails`. `DomusMind.Contracts` defines no shared error model.

### Evidence

[`Program.cs`](../../src/backend/DomusMind.Api/Program.cs), [`OpenApiAuthExtensions.cs`](../../src/backend/DomusMind.Api/OpenApi/OpenApiAuthExtensions.cs), [`Api/Controllers`](../../src/backend/DomusMind.Api/Controllers).

## Validation

### Scope

All commands.

### Mechanism

Validation has three layers: model binding under `[ApiController]`, shape checks at the start of each handler (which throw the context's `InvalidInput` code), and invariants in domain factories and aggregate methods. The `IValidator<T>` and `ValidationResult` abstractions exist in [`Application/Abstractions/Validation`](../../src/backend/DomusMind.Application/Abstractions/Validation), but nothing implements or calls them. There are no separate validator classes in the slices.

### Evidence

[`CreateFamilyCommandHandler.cs`](../../src/backend/DomusMind.Application/Features/Family/CreateFamily/CreateFamilyCommandHandler.cs).

## Time and Dates

### Scope

Domain events, audit fields, scheduling and expiry.

### Mechanism

Instants are UTC `DateTime` values and the property names end in `Utc`. Calendar dates and times of day travel as ISO strings (`yyyy-MM-dd`, `HH:mm`), and the internal `TemporalParser` in [`Application/Temporal`](../../src/backend/DomusMind.Application/Temporal) parses and round-trips them. `RepeatExpansion` expands recurrences for Calendar, Family and Tasks reads. Display formatting is left to the client. There is no clock abstraction: domain, application and infrastructure code read `DateTime.UtcNow` directly, so time-dependent behavior cannot be controlled in tests.

## Localization

### Scope

Web app text and household language preference.

### Mechanism

Interface text is localized in the client with `react-i18next`. Locale bundles live in [`src/web/app/src/i18n/locales`](../../src/web/app/src/i18n/locales) (en, de, es, fr, it, ja, zh). The server owns the `supported_languages` catalog, which `LanguageSeedService` seeds at startup and `GET /api/languages` serves anonymously. It also validates a family's primary language against that catalog. API error messages are English and are not localized.

## Configuration and Secrets

### Scope

Every runtime environment.

### Mechanism

Standard ASP.NET Core configuration with sections `ConnectionStrings:domusmind`, `Jwt`, `BootstrapAdmin`, `MicrosoftGraph` and `ExternalCalendarRefresh` (defaults in [`appsettings.json`](../../src/backend/DomusMind.Api/appsettings.json)). Options are bound and validated at startup. A missing JWT signing key or one under 32 characters, or an enabled bootstrap admin without credentials, stops the application from starting. In the self-hosted stack, values come from `.env` through `Section__Key` environment variables. In development, Aspire injects the connection string and user secrets supply the rest. Secrets are never committed.
