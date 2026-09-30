---
id: FR-AGENDA-LINKABLE-STATE
type: functional-requirement
title: "Link directly to an Agenda scope, mode and date"
status: draft
derived-from:
  - "UC-AGENDA-NAVIGATE"
verification:
  - scenario: "A link to Ana's Agenda in Week mode opens Member scope for Ana in Week mode for the current week (observed: agenda.md, Entry Points)."
  - scenario: "From Areas, selecting an area owner opens that person's Agenda (observed: agenda.md, Click Semantics)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Entry Points, Click Semantics)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST let other surfaces and links open the Agenda directly at a given scope (household or a person), mode and date, and SHOULD keep older Today and Planning links working by opening the equivalent Agenda state (observed: agenda.md).

## Rationale

People reach the Agenda from many places (dates, people, areas) and should land in the right context (observed: agenda.md).
