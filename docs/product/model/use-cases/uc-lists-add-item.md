---
id: UC-LISTS-ADD-ITEM
type: use-case
title: "Add an item to a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-ITEM-NAME-REQUIRED
  - BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-LIST-ITEM
  - TERM-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Item Addition); docs/_legacy/00_product/surfaces/lists.md (Quick Add); src/backend/DomusMind.Domain/Lists/SharedList.cs (AddItem); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Capture something to remember, buy or prepare in seconds.

## Trigger

A person types an item into the quick add bar of the active list.

## Preconditions

The list exists and is active.

## Main Flow

1. The person enters an item name, optionally with a quantity and a note.
2. The product appends the item, unchecked, at the end of the list and shows it immediately to the household.
3. The quick add bar is ready for the next item.

## Alternative Flows

- Sequential capture: the person repeats step 1 for several items without any interruption.

## Failure Conditions

- The name is empty: no item is added.
- The list is archived: no item is added; archived lists are read-only until restored (decided: Q-0031). The code does not enforce this yet (observed: src/backend/DomusMind.Domain/Lists/SharedList.cs).

## Postconditions

The item exists in the list, unchecked, in last position.
