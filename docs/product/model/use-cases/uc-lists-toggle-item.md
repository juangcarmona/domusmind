---
id: UC-LISTS-TOGGLE-ITEM
type: use-case
title: "Check or uncheck an item"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-TOGGLE-PRESERVES-ITEM
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-CHECKED-ITEM
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Toggle); docs/_legacy/00_product/surfaces/lists.md (Row Model: toggle is immediate); src/backend/DomusMind.Domain/Lists/SharedList.cs (ToggleItem); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Mark an item as handled for this use, or relevant again for the next.

## Trigger

A person taps the toggle on an item row or in the inspector.

## Preconditions

The item exists.

## Main Flow

1. The person toggles the item.
2. The product flips it between unchecked and checked immediately and shares the change with the household.

## Alternative Flows

- Unchecking a checked item makes it relevant again for the next use.

## Failure Conditions

- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

The item is in its new checked state and remains in the list with all other fields intact.
