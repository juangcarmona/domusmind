---
title: Architecture Decisions
arc42-section: "09"
description: Index of architecturally significant decisions and the architecture views they affect.
---

# Architecture Decisions

The ADRs in [`docs/adr/`](../adr/README.md) own the status, context, rationale, alternatives and consequences of each decision. This index only says where each decision shapes the architecture.

| Decision | ADR | Affected architecture |
| --- | --- | --- |
| Use an internal application mediator | [ADR-0001](../adr/0001-use-an-internal-application-mediator.md) | [04 Solution strategy](04-solution-strategy.md), [05 Building block view](05-building-block-view.md), [06 Runtime view](06-runtime-view.md) |
| Keep authentication local and separate from member identity | [ADR-0002](../adr/0002-keep-authentication-local-and-separate-from-member-identity.md) | [03 Context and scope](03-context-and-scope.md), [05 Building block view](05-building-block-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md) |
| Use delegated Graph auth for Outlook calendar ingestion | [ADR-0003](../adr/0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md) | [03 Context and scope](03-context-and-scope.md), [06 Runtime view](06-runtime-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md) |
| Build a domain-centric modular monolith | [ADR-0004](../adr/0004-build-a-domain-centric-modular-monolith.md) | [04 Solution strategy](04-solution-strategy.md), [05 Building block view](05-building-block-view.md), [07 Deployment view](07-deployment-view.md) |
| Expose a REST API through ASP.NET Core controllers | [ADR-0005](../adr/0005-expose-a-rest-api-through-aspnet-core-controllers.md) | [03 Context and scope](03-context-and-scope.md), [05 Building block view](05-building-block-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md) |
| Map explicitly without AutoMapper | [ADR-0006](../adr/0006-map-explicitly-without-automapper.md) | [05 Building block view](05-building-block-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md) |
| Use EF Core directly without generic repositories | [ADR-0007](../adr/0007-use-ef-core-directly-without-generic-repositories.md) | [05 Building block view](05-building-block-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md) |
| Collaborate across modules through persisted domain events | [ADR-0008](../adr/0008-collaborate-across-modules-through-persisted-domain-events.md) | [05 Building block view](05-building-block-view.md), [06 Runtime view](06-runtime-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md), [11 Risks and technical debt](11-risks-and-technical-debt.md) |
| Use GUID-backed strongly typed identifiers | [ADR-0010](../adr/0010-use-guid-backed-strongly-typed-identifiers.md) | [04 Solution strategy](04-solution-strategy.md), [05 Building block view](05-building-block-view.md), [08 Crosscutting concepts](08-crosscutting-concepts.md) |

[ADR-0009](../adr/0009-use-ulid-based-strongly-typed-identifiers.md) (ULID identifiers) is superseded by ADR-0010 and shapes no view; it was never implemented.
