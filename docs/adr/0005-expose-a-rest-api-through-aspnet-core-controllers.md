---
name: expose-a-rest-api-through-aspnet-core-controllers
description: Expose DomusMind's capabilities as a REST JSON API implemented with thin ASP.NET Core controllers and documented with Swagger/OpenAPI.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [backend, api]
---

# ADR-0005: Expose a REST API through ASP.NET Core controllers

## Context

This record is written retroactively. The legacy API conventions and `CLAUDE.md` state the rule as mandatory, but no decision record existed for it.

The API is DomusMind's system boundary: every client and integration reaches the system through it, and it exposes domain capabilities rather than database tables. ASP.NET Core offers two styles for HTTP endpoints, controllers and Minimal APIs. Clients and integrators need a documented, discoverable contract, including how to authenticate, and developers need to exercise the API with bearer tokens during development.

## Decision

We will expose a stateless REST API with JSON bodies, implemented with ASP.NET Core controllers and documented with Swagger/OpenAPI.

Controllers are thin transport adapters: they bind explicit request models, dispatch one command or query through the internal mediator, and translate the result into an HTTP response. They contain no domain logic, persistence logic or cross-slice orchestration. Swagger/OpenAPI documents endpoints, request and response models, authentication requirements and error responses, and allows bearer-token testing; it is part of the development workflow and integration experience.

## Consequences

- **Positive:** One consistent, documented boundary for every surface. Controllers stay uniform and easy to review because behaviour lives in slices. The OpenAPI document gives integrators a discoverable contract.
- **Negative:** Controllers carry more ceremony than Minimal API endpoints. Swagger annotations must be maintained alongside the controllers.
- **Neutral:** Checked against the code on 2026-10-01, the rule holds: the API registers `AddControllers`/`MapControllers`, all 16 controllers derive from `ControllerBase`, and Swashbuckle serves Swagger with bearer-auth wiring. The only Minimal API calls are infrastructure: a root redirect to Swagger when no web app is bundled and the SPA fallback. Request and response models live in the `DomusMind.Contracts.*` namespaces, not `Model.*` as the legacy documents and `CLAUDE.md` name them.

## Alternatives considered

- **Minimal APIs**: rejected as the default style by the API conventions, which require controllers as the transport adapters.

## References

- Legacy source: `docs/06_interfaces/api.md`
- Rule: [`CLAUDE.md`](../../CLAUDE.md) (mandatory architectural rules)
- Evidence: [`src/backend/DomusMind.Api/Program.cs`](../../src/backend/DomusMind.Api/Program.cs), [`src/backend/DomusMind.Api/Controllers/`](../../src/backend/DomusMind.Api/Controllers/), [`src/backend/DomusMind.Api/OpenApi/OpenApiAuthExtensions.cs`](../../src/backend/DomusMind.Api/OpenApi/OpenApiAuthExtensions.cs), [`src/backend/DomusMind.Api/DomusMind.Api.csproj`](../../src/backend/DomusMind.Api/DomusMind.Api.csproj)
- Related: [ADR-0001](0001-use-an-internal-application-mediator.md), [ADR-0006](0006-map-explicitly-without-automapper.md)
