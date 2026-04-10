# Spec — Clear Item Temporal

## Purpose

Remove all temporal fields from a household list item: due date, reminder, and repeat rule.

After this operation the item is no longer eligible for Agenda projection.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `clear-item-temporal`
- Command: `ClearSharedListItemTemporal`

---

## Inputs

Required:

- `listId`
- `itemId`

---

## Preconditions

- list must exist
- item must exist within the target list

---

## Behavior

- clear all three temporal fields atomically: `dueDate = null`, `reminder = null`, `repeat = null`
- if the item has no temporal fields at all, the operation is idempotent (no error, no event emitted)
- record the timestamp of the update

---

## Result

- `itemId`
- `dueDate` (null)
- `reminder` (null)
- `repeat` (null)
- `updatedAtUtc`

---

## Events

Emit (only if the item had temporal fields before the clear):

- `SharedListItemScheduled` — used here to signal projection invalidation to consumers; payload signals that dueDate, reminder, and repeat are now null

---

## Failure

- list not found
- item not found in the list

---

## Constraints

- All three temporal fields are cleared atomically. Partial clearing of individual temporal fields is done via `SetSharedListItemTemporal` (by resetting specific fields to null one at a time).
- Clearing temporal fields does not modify importance, checked state, name, quantity, or note.
- Clearing temporal fields does not delete the item.
- After clearing, the item continues to exist in the list in base state.
- The item will no longer appear in Agenda projection after this operation.
