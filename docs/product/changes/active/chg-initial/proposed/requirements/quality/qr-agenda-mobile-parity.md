---
id: QR-AGENDA-MOBILE-PARITY
type: quality-requirement
title: "The Agenda on mobile is the same product"
status: draft
quality-attribute: "usability"
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-AGENDA-NAVIGATE"
  - "UC-AGENDA-INSPECT-ENTRY"
  - "UC-AGENDA-CREATE-FROM-AGENDA"
verification:
  - scenario: "On a phone-width screen, a member switches scope, switches among Day, Week and Month, inspects an entry in a bottom sheet and creates a plan through the floating Add button, without horizontal scrolling in any standard view (observed: agenda.md, Mobile Behavior)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Mobile Behavior, Shell, Anti-Patterns)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The Agenda on mobile MUST preserve full scope switching, full mode switching and full create, inspect and edit capability, and MUST NOT require horizontal scrolling in its standard views; the scope selector MUST NOT be hidden behind an extra tap (observed: agenda.md, Mobile Behavior, Anti-Patterns).

## Measurement

Walk through every capability of FR-AGENDA-SCOPE-AND-MODE-SWITCHING, FR-AGENDA-INSPECT-ENTRY and FR-AGENDA-CREATE-IN-CONTEXT at phone width: each must be reachable (scope selector visible without an extra tap) and no standard Day, Week or Month view may scroll horizontally. Pass = 100% of capabilities available and zero horizontal scrolling (inferred measurement; the source states the obligation, not the method).
