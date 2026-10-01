---
name: keep-authentication-local-and-separate-from-member-identity
description: Implement built-in local authentication as an internal module of the modular monolith, keeping the authenticable User separate from the household Member.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [backend, security, identity]
---

# ADR-0002: Keep authentication local and separate from member identity

## Context

DomusMind is designed to be local-first, self-hostable, API-first and a modular monolith. It needs authentication for household access, administrative actions, secure API use, and invitation and membership flows. Identity management is not DomusMind's core business domain: the system models household operations, not enterprise identity and access management.

Self-hosted families need an installation that is simple to run and has few infrastructure dependencies, while the design should keep a path open to external identity integration later. Authentication could live inside the main backend, in a separate identity service, or in an external identity provider.

This record migrates the legacy `ADR-002 - Authentication and Identity Strategy`, which was accepted without a recorded date; the date above is the legacy file's first commit.

## Decision

We will implement built-in local authentication as an internal module of the modular monolith, in the same deployable unit as the main API, and not as a separate identity service in V1.

The authentication module stays logically isolated, with its own models, persistence area and application flows, and its endpoints are exposed by the main API. It is an internal module, not a bounded context of the household domain. The authenticable system identity (`User`) and the household domain entity (`Member`) are separate models and must not be merged. The rest of the system depends on an abstraction of the current authenticated user rather than on the authentication implementation, so that OIDC or federation can be added later without changing the household domain. This option gives the best balance of security, simplicity and delivery speed for V1.

## Consequences

- **Positive:** Simpler deployment and a better self-hosted experience, with fewer runtime dependencies. Faster implementation and easier debugging and tracing. Aligned with the modular monolith.
- **Negative:** DomusMind owns its password and session security implementation. Integrating an external identity provider later requires additional work, and advanced identity features are deferred.
- **Neutral:** Checked against the code on 2026-10-01, authentication lives in the backend's auth infrastructure and application features, issues JWT access tokens with refresh tokens, and persists users and refresh tokens in their own tables (`auth_users`, `refresh_tokens`). Two details differ from the legacy wording: the persistence area is separate tables in the shared `DomusMindDbContext` and database rather than a separate store, and the current-user abstraction is named `ICurrentUser`, not `ICurrentUserContext`. Members reference their linked user through an `AuthUserId`, keeping the two concepts distinct.

## Alternatives considered

- **Separate identity microservice**: rejected for V1 because it adds distributed-system complexity, more deployment units, secrets and configuration, and a harder self-hosted experience with no clear V1 benefit.
- **External self-hosted identity provider (for example Keycloak or authentik)**: not the V1 default because it adds operational burden and moving parts and fits a minimal local-first installation poorly; kept as a valuable future option for standards-based federation.

## References

- Legacy source: `docs/02_architecture/adrs/ADR-002-authentication-and-identity.md`
- Implementation: [`src/backend/DomusMind.Infrastructure/Auth/`](../../src/backend/DomusMind.Infrastructure/Auth/), [`src/backend/DomusMind.Application/Features/Auth/`](../../src/backend/DomusMind.Application/Features/Auth/), [`src/backend/DomusMind.Application/Abstractions/Security/ICurrentUser.cs`](../../src/backend/DomusMind.Application/Abstractions/Security/ICurrentUser.cs)
- Related: [ADR-0003](0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md), [ADR-0004](0004-build-a-domain-centric-modular-monolith.md)
