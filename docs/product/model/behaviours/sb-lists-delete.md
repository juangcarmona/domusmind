---
id: SB-LISTS-DELETE
type: structured-behaviour
title: "Deleting a list removes it and its items"
status: draft
illustrates:
  - UC-LISTS-DELETE-LIST
  - BR-LISTS-DELETE-PERMANENT
given:
  - "a list with items exists"
when: "a person permanently deletes the list"
then:
  - "the list no longer exists among the household's lists, active or archived"
  - "its items no longer exist"
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
provenance:
  source: "docs/_legacy/04_contexts/shared-lists.md (Commands, Domain Events: DeleteSharedList, SharedListDeleted); src/backend/DomusMind.Domain/Lists/SharedList.cs (Delete, ListDeleted); src/backend/DomusMind.Application/Features/Lists/DeleteList; src/backend/DomusMind.Api/Controllers/ListsController.cs (DELETE lists/{listId}); interview: product owner decision Q-0026 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Intent

Delete is the irreversible way to retire a list, distinct from archive.

## Boundaries

Does not cover whether the product asks for confirmation before deleting; the sources do not say.
