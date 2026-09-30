---
id: FR-AGENDA-MONTH-VIEW
type: functional-requirement
title: "Show a month for load awareness"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-NAVIGATE"
verification:
  - scenario: "In Month mode each day cell shows its plan count or compact titles and a marker when it has tasks or routines; today is distinguished; tapping a day opens it in Day mode (observed: agenda.md, Month)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Month)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST show Month mode as a grid of weeks in which each day shows a count or compact titles of its entries (including projected list items in Household scope) and a marker for tasks or routines, today distinguished, and tapping a day opens Day mode for it. Month is for navigation and awareness, not primary editing (observed: agenda.md).

## Rationale

Gives high-level load awareness and fast date navigation (observed: agenda.md).
