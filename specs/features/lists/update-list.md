# Spec - Update List

## Purpose

Update the metadata of an existing household list.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `update-list`
- Command: `UpdateSharedList`

---

## Inputs

Required:

- `listId`

At least one of the following must be provided:

- `name`
- `areaId` (pass null to remove the area association)
- `linkedPlanId` (pass null to remove the plan link)
- `kind`

---

## Preconditions

- list must exist
- if `name` is provided, it must be non-empty

---

## Behavior

- apply the provided field updates to the list
- fields not included in the request are left unchanged
- `areaId` and `linkedPlanId` may be cleared by passing explicit null

---

## Result

- `listId`
- `name`
- `areaId` (current value or null)
- `linkedPlanId` (current value or null)
- `kind` (current value or null)

---

## Failure

- list not found
- name provided but empty

---

## Constraints

- Updating metadata does not affect list semantics. A list linked to a plan or area remains a list.
- Linking or unlinking a plan does not create or remove Agenda entries. No scheduling behavior is introduced.
- Linking or unlinking an area does not create or remove tasks or responsibilities. No execution behavior is introduced.
- No new list-level fields may be introduced through this command.
- This command does not affect items in the list.
