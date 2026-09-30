---
id: FR-AGENDA-SCOPE-AND-MODE-SWITCHING
type: functional-requirement
title: "Switch Agenda scope, mode and date"
status: draft
derived-from:
  - "UC-AGENDA-NAVIGATE"
  - "BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT"
  - "BR-AGENDA-WEEK-STARTS-ON-HOUSEHOLD-FIRST-DAY"
verification:
  - scenario: "In Household Week for next week, a member taps Ana; the Agenda shows Ana's Week for the same dates (observed: agenda.md)."
  - scenario: "In Month mode, a member taps the 14th; the Agenda shows Day mode for the 14th in the same scope (observed: agenda.md, Month)."
  - scenario: "On a phone, the scope selector and mode toggle are reachable without scrolling or an extra tap (observed: agenda.md, Header, Anti-Patterns)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Header, Default Entry State, Month, Mobile Behavior, Anti-Patterns)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST offer, always reachable without scrolling, a scope selector (Household or any person), a Day/Week/Month mode toggle, previous/today/next navigation and a label for the current day, date or range. Switching scope MUST keep mode and date; switching mode MUST keep scope; the week MUST start on the household's configured first day of week (observed: agenda.md).

## Rationale

One surface replaces the former Today, Planning and Member Agenda surfaces, so movement within it must be cheap (observed: agenda.md, What This Surface Replaces).
