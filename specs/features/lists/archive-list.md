# Spec - Archive List

## Purpose

Move a household list out of active use.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `archive-list`
- Command: `ArchiveSharedList`

---

## Inputs

Required:

- `listId`

---

## Preconditions

- list must exist
- list must be in active state (not already archived)

---

## Behavior

- transition the list to archived state
- list is removed from the default active list collection

---

## Result

- `listId`
- `archivedAtUtc`

---

## Failure

- list not found
- list is already archived

---

## Constraints

- Archive does not delete the list. All data is preserved.
- All items remain unchanged, including their names, quantities, notes, and checked states.
- Archive does not affect linked entities. A linked plan or area is not modified.
- Archive does not emit behavior into other contexts. No tasks are created or removed. No Agenda entries are affected.
- Archived lists must not appear in `GetFamilySharedLists` results unless the query explicitly requests archived lists.
- This operation aligns with the list lifecycle: created → used → active → rested → archived.
