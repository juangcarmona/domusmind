---
title: Solution Strategy
arc42-section: "04"
description: The fundamental approaches that shape DomusMind's architecture and the drivers each one answers.
---

# Solution Strategy

Each strategy below answers a driver from [section 01](../01-introduction-and-goals/README.md)
or a constraint from [section 02](../02-constraints/README.md). Rationale and alternatives live
in the linked ADRs; structure and behaviour live in the linked views.

| Driver | Strategy | Detailed view or ADR |
| --- | --- | --- |
| Evolvability of the domain model; one owner per product axis | A **domain-centric modular monolith**: one deployable, split into Domain, Application, Contracts, Infrastructure and Api projects, with one module per bounded context running through them. The framework-free domain sits at the centre and dependencies point inwards. | [ADR-0004](../../adr/0004-build-a-domain-centric-modular-monolith.md), [building blocks](../05-building-block-view/README.md) |
| Explicit, independently evolving capabilities | **Vertical slices** per capability inside each module, executed as explicit commands and queries through an internal mediator. A command changes one aggregate; handlers never call other handlers. | [ADR-0001](../../adr/0001-use-an-internal-application-mediator.md), [crosscutting concepts](../08-crosscutting-concepts/README.md) |
| Modules stay decoupled | Modules share only identifiers and **domain events**. Every event raised by an aggregate is appended to a persisted event log. The intended collaboration mechanism is in-process reaction to those events; today events are persisted only, and no event handlers are registered or dispatched. | [ADR-0008](../../adr/0008-collaborate-across-modules-through-persisted-domain-events.md), [runtime view](../06-runtime-view/README.md), [risks](../11-risks-and-technical-debt/README.md) |
| One product on every screen | **API-first**: the REST API, built from ASP.NET Core controllers over explicit contract models with explicit mapping, is the only way into the system. One responsive web app is its client on desktop and mobile. | [ADR-0005](../../adr/0005-expose-a-rest-api-through-aspnet-core-controllers.md), [ADR-0006](../../adr/0006-map-explicitly-without-automapper.md) |
| Today understood at a glance; read surfaces never own data | **Read projections**: cross-module surfaces such as the Agenda are queries that project from each owning module with EF Core directly and without change tracking, and never write back. | [ADR-0007](../../adr/0007-use-ef-core-directly-without-generic-repositories.md), [runtime view](../06-runtime-view/README.md) |
| Household data isolation | **Local authentication** issues JWT access and refresh tokens to a user identity that is separate from household membership. Every household request is authorized against the caller's access to that family on the server. | [ADR-0002](../../adr/0002-keep-authentication-local-and-separate-from-member-identity.md), [crosscutting concepts](../08-crosscutting-concepts/README.md) |
| Read-only Outlook ingestion, pull only | An **integration adapter** in Infrastructure, behind an Application abstraction, imports entries into a separate read-only model owned by the Calendar module. Members trigger refreshes manually and a hosted worker runs scheduled ones. | [ADR-0003](../../adr/0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md), [runtime view](../06-runtime-view/README.md) |
| Operable by a single household | **One container image** in which the API serves the built web app, with PostgreSQL alongside and schema migrations applied at startup. .NET Aspire orchestrates the same parts for local development. | [Deployment view](../07-deployment-view/README.md) |

The module set mirrors the product's bounded contexts one to one, and the projection
strategy keeps each axis owned by a single module:

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:f0adb53673dfbfd7051791182578f9ee2f44e0a37b7699c16258854579342a13" -->
<!-- pdac:cite id="BC-FAMILY" digest="sha256:1b48c9a4b2eba0dfb34314f7afbe1b5801e4f8c41e0ab5a586a9e7da57f7ee4f" -->
<!-- pdac:cite id="BC-RESPONSIBILITIES" digest="sha256:031c9bd3da1ee33bb97c7f5b66f5d37827db17703653d1b00013680d61c55e58" -->
<!-- pdac:cite id="BC-CALENDAR" digest="sha256:a365941e30ed9ca7e5a897db9bfbf057484fb0f87caa0c7a2dc4fdb1275c771f" -->
<!-- pdac:cite id="BC-TASKS" digest="sha256:30abcef1a1eaa3f38dfcb0ada4d54bb3a4f83b4b9c50f1ae3d68b0faa4f835d3" -->
<!-- pdac:cite id="BC-LISTS" digest="sha256:7a770f21b03b8d0f769f46552bc0ef53fd885d5ce0ab41d081d831d2aa74ae4c" -->
<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd" -->

The Outlook adapter realises the pull-only, read-only integration constraint:

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:c575d9e19b4520f13a09facea0c1196b6a42ec1f4554865cdb470485d46eb4b0" -->

Identifiers are strongly typed value objects backed by GUIDs
([ADR-0010](../../adr/0010-use-guid-backed-strongly-typed-identifiers.md)); the mechanism
is described in [section 08](../08-crosscutting-concepts/README.md#identifiers).

The strategy deliberately adds no extension points for capabilities outside the
current scope, such as messaging channels, AI interpretation or further calendar
providers. They would arrive as new modules, slices or adapters within the same shape.

<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:bbe8615d41428bc12d1a225306bd3b5b1fc4e4747dc80c9125220f483bc610a9" -->
