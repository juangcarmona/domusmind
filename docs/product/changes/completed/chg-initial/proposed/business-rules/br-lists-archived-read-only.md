---
id: BR-LISTS-ARCHIVED-READ-ONLY
type: business-rule
title: "Archived lists are read-only until restored"
status: draft
applies-to:
  - UC-LISTS-ADD-ITEM
  - UC-LISTS-TOGGLE-ITEM
  - UC-LISTS-UPDATE-ITEM
  - UC-LISTS-SET-ITEM-IMPORTANCE
  - UC-LISTS-SCHEDULE-ITEM
  - UC-LISTS-CLEAR-ITEM-SCHEDULE
  - UC-LISTS-REORDER-ITEMS
  - UC-LISTS-REMOVE-ITEM
  - UC-LISTS-RESTORE-LIST
uses-terms:
  - TERM-ARCHIVED-LIST
  - TERM-LIST-ITEM
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Addition, 'add a new item to an active list'); src/backend/DomusMind.Domain/Lists/SharedList.cs (no archived check on item operations); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Rule

An archived list is read-only: no item can be added, checked or unchecked, or edited until the list is restored (decided: Q-0031; observed: openspec/specs/lists/spec.md, Item Addition, adding only to an active list). Once restored, the list accepts item changes again like any other active list.

The current code does not check the archived state on item operations (observed: src/backend/DomusMind.Domain/Lists/SharedList.cs); the implementation does not yet meet this rule. Whether list-level changes other than restore (such as renaming an archived list or deleting it) are also blocked is not stated by the decision (inferred: gap).

## Rationale

An archived list is put away with its data intact; keeping it read-only protects the state it will be restored to.

## Examples

- Adding "sunscreen" to an archived packing list is rejected; after restoring the list it can be added.
- Checking an item on an archived list is rejected.

## Exceptions

None.
