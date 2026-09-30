---
id: UC-LISTS-DELETE-LIST
type: use-case
title: "Permanently delete a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-DELETE-PERMANENT
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-ARCHIVED-LIST
provenance:
  source: "docs/_legacy/04_contexts/shared-lists.md (Commands, Domain Events: DeleteSharedList, SharedListDeleted); src/backend/DomusMind.Domain/Lists/SharedList.cs (Delete, ListDeleted); src/backend/DomusMind.Application/Features/Lists/DeleteList; src/backend/DomusMind.Api/Controllers/ListsController.cs (DELETE lists/{listId}); interview: product owner decision Q-0026 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Goal

Remove a list the household will never need again, instead of only putting it away.

## Trigger

A person decides a list should be removed for good.

## Preconditions

The list exists.

## Main Flow

1. The person deletes the list.
2. The product removes the list and all its items permanently (observed: src/backend/DomusMind.Application/Features/Lists/DeleteList).

## Alternative Flows

None.

## Failure Conditions

- The list does not exist: the delete is rejected (observed: src/backend/DomusMind.Application/Features/Lists/DeleteList).

## Postconditions

The list and its items no longer exist and cannot be restored. Permanently deleting a list is current product, alongside archive and restore (decided: Q-0026).
