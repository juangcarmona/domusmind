# Spec — Set Item Importance

## Purpose

Set or clear the importance flag on a household list item.

Importance marks an item as starred — visually prioritized in the list and in any Agenda projection.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `set-item-importance`
- Command: `SetSharedListItemImportance`

---

## Inputs

Required:

- `listId`
- `itemId`
- `importance` (bool)

---

## Preconditions

- list must exist
- item must exist within the target list

---

## Behavior

- set `importance` to the provided value
- if `importance` is already the provided value, the operation is idempotent (no error, no change)
- record the timestamp of the update

---

## Result

- `itemId`
- `importance` (current value)
- `updatedAtUtc`

---

## Events

Emit:

- `SharedListItemImportanceSet` — payload includes itemId, listId, importance value

---

## Failure

- list not found
- item not found in the list

---

## Constraints

- Importance is a binary flag. It is never a score or ranking.
- This command does not affect temporal fields, checked state, or any other item field.
- Importance does not cause automatic Agenda projection by itself. Temporal fields are required for Agenda eligibility.
- The command is always a no-op if the value does not change (idempotent).
