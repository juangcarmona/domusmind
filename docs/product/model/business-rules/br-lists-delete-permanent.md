---
id: BR-LISTS-DELETE-PERMANENT
type: business-rule
title: "Deleting a list is permanent and removes its items"
status: draft
applies-to:
  - UC-LISTS-DELETE-LIST
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-ARCHIVED-LIST
provenance:
  source: "docs/_legacy/04_contexts/shared-lists.md (Commands, Domain Events: DeleteSharedList, SharedListDeleted); src/backend/DomusMind.Domain/Lists/SharedList.cs (Delete, ListDeleted); src/backend/DomusMind.Application/Features/Lists/DeleteList; src/backend/DomusMind.Api/Controllers/ListsController.cs (DELETE lists/{listId}); interview: product owner decision Q-0026 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Rule

A household can retire a list in two ways: archive it (reversible, see BR-LISTS-ARCHIVE-STATE) or permanently delete it (decided: Q-0026). Deleting a list removes the list together with all its items; a deleted list cannot be restored (observed: src/backend/DomusMind.Domain/Lists/SharedList.cs, Delete; src/backend/DomusMind.Application/Features/Lists/DeleteList).

## Rationale

Archive keeps a list for later reuse; delete is for lists the household will not use again.

## Examples

- A one-off moving checklist is deleted once the move is over; it and its items are gone.
- A holiday packing list is archived instead, so it can be restored next summer.

## Exceptions

None stated in the sources.
