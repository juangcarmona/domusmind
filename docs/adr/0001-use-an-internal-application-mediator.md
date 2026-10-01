---
name: use-an-internal-application-mediator
description: Dispatch commands, queries and domain events through a minimal internal mediator built on the native DI container instead of an external mediator framework.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [backend, application, messaging]
---

# ADR-0001: Use an internal application mediator

## Context

DomusMind is a modular monolith built from vertical slices, with bounded contexts as modules, explicit command/query separation and domain events. Controllers must reach slice handlers without depending on them directly, and domain events must reach their handlers without coupling the emitting module to its subscribers. Something has to route commands, queries and domain events to the handler that owns them.

External mediator libraries, MediatR in particular, solve this generically. They add a third-party dependency to the centre of the architecture, and MediatR has changed its licence. DomusMind only needs dispatch by message type; it does not need a general pipeline framework.

This record migrates the legacy `ADR-001 - Internal Application Mediator`, which was accepted without a recorded date; the date above is the legacy file's first commit.

## Decision

We will implement a minimal internal application mediator and use no external mediator framework.

The mediator is a small set of contracts owned by the application: command and query markers with their handlers, a domain event marker with its handlers, and one dispatcher each for commands, queries and domain events. The dispatchers resolve handlers from the native .NET dependency injection container. The mediator provides only the behaviour DomusMind needs, which keeps the dispatch path under our control, easy to debug and trace, and stable over the long term.

## Consequences

- **Positive:** No external architectural dependency and no exposure to third-party licence changes. Dispatch behaviour is explicit and predictable, which makes the contracts easy to document for human and AI-assisted development. Fewer abstraction layers between a controller and its handler.
- **Negative:** DomusMind owns a small amount of infrastructure code. Any pipeline behaviour (validation, logging, transactions around handlers) has to be built internally when it is needed.
- **Neutral:** Checked against the code on 2026-10-01, the command and query dispatchers are in use by every controller and resolve handlers by reflection over the DI container; a missing registration surfaces as a dedicated handler-resolution error mapped by the API. Two parts of the contract set are not exercised: the domain event dispatcher is registered but never invoked, and no domain event handler exists (see [ADR-0008](0008-collaborate-across-modules-through-persisted-domain-events.md)); an `IValidator<T>` contract exists but has no implementations and is not part of the dispatch path, so validation currently happens inside handlers and the domain.

## Alternatives considered

- **MediatR**: rejected because it is an external dependency, its licence has changed, and its abstraction exceeds what DomusMind needs.
- **Direct handler invocation from controllers or other handlers**: rejected because it breaks the architectural boundaries between slices, complicates testing and increases coupling.

## References

- Legacy source: `docs/02_architecture/adrs/ADR-001-internal-application-mediator.md`
- Contracts: [`src/backend/DomusMind.Application/Abstractions/Messaging/`](../../src/backend/DomusMind.Application/Abstractions/Messaging/), [`src/backend/DomusMind.Domain/Abstractions/IDomainEvent.cs`](../../src/backend/DomusMind.Domain/Abstractions/IDomainEvent.cs)
- Dispatchers: [`src/backend/DomusMind.Infrastructure/Messaging/`](../../src/backend/DomusMind.Infrastructure/Messaging/)
- Related: [ADR-0004](0004-build-a-domain-centric-modular-monolith.md), [ADR-0008](0008-collaborate-across-modules-through-persisted-domain-events.md)
