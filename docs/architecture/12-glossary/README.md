---
title: Glossary
arc42-section: "12"
description: Architecture-specific technical terms used across the architecture views.
---

# Glossary

This glossary defines architecture terms only. Household and business terms (household, member, plan, task, routine, list, area and the rest) are defined in the product model as `TERM-*` artifacts under [`docs/product/model/domain/terms/`](../../product/model/domain/terms/), and bounded contexts as `BC-*` artifacts under [`docs/product/model/domain/bounded-contexts/`](../../product/model/domain/bounded-contexts/). The architecture never redefines them.

| Term | Definition |
| --- | --- |
| Aggregate | Consistency boundary in `DomusMind.Domain`, modified only through its root, which enforces invariants and raises domain events. |
| Aggregate root | The entity through which every change to an aggregate passes; the only type a command handler loads to change state. |
| Command | Explicit request to change state, implementing `ICommand<TResponse>`; one command modifies one aggregate. |
| Command handler | `ICommandHandler<TCommand, TResponse>` implementation in a slice; loads one aggregate through EF Core, calls it, writes its events to the event log and saves. Handlers never call other handlers. |
| Context exception | Per-module exception carrying an error code (for example `ListException`) that a controller maps to an HTTP status. |
| Dispatcher | Internal mediator (`ICommandDispatcher`, `IQueryDispatcher`, `IDomainEventDispatcher`) that resolves the handler for a message from dependency injection; see [ADR-0001](../../adr/0001-use-an-internal-application-mediator.md). |
| Domain event | Immutable record of a fact that has happened in an aggregate, implementing `IDomainEvent`; see [ADR-0008](../../adr/0008-collaborate-across-modules-through-persisted-domain-events.md). |
| Domain event handler | `IDomainEventHandler<TEvent>` implementation through which one module reacts to another module's event; none exist yet (see [11](../11-risks-and-technical-debt/README.md)). |
| Event log | Append-only `event_log` table holding every persisted domain event with its type, module, time and JSON payload. |
| Member identity | The domain member inside a family, distinct from the authentication user that signs in; see [ADR-0002](../../adr/0002-keep-authentication-local-and-separate-from-member-identity.md). |
| Module | A bounded context realised as a folder per context in each backend project (`Features/<Context>` in Application, `<Context>` in Domain), not as a separate deployable. |
| Projection | Query-time composition of rows from one or more modules into a read model; nothing about it is persisted. |
| Query | Explicit read request implementing `IQuery<TResponse>`, handled by an `IQueryHandler` that projects with `AsNoTracking()` and never changes state. |
| Read model | Response shape built by a projection for one surface, such as the weekly grid or the enriched timeline. Read models are not aggregates. |
| Slice | Vertical capability folder under `Features/<Context>/<Capability>` holding the command or query, its handler and its response mapping. |
| Strongly typed identifier | Per-aggregate `readonly record struct` wrapping a `Guid` (for example `TaskId`), so identifiers of different aggregates cannot be mixed; see [ADR-0010](../../adr/0010-use-guid-backed-strongly-typed-identifiers.md). |
| Temporal projection | The read-side inclusion of list items with timing in the Agenda read models, without transferring ownership from the Lists module. |

## Architecture terms that realise product terms

The Agenda is a set of read models and projections, not a module; its meaning belongs to the product model.

<!-- pdac:cite id="TERM-AGENDA" digest="sha256:a83d53e6c8e54bf187b9038ee34722a44ddd5c98211e1c69089db40b96b26014" -->

The household timeline is likewise a projection over several modules' aggregates.

<!-- pdac:cite id="TERM-HOUSEHOLD-TIMELINE" digest="sha256:410a4fc4b9ef563f593931082c0a49dd92d6d3a90f41a31c56ec86dcb9d358a3" -->

A list item shown in the Agenda is a temporal projection of a Lists aggregate, not a Calendar or Tasks entity.

<!-- pdac:cite id="TERM-PROJECTED-LIST-ITEM" digest="sha256:1d7dee71c8e9ef330968975a9a7b48e15b547f1b823286d462ab6b5ed059078f" -->

The Calendar aggregate `CalendarEvent` is the implementation of the household plan.

<!-- pdac:cite id="TERM-PLAN" digest="sha256:a1ff0f3de21aac20325b367ed1898b3eb3688f97d6c3e0b60b798c724025b07b" -->

The `Family` aggregate and the family identifier that scopes every module's data implement the household boundary.

<!-- pdac:cite id="TERM-HOUSEHOLD" digest="sha256:87493c2e4c72087ed204e99a656f157b648b7f2b5ad636e825bb6edd90716f76" -->

Imported external calendar entries are stored as read-only `ExternalCalendarEntry` records outside the `CalendarEvent` aggregate.

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-ENTRY" digest="sha256:a544452b97f1df5874ba287953f6e9ad857c23c3dd1902fc5f3d22bca412ca6b" -->
