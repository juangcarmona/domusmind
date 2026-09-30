---
id: UC-LISTS-REMOVE-ITEM
type: use-case
title: "Remove an item from a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by: []
uses-terms:
  - TERM-LIST-ITEM
  - TERM-PROJECTED-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Removal); docs/_legacy/00_product/surfaces/lists.md (Inspector: Actions); src/backend/DomusMind.Domain/Lists/SharedList.cs (RemoveItem); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Get rid of an item that no longer belongs in the list.

## Trigger

A person chooses remove in the item inspector.

## Preconditions

The item exists.

## Main Flow

1. The person removes the item.
2. The product deletes it permanently from the list; if it had temporal fields it also disappears from the Agenda.

## Alternative Flows

None.

## Failure Conditions

- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

The item is gone; the remaining items are unchanged. Removal cannot be undone.
