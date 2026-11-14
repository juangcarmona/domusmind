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
| Backend on .NET 10 and ASP.NET Core; persistence on PostgreSQL through EF Core (Npgsql) | Implementation baseline ([`DomusMind.slnx`](../../../DomusMind.slnx), [`deploy/docker-compose.yml`](../../../deploy/docker-compose.yml)) | One relational store for all modules and the event log; migrations are applied by the API at startup. | Only with a migration plan |
| External calendars come from Microsoft Outlook through Microsoft Graph with delegated, read-only scopes, pulled rather than pushed | Product constraint (below); Microsoft identity platform | DomusMind holds each connection's token cache server-side, refreshes through a background worker and never writes to Graph. The integration needs a Microsoft app registration supplied as configuration; without it the connect flow reports the feature as unconfigured. See [ADR-0003](../../adr/0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md). | No, within Phase 1 |
| The deliverable must run on a household's own container host | Self-hosting distribution ([`deploy/`](../../../deploy/)) | API and web app ship as one container image; all settings arrive as environment variables; no managed cloud service is required at runtime. See [deployment view](../07-deployment-view/README.md). | No |

The Graph constraint derives from:

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:c575d9e19b4520f13a09facea0c1196b6a42ec1f4554865cdb470485d46eb4b0" -->

## Product constraints

These are owned by the product model; only their architectural consequence is stated
here.

| Constraint | Architectural consequence | Negotiable |
| --- | --- | --- |
| Current product scope | Eight backend areas include Inbox and Chats. Approved Inbox proposals execute through the owning area. Household Assets remains accepted intent without a backend module; additional capabilities follow reviewed Product Changes. | Product decision |
| Household-first | The family is the partition key of household data and the unit of authorization. | Product decision |
| Separate axes | One module owns each axis; cross-module read surfaces project data and never write it back. | Product decision |
| Explicit household decisions | Assistant requests can stage Inbox proposals. Each action requires explicit approval and the owning area's authorization and validation. Scheduled calendar refresh imports read-only external data. | Product decision |

<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:86c4b7c550c3e4edf5c1f614510ece6a13274480c6333c7114e1a103f0219169" -->
<!-- pdac:cite id="CON-PRODUCT-HOUSEHOLD-FIRST" digest="sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a" -->
<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:f0adb53673dfbfd7051791182578f9ee2f44e0a37b7699c16258854579342a13" -->
<!-- pdac:cite id="CON-PRODUCT-NO-MAGIC-AUTOMATION" digest="sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161" -->

## Organizational and process constraints

| Constraint | Source | Architectural consequence | Negotiable |
| --- | --- | --- | --- |
| The product model is the single source of product intent; specs and architecture cite it | [`docs/product/`](../../product/README.md), `product-ci` workflow | Architecture and OpenSpec documents carry ProductShape citations that CI verifies; they never restate requirements. | No |
| Behaviour is specified per capability in OpenSpec | [`openspec/specs/`](../../../openspec/specs/) | Slices are implemented against a spec; architecture documents stay at the level of shape. | No |

## Convention constraints

Team decisions that every change must respect. They are stated as rules in
the [accepted ADRs](../../adr/README.md).

| Convention | ADR |
| --- | --- |
| Domain-centric modular monolith; the domain stays framework-agnostic; projects are not collapsed | [ADR-0004](../../adr/0004-build-a-domain-centric-modular-monolith.md) |
| Internal application mediator; no MediatR or similar | [ADR-0001](../../adr/0001-use-an-internal-application-mediator.md) |
| REST API through ASP.NET Core controllers, documented with OpenAPI; no Minimal APIs | [ADR-0005](../../adr/0005-expose-a-rest-api-through-aspnet-core-controllers.md) |
| Explicit mapping to API contract models; no AutoMapper; domain entities never exposed | [ADR-0006](../../adr/0006-map-explicitly-without-automapper.md) |
| EF Core used directly; no generic repositories | [ADR-0007](../../adr/0007-use-ef-core-directly-without-generic-repositories.md) |
| One command modifies one aggregate; cross-module collaboration through persisted domain events | [ADR-0008](../../adr/0008-collaborate-across-modules-through-persisted-domain-events.md) |
| Authentication is local and separate from household member identity | [ADR-0002](../../adr/0002-keep-authentication-local-and-separate-from-member-identity.md) |
