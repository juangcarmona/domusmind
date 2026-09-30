---
id: FR-AGENDA-DEFAULT-ENTRY
type: functional-requirement
title: "Open the Agenda on today's household day"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "BR-AGENDA-DEFAULT-ENTRY-STATE"
verification:
  - scenario: "A member opens DomusMind with no specific link and sees the Agenda in Household scope, Day mode, for today (observed: agenda.md, Default Entry State)."
  - scenario: "A member follows an old Today or Planning link and lands on the Agenda in Household Day, or Household Week respectively (observed: agenda.md, Legacy Route Redirects)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Default Entry State, Legacy Route Redirects)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST open the Agenda in Household scope, Day mode, on today's date unless a link specifies another state, and MUST NOT default to Month (observed: agenda.md).

## Rationale

The first screen answers "what matters today?" (observed: agenda.md).
