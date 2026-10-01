---
title: Architecture Constraints
arc42-section: "02"
description: The technical, product, organizational and convention constraints that restrict DomusMind's architecture, and their consequences.
---

# Architecture Constraints

Constraints imposed from outside the architecture are separated from team decisions.
A team decision is listed only where it binds every change; its rationale lives in the
linked ADR, and it is negotiable only by superseding that ADR.

## Technical constraints

| Constraint | Source | Architectural consequence | Negotiable |
| --- | --- | --- | --- |
| Backend on .NET 10 and ASP.NET Core; persistence on PostgreSQL through EF Core (Npgsql) | Implementation baseline ([`DomusMind.slnx`](../../DomusMind.slnx), [`deploy/docker-compose.yml`](../../deploy/docker-compose.yml)) | One relational store for all modules and the event log; migrations are applied by the API at startup. | Only with a migration plan |
| External calendars come from Microsoft Outlook through Microsoft Graph with delegated, read-only scopes, pulled rather than pushed | Product constraint (below); Microsoft identity platform | DomusMind holds each connection's token cache server-side, refreshes through a background worker and never writes to Graph. The integration needs a Microsoft app registration supplied as configuration; without it the connect flow reports the feature as unconfigured. See [ADR-0003](../adr/0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md). | No, within Phase 1 |
| The deliverable must run on a household's own container host | Self-hosting distribution ([`deploy/`](../../deploy/)) | API and web app ship as one container image; all settings arrive as environment variables; no managed cloud service is required at runtime. See [deployment view](07-deployment-view.md). | No |

The Graph constraint derives from:

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:7c5c21e2aa317fb9e77b5e4feafe491cfd68b7506203b47bae37b46392d185cb" -->

## Product constraints

These are owned by the product model; only their architectural consequence is stated
here.

| Constraint | Architectural consequence | Negotiable |
| --- | --- | --- |
| Current product scope | Only the six area modules plus authentication and setup exist; out-of-scope areas, messaging channels, AI and further integrations get no building blocks or extension points ahead of need. | Product decision |
| Household-first | The family is the partition key of household data and the unit of authorization. | Product decision |
| Separate axes | One module owns each axis; cross-module read surfaces project data and never write it back. | Product decision |
| No hidden automation | No background process creates household records. The only scheduled job refreshes read-only external calendar state. | Product decision |

<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:5f3b2eacf39dd82ebe0c6616860f4f0d9edac9589d0f8889a7e093db12e9e239" -->
<!-- pdac:cite id="CON-PRODUCT-HOUSEHOLD-FIRST" digest="sha256:9ffd7fd5d31817890e9263cea8286b25102bd41092769a0cfe3e0765792808b3" -->
<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:740fa2cc41d90ec6afb099c5f97fa44e9092af8d83cd68291f807262387a44b0" -->
<!-- pdac:cite id="CON-PRODUCT-NO-MAGIC-AUTOMATION" digest="sha256:954fc2ba5c4d7ea5b528499f39567467ef38878bb7796701f81792ce9c4d020d" -->

## Organizational and process constraints

| Constraint | Source | Architectural consequence | Negotiable |
| --- | --- | --- | --- |
| The product model is the single source of product intent; specs and architecture cite it | [`docs/product/`](../product/README.md), `product-ci` workflow | Architecture and OpenSpec documents carry ProductShape citations that CI verifies; they never restate requirements. | No |
| Behaviour is specified per capability in OpenSpec | [`openspec/specs/`](../../openspec/specs/) | Slices are implemented against a spec; architecture documents stay at the level of shape. | No |

## Convention constraints

Team decisions that every change must respect. They are stated as rules in
[`CLAUDE.md`](../../CLAUDE.md) and justified in the ADRs.

| Convention | ADR |
| --- | --- |
| Domain-centric modular monolith; the domain stays framework-agnostic; projects are not collapsed | [ADR-0004](../adr/0004-build-a-domain-centric-modular-monolith.md) |
| Internal application mediator; no MediatR or similar | [ADR-0001](../adr/0001-use-an-internal-application-mediator.md) |
| REST API through ASP.NET Core controllers, documented with OpenAPI; no Minimal APIs | [ADR-0005](../adr/0005-expose-a-rest-api-through-aspnet-core-controllers.md) |
| Explicit mapping to API contract models; no AutoMapper; domain entities never exposed | [ADR-0006](../adr/0006-map-explicitly-without-automapper.md) |
| EF Core used directly; no generic repositories | [ADR-0007](../adr/0007-use-ef-core-directly-without-generic-repositories.md) |
| One command modifies one aggregate; cross-module collaboration through persisted domain events | [ADR-0008](../adr/0008-collaborate-across-modules-through-persisted-domain-events.md) |
| Authentication is local and separate from household member identity | [ADR-0002](../adr/0002-keep-authentication-local-and-separate-from-member-identity.md) |
