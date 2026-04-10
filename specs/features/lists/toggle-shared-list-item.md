# Spec - Toggle List Item

## Purpose

Toggle the checked state of an item in a household list.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `toggle-shared-list-item`
- Command: `ToggleSharedListItem`

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

- toggle the checked state of the target item:
  - unchecked → checked: item has been handled for this use of the list
  - checked → unchecked: item is relevant again for the next use of the list
- record the timestamp of the state change

---

## Result

- `itemId`
- `checked`
- `updatedAtUtc`
- `uncheckedCount` — updated count of unchecked items in the list

---

## Failure

- list not found
- item not found in the list

---

## Constraints

- Toggle is binary. There are no intermediate states, workflows, or transitions.
- Checking an item does not remove it. Items remain in the list after being checked.
- Toggle has no side effects outside the list. It does not create tasks or emit cross-context events.
- A checked item with temporal fields remains valid and continues to project into Agenda in a de-emphasized state. Toggle does not clear temporal fields.
- Toggle does not add or remove importance or temporal data. Use dedicated commands for those.