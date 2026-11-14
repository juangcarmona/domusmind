---
id: LC-LIST
type: domain-lifecycle
title: List lifecycle
status: active
subject: TERM-LIST
states:
  - id: ACTIVE
    title: Active
    initial: true
  - id: ARCHIVED
    title: Archived
  - id: DELETED
    title: Deleted
    terminal: true
transitions:
  - id: ARCHIVE
    title: Archive a list
    from:
      - ACTIVE
    to: ARCHIVED
    trigger: A member archives the list
    initiated-by: &a1
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-LISTS-ARCHIVE-STATE
    realized-by:
      - UC-LISTS-ARCHIVE-LIST
  - id: RESTORE
    title: Restore an archived list
    from:
      - ARCHIVED
    to: ACTIVE
    trigger: A member restores the list
    initiated-by: *a1
    governed-by:
      - BR-LISTS-ARCHIVE-STATE
    realized-by:
      - UC-LISTS-RESTORE-LIST
  - id: DELETE
    title: Permanently delete a list
    from:
      - ACTIVE
      - ARCHIVED
    to: DELETED
    trigger: A member permanently deletes the list
    initiated-by: *a1
    governed-by:
      - BR-LISTS-DELETE-PERMANENT
    realized-by:
      - UC-LISTS-DELETE-LIST
provenance:
  source: TERM-LIST; BR-LISTS-ARCHIVE-STATE; BR-LISTS-ARCHIVED-READ-ONLY; BR-LISTS-DELETE-PERMANENT; src/backend/DomusMind.Domain/Lists/SharedList.cs; product owner decisions Q-0026 and Q-0031
  confidence: high
  recovered-from: documentation
---

## Purpose

Distinguish a reusable checklist from one put away for later or removed permanently.

## Invariants

The List belongs to one household. Archiving and restoring preserve every item and its fields. Archived Lists are read-only. Deleting removes the List and all its items.

## State Semantics

Active Lists are available for use. Archived Lists keep their data for restoration. Deleted Lists no longer exist and cannot be restored; Deleted is the outcome of removal, not a persisted status.

## Transition Semantics

Archive and restore are reversible. Deletion is permanent from either existing state. ListArchived, ListRestored and ListDeleted record these changes in the domain.

## Boundaries

Checking an item changes the item and leaves the List active. A link to an Area or Plan does not transfer ownership or alter that concept when the List is archived.
