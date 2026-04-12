# Implementation Plan — List Item Capability Model

Status: Pre-implementation freeze
Audience: Engineering
Scope: V1 Lists extension
Depends on:
  - docs/04_contexts/shared-lists.md
  - docs/04_contexts/shared-lists-item-model.md
  - specs/features/lists/set-item-importance.md
  - specs/features/lists/set-item-temporal.md
  - specs/features/lists/clear-item-temporal.md
  - specs/read-models/calendar/view-member-agenda.md
  - specs/read-models/calendar/view-family-timeline.md

---

## Summary

This plan covers the extension of the Shared Lists item model to support the full capability set:

- importance flag
- temporal fields (due date, reminder, repeat)
- Agenda projection for temporally-enriched items

The plan covers Domain → Persistence → Commands → Queries → API.

It does not cover UI implementation. UI follows separately once the backend contract is stable.

---

## Constraints

- Do NOT introduce task creation from list items at any step
- Do NOT create Calendar entities from list item temporal fields
- Do NOT push to Agenda via events — Agenda projection is a query concern
- Treat each step as independently releasable
- All invariants defined in `shared-lists-item-model.md` must be enforced at the domain layer

---

## Phase 1 — Domain Model

### Files to change

- `DomusMind.Domain` — `SharedListItem` entity or value object

### Changes

Add fields to `SharedListItem`:

```csharp
bool Importance          // default: false
DateOnly? DueDate
DateTimeOffset? Reminder
RepeatRule? Repeat       // value object wrapping recurrence expression
```

### Invariants to enforce in domain

- `repeat` requires `DueDate` — if `DueDate` is null and `Repeat` is set → domain exception
- `name` non-empty on all mutations
- Setting `Importance` does not affect temporal fields and vice versa
- `Toggle` does not modify capability fields

### New domain methods on SharedListItem (or SharedList routing to item)

- `SetImportance(bool importance)`
- `SetTemporal(DateOnly? dueDate, DateTimeOffset? reminder, RepeatRule? repeat)`
- `ClearTemporal()`

### Domain events to add

- `SharedListItemImportanceSet`
- `SharedListItemScheduled` — raised by both `SetTemporal` (on first temporal field) and `ClearTemporal` (when clearing)
- Reuse `SharedListItemUpdated` for subsequent temporal field changes after first set

---

## Phase 2 — Persistence

### Files to change

- `DomusMind.Infrastructure` — EF Core mapping for `SharedListItem`

### Changes

Add nullable columns to `SharedListItems` table:

```sql
Importance   BIT          NOT NULL DEFAULT 0
DueDate      DATE         NULL
Reminder     DATETIMEOFFSET NULL
Repeat       NVARCHAR(50) NULL     -- RepeatRule serialized as string
```

### Migration

- Add EF Core migration: `AddListItemCapabilityFields`
- All new columns default to their null-equivalent
- No existing data transformed
- Migration is safe to apply to production with existing rows

---

## Phase 3 — Commands (Write Side)

### New slices

Each slice follows the pattern defined in `docs/05_slices/slice-conventions.md`.

#### `set-item-importance`

- Command: `SetSharedListItemImportance`
- Handler: load `SharedList`, call `SetImportance` on item, persist, emit `SharedListItemImportanceSet`
- Validator: listId required, itemId required, importance is bool

#### `set-item-temporal`

- Command: `SetSharedListItemTemporal`
- Handler: load `SharedList`, call `SetTemporal` on item, persist, emit event
- Validator: listId and itemId required; at least one temporal field provided; repeat without dueDate context rejected

#### `clear-item-temporal`

- Command: `ClearSharedListItemTemporal`
- Handler: load `SharedList`, call `ClearTemporal` on item, persist, emit `SharedListItemScheduled` if item had temporal data
- Validator: listId and itemId required

### Existing slice to verify (no command change needed)

- `update-list-item` — verify constraints already updated; no temporal fields go through this command
- `toggle-shared-list-item` — verify toggle does not clear temporal fields

---

## Phase 4 — Read Model Projection (Agenda)

### New query

- `GetTemporalItemsForAgenda` — query in Shared Lists context
- Returns items with any temporal field within the requested date window
- Scoped to a family
- Optional: filter by window (startDate, endDate)
- Uses `AsNoTracking()` + projection — no full aggregate load

### Integration into Agenda projection

The existing `GetMemberAgenda` and `GetFamilyTimeline` query handlers must be extended to:

1. Call `GetTemporalItemsForAgenda` for the family scoped to the requested window
2. Map results to `AgendaEntry` with `type = list-item`
3. Apply ordering rules (see `shared-lists-item-model.md` — Ordering within Agenda day)
4. Never merge `list-item` entries into `task` or `event` identity

### Identity discriminator

The read model response must include `type` as a discriminated string:

```
event | task | routine | external-calendar-entry | list-item
```

This discriminator is the contract. It must not collapse or alias item types.

---

## Phase 5 — API Contracts

### New endpoints (in `DomusMind.Api`)

```
PATCH /lists/{listId}/items/{itemId}/importance
PATCH /lists/{listId}/items/{itemId}/temporal
DELETE /lists/{listId}/items/{itemId}/temporal
```

### Updated response contracts

`SharedListItemResponse` in `DomusMind.Contracts` must include:

```
importance: bool
dueDate: string? (ISO 8601 date)
reminder: string? (ISO 8601 datetime)
repeat: string? (RepeatRule expression)
```

`AgendaEntryResponse` in `DomusMind.Contracts` must be updated to:

- support `type: list-item`
- include `listId`, `listName`, `importance` when type is `list-item`

---

## Phase 6 — UI

UI implementation is NOT part of this plan.

UI will be designed against the stable API contracts produced above.

The following UI slices are implied but separate:

- Lists inspector: importance + temporal controls
- Agenda rendering: `list-item` entry type visual treatment
- Agenda item: `Open in Lists` navigation on list-item selection

---

## Implementation Order (strict)

```
Phase 1 — Domain model (SharedListItem fields + invariants + domain methods)
Phase 2 — Persistence (migration + EF mapping)
Phase 3 — Commands (three new slices + verify existing)
Phase 4 — Read model projection (GetTemporalItemsForAgenda + Agenda query integration)
Phase 5 — API contracts + endpoints
Phase 6 — UI (separate, after Phase 5 is stable)
```

Each phase must compile and pass tests before the next phase starts.

---

## Test Requirements

Before any phase is considered done:

- `dotnet build` must succeed
- `dotnet test` must pass (all tests)

New test coverage expected for:

- `SharedListItem` domain invariants (importance, temporal constraints, toggle behavior)
- `SetSharedListItemTemporal` handler — valid cases and repeat-without-dueDate rejection
- `ClearSharedListItemTemporal` handler — idempotency, event emission behavior
- `GetTemporalItemsForAgenda` query — window filtering, null defaults, checked item inclusion
- Agenda projection — `list-item` entries appear and are ordered correctly

---

## Failure Modes to Prevent

| Risk | Prevention |
|------|------------|
| list-item becomes task | invariant: no task creation command is ever called from list item operations |
| temporal item becomes Calendar event | invariant: no Calendar command is ever called from list item operations |
| Agenda projection duplicates items | discriminated `type` field enforced in all read model shapes |
| Repeat set without dueDate reaches persistence | domain layer rejects before persistence |
| Existing items gain temporal status silently | null defaults on all new columns; no implicit conversions |
| UI edits temporal item via Agenda | API enforces `isReadOnly = true` for list-item entries in Agenda response |
