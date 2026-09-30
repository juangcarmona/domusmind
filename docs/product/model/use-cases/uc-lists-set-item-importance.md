---
id: UC-LISTS-SET-ITEM-IMPORTANCE
type: use-case
title: "Star or unstar an item"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-IMPORTANCE-BINARY
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-ITEM-IMPORTANCE
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Importance); docs/_legacy/00_product/surfaces/lists.md (Inspector: Importance); src/backend/DomusMind.Domain/Lists/SharedList.cs (SetItemImportance); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Make an item stand out as needing attention.

## Trigger

A person taps the star of an item.

## Preconditions

The item exists.

## Main Flow

1. The person marks or unmarks the item as important.
2. The product shows the item as starred or not starred.

## Alternative Flows

- The item is already in the requested state: the product succeeds without change.

## Failure Conditions

- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

The item's importance is as requested; nothing else changed.
