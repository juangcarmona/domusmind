---
id: FR-LISTS-DELETE-LIST
type: functional-requirement
title: "Permanently delete a list"
status: draft
derived-from:
  - UC-LISTS-DELETE-LIST
  - BR-LISTS-DELETE-PERMANENT
verification:
  - scenario-ref: "SB-LISTS-DELETE"
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
provenance:
  source: "docs/_legacy/04_contexts/shared-lists.md (Commands, Domain Events: DeleteSharedList, SharedListDeleted); src/backend/DomusMind.Domain/Lists/SharedList.cs (Delete, ListDeleted); src/backend/DomusMind.Application/Features/Lists/DeleteList; src/backend/DomusMind.Api/Controllers/ListsController.cs (DELETE lists/{listId}); interview: product owner decision Q-0026 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Requirement

The product MUST let a household permanently delete a list, in addition to archiving and restoring it (decided: Q-0026). Deleting MUST remove the list and all its items, and a deleted list MUST NOT be restorable.

## Rationale

Households need to get rid of lists they will never reuse, not only put them away.
