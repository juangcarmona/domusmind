---
id: BR-LISTS-ITEM-IS-NOT-A-TASK
type: business-rule
title: "A list item never becomes a task or scheduled work"
status: draft
applies-to:
  - BC-LISTS
  - UC-LISTS-SCHEDULE-ITEM
  - UC-LISTS-UPDATE-LIST
uses-terms:
  - TERM-LIST-ITEM
  - TERM-TASK
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Purpose, List Creation, Item Temporal Assignment); docs/_legacy/04_contexts/shared-lists.md (SharedListItem, Boundaries: Tasks); docs/_legacy/00_product/surfaces/lists.md (Anti-Patterns: Semantic drift)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

No action converts a list item into a task or a plan: linking the list to a plan, or giving an item a due date, reminder or repeat, leaves it a list item. No implicit task creation occurs (observed: openspec/specs/lists/spec.md, Item Temporal Assignment; docs/_legacy/04_contexts/shared-lists.md, SharedListItem).

## Rationale

Lists own capture and flexible execution, Tasks own structured execution; collapsing them would lose that distinction (observed: docs/_legacy/00_product/surfaces/lists.md, What a List Is Not; docs/_legacy/04_contexts/shared-lists-item-model.md, What This Model Does NOT Include).

## Examples

- A packing list linked to a trip plan keeps its items as list items.
- "Buy birthday card" with a due date shows in the Agenda as a list item, not a task.

## Exceptions

None.
