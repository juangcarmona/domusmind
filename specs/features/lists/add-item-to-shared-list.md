# Spec - Add Item to List

## Purpose

Add a new item to an existing household list.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `add-item-to-shared-list`
- Command: `AddItemToSharedList`

---

## Inputs

Required:

- `listId`
- `name`

Optional:

- `quantity`
- `note`

---

## Preconditions

- list must exist
- name must be non-empty

---

## Behavior

- append a new item to the list
- assign the next stable order position
- initialize item state as unchecked

---

## Result

- `itemId`
- `name`
- `checked` (false)
- `order`

---

## Failure

- list not found
- name is empty or invalid

---

## Constraints

- This is the critical-path interaction for list usage. Validation must be minimal.
- Items are appended in stable order. There is no priority ordering or sorting by external attributes.
- A newly added item is always unchecked. There is no initial state other than unchecked.
- Items must not carry due dates, reminders, assignees, priority, or recurrence. Name is the only required field.
- Items are not tasks. Adding an item to a list does not create work in the Tasks context.
