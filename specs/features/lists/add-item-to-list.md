# Spec - Add Item to List

## Purpose

Add a new item to an existing household list.

---

## Context

- Module: Lists
- Aggregate: `List`
- Slice: `add-item-to-list`
- Command: `AddItemToList`

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
- Quick add requires only `name`. Extended capabilities (importance, temporal fields) are set via dedicated commands after creation.
- Items are not tasks. Adding an item to a list does not create work in the Tasks context.
- Assignment is not a list item field and must never be introduced here.
