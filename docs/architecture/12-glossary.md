---
title: Glossary
arc42-section: "12"
description: Architecture-specific technical terms used across the architecture views.
---

# Glossary

This glossary defines architecture terms only. Household and business terms (household, member, plan, task, routine, list, area and the rest) are defined in the product model as `TERM-*` artifacts under [`docs/product/model/domain/terms/`](../product/model/domain/terms/), and bounded contexts as `BC-*` artifacts under [`docs/product/model/domain/bounded-contexts/`](../product/model/domain/bounded-contexts/). The architecture never redefines them.

| Term | Definition |
| --- | --- |
| Aggregate | Consistency boundary in `DomusMind.Domain`, modified only through its root, which enforces invariants and raises domain events. |
| Aggregate root | The entity through which every change to an aggregate passes; the only type a command handler loads to change state. |
| Command | Explicit request to change state, implementing `ICommand<TResponse>`; one command modifies one aggregate. |
| Command handler | `ICommandHandler<TCommand, TResponse>` implementation in a slice; loads one aggregate through EF Core, calls it, writes its events to the event log and saves. Handlers never call other handlers. |
| Context exception | Per-module exception carrying an error code (for example `ListException`) that a controller maps to an HTTP status. |
| Dispatcher | Internal mediator (`ICommandDispatcher`, `IQueryDispatcher`, `IDomainEventDispatcher`) that resolves the handler for a message from dependency injection; see [ADR-0001](../adr/0001-use-an-internal-application-mediator.md). |
| Domain event | Immutable record of a fact that has happened in an aggregate, implementing `IDomainEvent`; see [ADR-0008](../adr/0008-collaborate-across-modules-through-persisted-domain-events.md). |
| Domain event handler | `IDomainEventHandler<TEvent>` implementation through which one module reacts to another module's event; none exist yet (see [11](11-risks-and-technical-debt.md)). |
| Event log | Append-only `event_log` table holding every persisted domain event with its type, module, time and JSON payload. |
| Member identity | The domain member inside a family, distinct from the authentication user that signs in; see [ADR-0002](../adr/0002-keep-authentication-local-and-separate-from-member-identity.md). |
| Module | A bounded context realised as a folder per context in each backend project (`Features/<Context>` in Application, `<Context>` in Domain), not as a separate deployable. |
| Projection | Query-time composition of rows from one or more modules into a read model; nothing about it is persisted. |
| Query | Explicit read request implementing `IQuery<TResponse>`, handled by an `IQueryHandler` that projects with `AsNoTracking()` and never changes state. |
| Read model | Response shape built by a projection for one surface, such as the weekly grid or the enriched timeline. Read models are not aggregates. |
| Slice | Vertical capability folder under `Features/<Context>/<Capability>` holding the command or query, its handler and its response mapping. |
| Strongly typed identifier | Per-aggregate `readonly record struct` wrapping a `Guid` (for example `TaskId`), so identifiers of different aggregates cannot be mixed; see [ADR-0010](../adr/0010-use-guid-backed-strongly-typed-identifiers.md). |
| Temporal projection | The read-side inclusion of list items with timing in the Agenda read models, without transferring ownership from the Lists module. |

## Architecture terms that realise product terms

The Agenda is a set of read models and projections, not a module; its meaning belongs to the product model.

<!-- pdac:cite id="TERM-AGENDA" digest="sha256:816f1a2f6d4e87da6b928b3c6d68433adab2c5467f4c02667e00fdb9b86387f0" -->

The household timeline is likewise a projection over several modules' aggregates.

<!-- pdac:cite id="TERM-HOUSEHOLD-TIMELINE" digest="sha256:af4c661643850c49998b6046ecfbf3ad3ec73b46dc61bc0d031acace8ed5b932" -->

A list item shown in the Agenda is a temporal projection of a Lists aggregate, not a Calendar or Tasks entity.

<!-- pdac:cite id="TERM-PROJECTED-LIST-ITEM" digest="sha256:c8ace0e5be8d059740bfd3edd63eb5f8b101c2687ffcccbc7f918c9e5a864fd7" -->

The Calendar aggregate `CalendarEvent` is the implementation of the household plan.

<!-- pdac:cite id="TERM-PLAN" digest="sha256:f06fe7678136c535cf5f55e815f161d73562165b17c565f9496386dbd8395ac3" -->

The `Family` aggregate and the family identifier that scopes every module's data implement the household boundary.

<!-- pdac:cite id="TERM-HOUSEHOLD" digest="sha256:f71aa38813b387bc9962f78a716ad33a144b8b754494e9dd560afab3e5520a98" -->

Imported external calendar entries are stored as read-only `ExternalCalendarEntry` records outside the `CalendarEvent` aggregate.

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-ENTRY" digest="sha256:9ef123050d8fc32bd7755be77ace1bab03fe0009c4583a0bdced3aff9aba6272" -->
