---
id: FR-AGENDA-WEEK-VIEW
type: functional-requirement
title: "Show a seven-day coordination week"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "BR-AGENDA-WEEK-STARTS-ON-HOUSEHOLD-FIRST-DAY"
verification:
  - scenario: "In Household Week a day with many plans and tasks is visibly heavier than the others (observed: agenda.md, Week, \"overloaded days should be visually scannable\")."
  - scenario: "On a phone in Week mode, tapping a date in the date strip opens Day mode for that date (observed: agenda.md, Mobile Week)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Week)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST show Week mode as seven day columns starting on the household's first day of week, with timed plans as positioned blocks, untimed plans in a day lane, routines and tasks in compact lanes and projected list items in a household lane (Household scope) or the task lane (Member scope); overloaded days MUST be scannable (observed: agenda.md).

## Rationale

Week is the default coordination view when a day is not enough (observed: agenda.md).
