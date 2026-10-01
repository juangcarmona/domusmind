---
name: collaborate-across-modules-through-persisted-domain-events
description: Let modules collaborate only through domain events owned by the emitting module, and persist every committed domain event in an append-only event log without adopting event sourcing.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [backend, messaging, persistence]
---

# ADR-0008: Collaborate across modules through persisted domain events

## Context

This record is written retroactively. The legacy event-processing and data-model documents and `CLAUDE.md` state the rule as mandatory, but no decision record existed for it.

Bounded contexts in the modular monolith must react to each other's changes (a scheduled event may lead to preparation tasks or reminders in another module) without coupling to each other's internals. One command modifies one aggregate, and handlers do not call other handlers. The system also needs an audit history of meaningful state changes, a basis for retries and asynchronous consumers, and material for future projections, analytics and integrations.

## Decision

We will make domain events the only mechanism for cross-module collaboration, and we will persist every committed domain event in an append-only event log.

Aggregates emit immutable, past-tense events after a state change. Each event belongs to exactly one bounded context; only the owning module emits it and other modules may subscribe. Events are dispatched in-memory for reactions within the same request, after persistence, and are appended to the event log with their type, owning module, aggregate, timestamp, version and payload. Consumers are idempotent, tolerant to retries and do not mutate the emitting aggregate. Events are long-term contracts: breaking payload changes are avoided in favour of new or versioned events. The log is a record of facts, not the source of aggregate state.

## Consequences

- **Positive:** Modules stay decoupled while still reacting to each other. The log gives auditability and a foundation for retries, asynchronous consumers and projections.
- **Negative:** Event payloads become contracts that must be versioned with care. Cross-module reactions are indirect and harder to follow than direct calls.
- **Neutral:** Checked against the code on 2026-10-01, only the persistence half is realised. Command handlers write their aggregate's events through `IEventLogWriter` into the `event_log` table, and nothing updates or deletes entries, so the log is append-only by practice (not by database constraint). The in-memory half is not: `IDomainEventDispatcher` is registered but never invoked and no `IDomainEventHandler` exists, so no module reacts to another's events today. The writer records `AggregateType` and `AggregateId` as `"unknown"`, infers the module only for Family, Responsibilities, Calendar and Tasks, and leaves correlation and causation empty. Some handlers write events in a second `SaveChanges` after the aggregate, so the two are not always atomic, and `RequestShoppingList` modifies a meal plan and creates a shared list in one command instead of reacting to an event. An outbox remains an optional later refinement.

## Alternatives considered

- **Event sourcing**: rejected for V1 because the storage strategy is an append-only event log with no event sourcing, which the data model states is not required.
- **Direct calls between modules or between handlers**: ruled out by the rule itself: handlers do not call other handlers, and cross-module reactions happen through domain events.

## References

- Legacy sources: `docs/02_architecture/event-processing.md`, `docs/07_platform/data-model.md` (Event Log)
- Rule: [`CLAUDE.md`](../../CLAUDE.md) (mandatory architectural rules)
- Evidence: [`src/backend/DomusMind.Application/Abstractions/Persistence/IEventLogWriter.cs`](../../src/backend/DomusMind.Application/Abstractions/Persistence/IEventLogWriter.cs), [`src/backend/DomusMind.Infrastructure/Events/EventLogWriter.cs`](../../src/backend/DomusMind.Infrastructure/Events/EventLogWriter.cs), [`src/backend/DomusMind.Infrastructure/Messaging/DomainEventDispatcher.cs`](../../src/backend/DomusMind.Infrastructure/Messaging/DomainEventDispatcher.cs), [`src/backend/DomusMind.Application/Features/MealPlanning/RequestShoppingList/RequestShoppingListCommandHandler.cs`](../../src/backend/DomusMind.Application/Features/MealPlanning/RequestShoppingList/RequestShoppingListCommandHandler.cs)
- Related: [ADR-0001](0001-use-an-internal-application-mediator.md), [ADR-0004](0004-build-a-domain-centric-modular-monolith.md)
