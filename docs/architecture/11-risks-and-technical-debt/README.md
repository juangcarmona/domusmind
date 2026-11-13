---
title: Risks and Technical Debt
arc42-section: "11"
description: Prioritized architectural risks and technical debt, with evidence and the mitigation or decision each needs.
---

# Risks and Technical Debt

Entries are ordered by priority. **Debt** is an observed condition in the repository; **risk** is a credible consequence that has not yet materialised. Each entry was checked against the implementation on 2026-10-01. Remove an entry when its condition no longer holds.

## Architecture and security

The approved product requires production OIDC sign-in and authorization through a
bound provider identity. The current implementation is listed below as a delivery gap.

<!-- pdac:cite id="CON-PRODUCTION-OIDC-SIGN-IN" digest="sha256:eb9059ccb8ca63b54036bb940fc2b5c472e06357b2e2084a9ad7900c88b490a3" -->

| Risk or debt | Architectural impact | Evidence | Priority | Mitigation or decision |
| --- | --- | --- | --- | --- |
| Gap: production still uses local password sign-in and browser-held tokens, while the approved product requires OIDC with operator-restricted setup and invitation-based identity binding | The current auth and setup paths cannot meet the approved production access contract | [Auth runtime](../06-runtime-view/README.md), [auth infrastructure](../../../src/backend/DomusMind.Infrastructure/Auth/), [setup features](../../../src/backend/DomusMind.Application/Features/Setup/) | High | Deliver the OIDC BFF, first-administrator claim, invitation lifecycle and access revocation as a separately verified change |
| Debt: only the Shopping List and Chat-to-Inbox paths currently have registered domain-event reactions; most persisted events do not drive cross-module behavior | The mediator and event contracts support reactions, but most cross-module behaviors still need a focused handler before they can use the event rule | [`CreateShoppingListFromMealPlanEventHandler.cs`](../../../src/backend/DomusMind.Application/Features/Lists/CreateShoppingListFromMealPlan/CreateShoppingListFromMealPlanEventHandler.cs), [`StageAssistantProposalHandler.cs`](../../../src/backend/DomusMind.Application/Features/Inbox/FromAssistant/StageAssistantProposalHandler.cs), [`DomainEventDispatcher.cs`](../../../src/backend/DomusMind.Infrastructure/Messaging/DomainEventDispatcher.cs) | Medium | Add a handler when a product behavior requires a cross-module reaction |
| Debt: the event log is append-only by application convention only; the database does not prevent direct updates or deletes | Direct SQL or another writer can alter or remove historical facts | [`EventLogEntryConfiguration.cs`](../../../src/backend/DomusMind.Infrastructure/Persistence/Configurations/EventLog/EventLogEntryConfiguration.cs), [`EventLogWriter.cs`](../../../src/backend/DomusMind.Infrastructure/Events/EventLogWriter.cs) | Medium | Add database-level append-only enforcement if the threat model requires protection from privileged writers |
| Debt: shared lists now reject overlapping writes through an aggregate version and HTTP 409, but other aggregate types still use last-writer-wins | Concurrent edits to plans and tasks can still overwrite one another | [`ListConfiguration.cs`](../../../src/backend/DomusMind.Infrastructure/Persistence/Configurations/Lists/ListConfiguration.cs), [`Program.cs`](../../../src/backend/DomusMind.Api/Program.cs) | Medium | Add concurrency tokens to other aggregates if their product behavior requires conflict detection |
| Debt: the web client refreshes its access token only once at load and never on a 401 | Long-lived sessions fail mid-use instead of renewing | [`src/web/app/src/api`](../../../src/web/app/src/api/), [`src/web/app/src/auth`](../../../src/web/app/src/auth/) | Medium | Retry once through the refresh-token endpoint on a 401 |
| Debt: Outlook ingestion does not recover when Graph rejects a delta token; delta state resets only when the sync horizon changes | A rejected cursor stops a feed from syncing until the member changes configuration | [`BackgroundJobs/Calendar`](../../../src/backend/DomusMind.Infrastructure/BackgroundJobs/Calendar/), [`Integrations/Calendar`](../../../src/backend/DomusMind.Infrastructure/Integrations/Calendar/) | Medium | Reset the cursor and run a full window sync on a rejected delta token |
| Debt: the MSAL token cache is stored in a field named `EncryptedRefreshToken` | Misleading name for a secret-bearing column invites wrong handling | [`ExternalCalendarProviderAccount.cs`](../../../src/backend/DomusMind.Application/Abstractions/Integrations/Calendar/ExternalCalendarProviderAccount.cs) | Low | Rename through a migration |
| Debt: handlers call `DateTime.UtcNow` directly (54 files); there is no clock abstraction | Time-dependent rules (Agenda windows, routines, sync horizons) are hard to test deterministically | `grep -rl DateTime.UtcNow src/backend` | Low | Introduce a clock service in Infrastructure and inject it |
| Debt: the domain and client lack behaviours the product model requires (see [Gaps against the product model](#gaps-against-the-product-model)) | The domain is the stable centre; missing invariants and operations surface as wrong behaviour, not as API gaps | Domain sources linked below | Medium | Implement each through a slice in the owning module, governed by the cited artifact |
| Debt: errors are mapped per controller to ad-hoc `{ error }` bodies, unhandled domain `InvalidOperationException`s become 500, and the global handler emits a custom JSON shape rather than problem details | API error contract is inconsistent across modules; clients cannot rely on codes or status | [`Program.cs`](../../../src/backend/DomusMind.Api/Program.cs), [`ListsController.cs`](../../../src/backend/DomusMind.Api/Controllers/ListsController.cs), [`MealPlansController.cs`](../../../src/backend/DomusMind.Api/Controllers/MealPlansController.cs) | Medium | Define one error model in Contracts and map context exceptions centrally |
| Risk: all modules, including authentication, share one PostgreSQL database and one `DbContext`; read models and command handlers in one context read other contexts' aggregates directly (for example Calendar and Lists handlers reading Family members) | Module boundaries are held by convention only; extracting a module or isolating auth data needs schema work | [`DomusMindDbContext.cs`](../../../src/backend/DomusMind.Infrastructure/Persistence/DomusMindDbContext.cs), [`GetWeeklyGridQueryHandler.cs`](../../../src/backend/DomusMind.Application/Features/Family/GetWeeklyGrid/GetWeeklyGridQueryHandler.cs) | Low | Accept for the monolith; revisit if a module must be extracted |
| Debt: application slices have no validators (`IValidator<T>` is defined but unimplemented); input rules live only in domain value objects and handlers | Slice convention and implementation disagree; invalid input fails late, often as a domain exception | [`IValidator.cs`](../../../src/backend/DomusMind.Application/Abstractions/Validation/IValidator.cs) | Low | Either implement validators where rules belong to the application layer or drop the abstraction |
| Debt: `DomusMind.Contracts` references `DomusMind.Domain` although no contract uses a domain type, and contracts live in `DomusMind.Contracts.*` namespaces where historical guidance used `Model.*` | Contracts can start depending on domain types unnoticed; guidance and code disagree | [`DomusMind.Contracts.csproj`](../../../src/backend/DomusMind.Contracts/DomusMind.Contracts.csproj), [historical guidance](https://github.com/juangcarmona/domusmind/blob/617876cbc123e07f0bf4a1b300df1c9816b7afc7/AGENTS.md) | Low | Remove the project reference; align the guidance with the namespaces in use |
| Debt: the Tasks domain still models a routine kind (`Cue`/`Scheduled`) that the product model removed | Dead concept in the domain and persistence | [`RoutineKind.cs`](../../../src/backend/DomusMind.Domain/Tasks/Enums/RoutineKind.cs), [`CreateRoutineCommandHandler.cs`](../../../src/backend/DomusMind.Application/Features/Tasks/CreateRoutine/CreateRoutineCommandHandler.cs) | Low | Remove through a Tasks slice change |

## Deployment and operations

| Risk or debt | Architectural impact | Evidence | Priority | Mitigation or decision |
| --- | --- | --- | --- | --- |
| Debt: Outlook ingestion needs `MicrosoftGraph` settings (client id, client secret, tenant id, token encryption key) that neither `deploy/docker-compose.yml` nor `deploy/.env.example` passes through | Self-hosted instances cannot connect Outlook and get HTTP 503 "not configured" | [`deploy/docker-compose.yml`](../../../deploy/docker-compose.yml), [`MicrosoftGraphCalendarAuthService.cs`](../../../src/backend/DomusMind.Infrastructure/Integrations/Calendar/Microsoft/MicrosoftGraphCalendarAuthService.cs) | Medium | Add the settings to the Compose environment and `.env.example` |
| Debt: release documentation disagrees with the release pipeline: `deploy/README.md` lists `latest` and `1.0` tags that `release.yml` never produces, and `.env.example` defaults `VERSION=edge`, mentions an alpha prerelease the release tag pattern rejects and omits `JWT_AUDIENCE` | Self-hosters follow instructions that do not match the published images or configuration | [`deploy/README.md`](../../../deploy/README.md), [`release.yml`](../../../.github/workflows/release.yml) | Low | Align the README and `.env.example` with the release workflow |
| Debt: the `Microsoft.Graph` SDK package is referenced but unused; Graph is called through raw REST | Unneeded dependency surface and update noise | [`DomusMind.Infrastructure.csproj`](../../../src/backend/DomusMind.Infrastructure/DomusMind.Infrastructure.csproj) | Low | Remove the package reference |
| Debt: `src/web/app/nginx.conf.template` is orphaned; the API serves the built web app from `wwwroot` | Dead deployment artefact suggests a topology that is not used | [`nginx.conf.template`](../../../src/web/app/nginx.conf.template), [`Program.cs`](../../../src/backend/DomusMind.Api/Program.cs) | Low | Delete it |

## Product alignment and delivery

| Risk or debt | Architectural impact | Evidence | Priority | Mitigation or decision |
| --- | --- | --- | --- | --- |
| Debt: OpenSpec specs lag the accepted product model; ProductShape reports 22 drift warnings across 4 specs (22 `pdac-drift` markers under `openspec/specs/`) | Implementers reading OpenSpec build against superseded behaviour | `npx -y @prodshape/cli@0.22.0 drift openspec` | High | Refresh the drifted specs through OpenSpec changes |
| Debt: the `recipe-management` OpenSpec change is still active although all 49 of its tasks are done and recipe management is delivered | OpenSpec's current specs do not include delivered behaviour | [`openspec/changes/recipe-management/tasks.md`](../../../openspec/changes/recipe-management/tasks.md), [`RecipesController.cs`](../../../src/backend/DomusMind.Api/Controllers/RecipesController.cs) | Medium | Archive the change into `openspec/specs/` |
| Debt: required status checks on `main` are only `backend build`, `webapp build`, `Validate Astro site` and the two CodeQL analyses; `product model` (product-ci) and `architecture docs` (docs-ci) are not required | Product-model validity, OpenSpec citations and architecture citations can regress on `main` | `gh api repos/juangcarmona/domusmind/branches/main/protection/required_status_checks`, [`product-ci.yml`](../../../.github/workflows/product-ci.yml), [`docs-ci.yml`](../../../.github/workflows/docs-ci.yml) | Medium | Add both checks to branch protection |
| Debt: list changes are not pushed to other open clients | A product quality requirement is unmet (see [10 Quality requirements](../10-quality-requirements/README.md#list-changes-reach-the-whole-household)) | [`package.json`](../../../src/web/app/package.json), [`Program.cs`](../../../src/backend/DomusMind.Api/Program.cs) | Medium | Decide on a push or polling mechanism; record it as an ADR |
| Risk: backend application tests run on the EF Core in-memory provider and nothing tests the composed container image end to end | PostgreSQL-specific and transactional behaviour, including the two-`SaveChanges` event-log writes, is untested; deployment regressions surface only in production | [`tests/backend/DomusMind.Application.Tests`](../../../tests/backend/DomusMind.Application.Tests/), [`backend-ci.yml`](../../../.github/workflows/backend-ci.yml) | Medium | Run integration tests against PostgreSQL (for example in a container) and smoke-test the built image in CI |
| Risk: web behaviour, including mobile parity, has a small test baseline (two web unit test files and one browser journey covering onboarding, Agenda-to-Lists navigation, Inbox proposal approval and rejection, explicit Chat assistant use, meal planning, and shared list reuse; only selected flows run at phone width) | Regressions in the single client reach every device; most surface interactions and phone layouts remain uncovered | [`src/web/app/src`](../../../src/web/app/src/), [`household-onboarding.e2e.ts`](../../../src/web/app/e2e/household-onboarding.e2e.ts), [`docs/engineering-e2e.md`](../../engineering-e2e.md) | Medium | Expand model-linked browser journeys to the remaining primary surfaces and phone interactions |
| Risk: local development orchestration exists only as the Aspire AppHost (Postgres, API, Vite app); the Compose stack under `deploy/` runs published images only | Contributors without the Aspire workload and a container runtime cannot run the system locally | [`AppHost.cs`](../../../src/backend/DomusMind.AppHost/AppHost.cs), [`deploy/docker-compose.yml`](../../../deploy/docker-compose.yml) | Low | Document the AppHost prerequisites, or add a source-build Compose profile |

## Gaps against the product model

Each gap below is a missing operation, invariant or client call. The product artifact owns the expected behaviour.

Lists have no privacy state; every list is visible to the whole family ([`SharedList.cs`](../../../src/backend/DomusMind.Domain/Lists/SharedList.cs)).

<!-- pdac:cite id="FR-LISTS-PRIVATE-LIST" digest="sha256:e657bc7ca8eaf2f90ba10a7dfcf6535f2d8b9c07d9e0f3397e3dac1b053f72ff" -->

The routine kind still required by the create and update routine handlers has no counterpart in the product's routine definition.

<!-- pdac:cite id="FR-TASKS-CREATE-ROUTINE" digest="sha256:a3a40147d3e2cc80d9bc822f878d5ef81d91f9f3a3e77255921ec0cd42ce5ae6" -->

Shopping-list derivation is delivered, but by a command that writes into the Lists module directly rather than through a Lists reaction.

<!-- pdac:cite id="FR-MEALS-SHOPPING-LIST-DERIVATION" digest="sha256:b80be8ac1ed40f2e58ff5d426c5fcdff0c93c3826ffc5907d595f6349287635b" -->

Recipe management is delivered while its OpenSpec change remains active.

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:1340dae095e80b3c9ea2f47bb1780af2e6ae2acbd28baeb5750cd532bd02d34d" -->

External calendars refresh only through the background worker and manual sync; the web app never triggers the catch-up sync endpoint on sign-in or when the Agenda opens ([`ExternalCalendarRefreshWorker.cs`](../../../src/backend/DomusMind.Infrastructure/BackgroundJobs/Calendar/ExternalCalendarRefreshWorker.cs), [`externalCalendarApi.ts`](../../../src/web/app/src/api/externalCalendarApi.ts)).

<!-- pdac:cite id="FR-CALENDAR-BACKGROUND-REFRESH" digest="sha256:6395fc275a0ad0d787e23defa29afc83213e042d2ecf161add0059c731822988" -->
