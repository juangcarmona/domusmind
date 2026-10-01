---
id: BR-LISTS-AGENDA-PROJECTION
type: business-rule
title: "List items with temporal fields appear in the Agenda"
status: draft
applies-to:
  - BC-LISTS
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - UC-LISTS-SCHEDULE-ITEM
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-AGENDA
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Agenda Projection, Item Toggle, Item Removal); docs/_legacy/04_contexts/shared-lists-item-model.md (Projection Rules, Projection invalidation, Checked item projection); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A list item appears in the Agenda for a date window when its due date falls in the window, or its reminder falls in the window, or its repeat rule produces an occurrence in the window; each condition alone is sufficient. Items without temporal fields never appear (observed: openspec/specs/lists/spec.md, Agenda Projection). Checked items that meet a condition still appear, de-emphasised. An item leaves the Agenda when its temporal fields are cleared, when it is removed, or when its dates fall outside the window (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Projection invalidation).

## Rationale

Temporal list items are first-class Agenda sources so the household sees everything time-relevant in one place (observed: docs/_legacy/00_product/surfaces/lists.md, Relationship with Agenda).

## Examples

- "Return library books" due Friday shows in Friday's Agenda with its list name.
- After checking it, it still shows on Friday, de-emphasised.

## Exceptions

Only temporally-enriched items project; showing all list items in the Agenda is rejected (observed: docs/_legacy/00_product/surfaces/lists.md, Anti-Patterns).
