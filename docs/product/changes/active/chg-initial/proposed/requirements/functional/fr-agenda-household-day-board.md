---
id: FR-AGENDA-HOUSEHOLD-DAY-BOARD
type: functional-requirement
title: "Show the household day as a board"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "BR-AGENDA-ENTRY-PRIORITY-ORDER"
  - "BR-AGENDA-LIST-ITEMS-HOUSEHOLD-ROW"
verification:
  - scenario: "Ana has five entries today; her collapsed row shows the two highest-priority entries and \"+3\"; tapping it expands the row in place and collapses any other expanded row (observed: agenda.md, Household + Day = Board)."
  - scenario: "A list item due today appears in the shared household row, not in any person's row (observed: agenda.md, scope placement rules)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
  - "TERM-LIST-ITEM"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Household + Day = Board, Projected List Item Scope Placement Rules)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

In Household scope and Day mode the product MUST show a board: a shared row with household plans, unassigned tasks and projected list items, then one row per person, with entries in priority order. A collapsed row MUST show at most two entries and summarize the rest as "+N", expanding in place with only one row expanded at a time (observed: agenda.md).

## Rationale

The board is optimized for scanning the whole household instantly; it is not a timeline (observed: agenda.md).
