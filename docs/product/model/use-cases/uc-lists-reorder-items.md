---
id: UC-LISTS-REORDER-ITEMS
type: use-case
title: "Reorder a list's items"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-REORDER-FULL-SET
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Reorder); src/backend/DomusMind.Domain/Lists/SharedList.cs (ReorderItems); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Arrange items in the order that is easiest to scan.

## Trigger

A person rearranges items in the list.

## Preconditions

The list has items.

## Main Flow

1. The person sets a new order for the list's items.
2. The product assigns each item its new position.

## Alternative Flows

None.

## Failure Conditions

- The new order does not contain exactly the list's current items: the reorder is rejected and the previous order kept.
- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

Items appear in the new order; no other field changed.
