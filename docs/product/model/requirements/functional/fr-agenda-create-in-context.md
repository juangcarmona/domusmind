---
id: FR-AGENDA-CREATE-IN-CONTEXT
type: functional-requirement
title: "Create items from the Agenda with context prefilled"
status: draft
derived-from:
  - "UC-AGENDA-CREATE-FROM-AGENDA"
verification:
  - scenario: "In Ana's Day timeline, clicking the empty 15:00 slot opens the add dialog prefilled with Ana, today and 15:00, all of which can be changed (observed: agenda.md, Creating From Canvas)."
  - scenario: "Pressing Add in Household scope on Friday opens the creation chooser with the household and Friday as defaults (observed: agenda.md, Creating)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Interaction Grammar: Create, Creating, Creating From Canvas, Mobile Behavior)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST offer an Add action in the Agenda (a floating button on mobile) that opens a creation chooser defaulting to the current scope and date, and clicking an empty time slot in a person's day timeline or the Week grid MUST open it prefilled with that date and time; the person MUST be able to override all defaults (observed: agenda.md). Creation MUST NOT navigate to a separate page (observed: agenda.md, Anti-Patterns).

## Rationale

Capturing in context is cheaper than remembering (inferred from the product mission).
