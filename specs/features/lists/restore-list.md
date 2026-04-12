# Spec - Restore List

## Purpose

Return an archived household list to active use.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `restore-list`
- Command: `RestoreSharedList`

---

## Inputs

Required:

- `listId`

---

## Preconditions

- list must exist
- list must be in archived state

---

## Behavior

- transition the list from archived to active state
- list becomes visible again in default active list queries

---

## Result

- `listId`

---

## Failure

- list not found
- list is not archived

---

## Constraints

- Restore is the symmetric inverse of archive. No additional behavior is introduced.
- No item data is modified. Items retain the state they had when the list was archived.
- Restore does not affect linked entities. A linked plan or area is not modified.
- Restore does not emit behavior into other contexts. No tasks are created. No Agenda entries are affected.
- Restored lists behave identically to any other active list.
