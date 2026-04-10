# Spec - Get List Detail

## Purpose

Retrieve the full detail of a household list, including its items.

---

## Context

- Module: Lists
- Read Model: `SharedListDetail`
- Slice: `get-shared-list-detail`
- Query: `GetSharedListDetail`

---

## Inputs

Required:

- `listId`

---

## Preconditions

- list must exist

---

## Query Behavior

- retrieve the target list's metadata and its full ordered item collection
- items are split into two groups for surface rendering:
  - unchecked items (ordered by stable append position)
  - checked items (ordered by stable append position)

---

## Result

List metadata:

- `listId`
- `name`
- `uncheckedCount`

Optional metadata (when set):

- `areaId`
- `linkedPlanId`
- `kind`

Items (per item):

- `itemId`
- `name`
- `checked`
- `quantity` (optional)
- `note` (optional)
- `order`

---

## Failure

- list not found

---

## Constraints

- Each item in the result must conform strictly to the item model: name, quantity, note, checked, order.
- Items must not include due dates, reminders, assignees, priority, recurrence, or status fields.
- Items must not be described or treated as tasks in any consuming context.
- Internal audit metadata (`updatedAtUtc`, `updatedByMemberId`) is not part of the item model exposed to the surface. If persisted internally, it must not influence item semantics.
- Ordering is stable and based on append position. No external sort attributes exist on items.