---
id: BR-AGENDA-DEFAULT-ENTRY-STATE
type: business-rule
title: "The Agenda opens on the household, today, by day"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-NAVIGATE"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Default Entry State, Anti-Patterns); docs/_legacy/00_product/experience.md (Agenda)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Unless a link says otherwise, the Agenda opens in Household scope, Day mode, on today's date. Month is never the default (observed: agenda.md).

## Rationale

Answers "what matters today?" without navigating anywhere (observed: agenda.md).

## Examples

- Opening DomusMind lands on today's household board (observed: agenda.md, legacy redirect from / to the Agenda).

## Exceptions

None.
