---
id: FR-LISTS-AGENDA-PROJECTION
type: functional-requirement
title: "Show time-aware list items in the Agenda"
status: draft
derived-from:
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - BR-LISTS-AGENDA-PROJECTION
  - BR-AGENDA-PROJECTION-READ-ONLY
verification:
  - scenario-ref: "SB-LISTS-DUE-TODAY-IN-AGENDA"
  - scenario-ref: "SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA"
  - scenario-ref: "SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA"
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
  - TERM-AGENDA
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Agenda Projection); docs/_legacy/04_contexts/shared-lists-item-model.md (Projection Rules)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show a list item in the Agenda for a period when its due date or reminder falls in the period, or its repeat rule produces an occurrence in it; each condition alone MUST suffice. Projected items MUST be read-only in the Agenda, carry a visible list-origin cue, and be distinct from tasks and plans. Checked items meeting a condition MUST still appear, de-emphasised, until their timing is cleared.

## Rationale

The Agenda is the one place where everything time-relevant in the household appears together.
