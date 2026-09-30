---
id: BR-LISTS-REORDER-FULL-SET
type: business-rule
title: "Reordering replaces the whole order and carries no meaning"
status: draft
applies-to:
  - UC-LISTS-REORDER-ITEMS
uses-terms:
  - TERM-LIST-ITEM
  - TERM-ITEM-IMPORTANCE
provenance:
  source: "openspec/specs/lists/spec.md (Item Reorder); docs/_legacy/04_contexts/shared-lists.md (Invariants: order unique within a list); src/backend/DomusMind.Domain/Lists/SharedList.cs (ReorderItems)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A reorder provides the complete intended order of exactly the list's current items, with no additions, omissions or duplicates; otherwise it is rejected and the current order kept. Each position is unique within the list. Order has no priority, urgency or importance meaning; checked and unchecked items share one sequence (observed: openspec/specs/lists/spec.md, Item Reorder; enforced in src/backend/DomusMind.Domain/Lists/SharedList.cs).

## Rationale

Order is for human scanning convenience only (observed: src/backend/DomusMind.Domain/Lists/SharedList.cs, ReorderItems).

## Examples

- Reordering three items with a sequence naming only two of them is rejected.

## Exceptions

None.
