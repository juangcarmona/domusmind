---
id: UC-LISTS-CLEAR-ITEM-SCHEDULE
type: use-case
title: "Remove all timing from an item"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT
  - BR-LISTS-AGENDA-PROJECTION
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Temporal Clearing); docs/_legacy/04_contexts/shared-lists-item-model.md (Clearing temporal fields); src/backend/DomusMind.Domain/Lists/SharedList.cs (ClearItemTemporal); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Take an item out of the Agenda while keeping it in the list.

## Trigger

A person clears the item's timing.

## Preconditions

The item exists.

## Main Flow

1. The person clears the item's timing.
2. The product removes due date, reminder and repeat together.
3. The item no longer appears in the Agenda.

## Alternative Flows

- The item has no timing: the product succeeds without change.

## Failure Conditions

- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

The item has no temporal fields and remains in the list with all other fields unchanged.
