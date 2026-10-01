---
title: Solution Strategy
arc42-section: "04"
description: The fundamental approaches that shape DomusMind's architecture and the drivers each one answers.
---

# Solution Strategy

Each strategy below answers a driver from [section 01](01-introduction-and-goals.md)
or a constraint from [section 02](02-constraints.md). Rationale and alternatives live
in the linked ADRs; structure and behaviour live in the linked views.

| Driver | Strategy | Detailed view or ADR |
| --- | --- | --- |
| Evolvability of the domain model; one owner per product axis | A **domain-centric modular monolith**: one deployable, split into Domain, Application, Contracts, Infrastructure and Api projects, with one module per bounded context running through them. The framework-free domain sits at the centre and dependencies point inwards. | [ADR-0004](../adr/0004-build-a-domain-centric-modular-monolith.md), [building blocks](05-building-block-view.md) |
| Explicit, independently evolving capabilities | **Vertical slices** per capability inside each module, executed as explicit commands and queries through an internal mediator. A command changes one aggregate; handlers never call other handlers. | [ADR-0001](../adr/0001-use-an-internal-application-mediator.md), [crosscutting concepts](08-crosscutting-concepts.md) |
| Modules stay decoupled | Modules share only identifiers and **domain events**. Every event raised by an aggregate is appended to a persisted event log. The intended collaboration mechanism is in-process reaction to those events; today events are persisted only, and no event handlers are registered or dispatched. | [ADR-0008](../adr/0008-collaborate-across-modules-through-persisted-domain-events.md), [runtime view](06-runtime-view.md), [risks](11-risks-and-technical-debt.md) |
| One product on every screen | **API-first**: the REST API, built from ASP.NET Core controllers over explicit contract models with explicit mapping, is the only way into the system. One responsive web app is its client on desktop and mobile. | [ADR-0005](../adr/0005-expose-a-rest-api-through-aspnet-core-controllers.md), [ADR-0006](../adr/0006-map-explicitly-without-automapper.md) |
| Today understood at a glance; read surfaces never own data | **Read projections**: cross-module surfaces such as the Agenda are queries that project from each owning module with EF Core directly and without change tracking, and never write back. | [ADR-0007](../adr/0007-use-ef-core-directly-without-generic-repositories.md), [runtime view](06-runtime-view.md) |
| Household data isolation | **Local authentication** issues JWT access and refresh tokens to a user identity that is separate from household membership. Every household request is authorized against the caller's access to that family on the server. | [ADR-0002](../adr/0002-keep-authentication-local-and-separate-from-member-identity.md), [crosscutting concepts](08-crosscutting-concepts.md) |
| Read-only Outlook ingestion, pull only | An **integration adapter** in Infrastructure, behind an Application abstraction, imports entries into a separate read-only model owned by the Calendar module. Members trigger refreshes manually and a hosted worker runs scheduled ones. | [ADR-0003](../adr/0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md), [runtime view](06-runtime-view.md) |
| Operable by a single household | **One container image** in which the API serves the built web app, with PostgreSQL alongside and schema migrations applied at startup. .NET Aspire orchestrates the same parts for local development. | [Deployment view](07-deployment-view.md) |

The module set mirrors the product's bounded contexts one to one, and the projection
strategy keeps each axis owned by a single module:

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:740fa2cc41d90ec6afb099c5f97fa44e9092af8d83cd68291f807262387a44b0" -->
<!-- pdac:cite id="BC-FAMILY" digest="sha256:3bf7d1e482866302ac3725ac77eed6a5f37cebf2ba60a99323a37194c8f187ae" -->
<!-- pdac:cite id="BC-RESPONSIBILITIES" digest="sha256:288de55e6b2023b73e2a4c9992f9390250c4a7690500cd4d6a207c432f324cc5" -->
<!-- pdac:cite id="BC-CALENDAR" digest="sha256:672274e54e0fbc3961c16ed85b7c8a6db31b7ff828ca0a88304a049be3389c0f" -->
<!-- pdac:cite id="BC-TASKS" digest="sha256:7a1a64e2267e7795b9766611ab9d65af92e80600a128617a728d4bff899a032d" -->
<!-- pdac:cite id="BC-LISTS" digest="sha256:278bab0e63c76bf592a652d1a53fcaf99a6a79cf52153a4d566d26dfcd1bf0e0" -->
<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:3e2d211a42325e9e719480512210f6ec9c5255f4fa647518cae3598e2bd6e835" -->

The Outlook adapter realises the pull-only, read-only integration constraint:

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:7c5c21e2aa317fb9e77b5e4feafe491cfd68b7506203b47bae37b46392d185cb" -->

Identifiers are strongly typed value objects backed by GUIDs
([ADR-0010](../adr/0010-use-guid-backed-strongly-typed-identifiers.md)); the mechanism
is described in [section 08](08-crosscutting-concepts.md#identifiers).

The strategy deliberately adds no extension points for capabilities outside the
current scope, such as messaging channels, AI interpretation or further calendar
providers. They would arrive as new modules, slices or adapters within the same shape.

<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:5f3b2eacf39dd82ebe0c6616860f4f0d9edac9589d0f8889a7e093db12e9e239" -->
