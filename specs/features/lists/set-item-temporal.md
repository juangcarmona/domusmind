# Spec — Set Item Temporal

## Purpose

Set temporal fields on a household list item: due date, reminder, and/or repeat rule.

Setting any temporal field makes the item eligible to project into the Agenda surface.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `set-item-temporal`
- Command: `SetSharedListItemTemporal`

---

## Inputs

Required:

- `listId`
- `itemId`

At least one of the following must be provided:

- `dueDate` (DateOnly)
- `reminder` (DateTimeOffset — absolute UTC or local representation)
- `repeat` (RepeatRule)

---

## Preconditions

- list must exist
- item must exist within the target list
- if `repeat` is provided, `dueDate` must be set — either in this request or already on the item
- if only `repeat` is provided and no `dueDate` is present on the item or in the request → reject

---

## Behavior

- apply only the provided temporal fields; fields not included in the request are left unchanged
- if `dueDate` is being set and `repeat` is already present with no prior `dueDate` → this fulfills the `repeat` + `dueDate` constraint
- record the timestamp of the update
- if the item transitions from non-temporal (no prior temporal fields) to temporal (at least one field now set) → emit `SharedListItemScheduled`
- if the item already had temporal fields and they are being updated → emit `SharedListItemUpdated`

---

## Result

- `itemId`
- `dueDate` (current value or null)
- `reminder` (current value or null)
- `repeat` (current value or null)
- `updatedAtUtc`

---

## Events

Emit one of:

- `SharedListItemScheduled` — when item transitions from non-temporal to temporal for the first time
- `SharedListItemUpdated` — when item already had temporal fields and they are being changed

---

## Failure

- list not found
- item not found in the list
- `repeat` provided but no `dueDate` available (neither in request nor on existing item)

---

## Constraints

- This command sets temporal fields only. It does not modify name, quantity, note, importance, checked state, or order.
- Setting temporal fields does not convert the item to a task. The item remains a SharedListItem.
- Setting temporal fields does not create a Calendar event.
- Projection into Agenda is a read-model concern driven by query. This command does not push data to Agenda.
- `RepeatRule` is a lightweight recurrence hint for projection eligibility. It is not a Calendar RecurrenceRule.
