# Spec - Reorder List Items

## Purpose

Set the display order of items within a household list.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `reorder-list-items`
- Command: `ReorderSharedListItems`

---

## Inputs

Required:

- `listId`
- `orderedItemIds` — complete ordered array of item IDs representing the desired sequence

---

## Preconditions

- list must exist
- all item IDs in `orderedItemIds` must exist within the target list
- `orderedItemIds` must contain exactly the same set of items currently in the list (no additions, no omissions)

---

## Behavior

- assign each item a stable order position corresponding to its index in `orderedItemIds`
- replace the existing order in full

---

## Result

- `listId`
- confirmation that order was applied

---

## Failure

- list not found
- one or more item IDs not found in the list
- item ID set does not match the list's current item set

---

## Constraints

- Order is for human scanning convenience. It carries no semantic meaning.
- This operation must not introduce priority, urgency, or importance semantics.
- Order is not derived from any item attribute. It is entirely user-defined.
- Checked and unchecked items are part of the same ordered sequence. Display grouping (unchecked first) is a surface concern, not a model concern.
- This command does not modify any item field other than order.
- This command does not affect the list's metadata or its relationships with areas or plans.
- No cross-context side effects. Reordering does not create tasks or Agenda entries.
