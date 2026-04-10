# Spec - Update List Item

## Purpose

Update the editable fields of an item in a household list.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `update-list-item`
- Command: `UpdateSharedListItem`

---

## Inputs

Required:

- `listId`
- `itemId`

At least one of the following must be provided:

- `name`
- `quantity`
- `note`

---

## Preconditions

- list must exist
- item must exist within the target list
- if `name` is provided, it must be non-empty

---

## Behavior

- apply the provided field updates to the target item
- fields not included in the request are left unchanged
- `quantity` may be cleared by passing an explicit null or empty value

---

## Result

- `itemId`
- `name`
- `quantity` (current value or null)
- `note` (current value or null)
- `checked` (unchanged)

---

## Failure

- list not found
- item not found in the list
- name provided but empty

---

## Constraints

- Only name, quantity, and note are editable through this command. These are the full item model.
- Checked state is not modified by this command. Use `ToggleSharedListItem` for state changes.
- Order is not modified by this command.
- No new fields may be introduced here. Due dates, reminders, assignees, priority, and recurrence must not be added.
- This command does not affect the list's relationship with Agenda or Areas.
