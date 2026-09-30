---
id: "FR-MEALS-AGENDA-PROJECTION"
type: "functional-requirement"
title: "Show meals in the Agenda"
status: "draft"
derived-from:
  - "UC-MEALS-SEE-MEALS-IN-AGENDA"
  - "BR-MEALS-AGENDA-PROJECTION"
  - "BR-AGENDA-PROJECTION-READ-ONLY"
verification:
  - scenario-ref: "SB-MEALS-AGENDA-SHOWS-ASSIGNED-MEAL"
  - scenario-ref: "SB-MEALS-AGENDA-HIDES-EMPTY-SLOT"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-AGENDA"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Agenda Projection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show meal slots in the Agenda as read-only, non-timed household entries distinct from Plans and Tasks, omitting Unplanned slots without notes (observed: openspec/specs/meal-planning/spec.md, Agenda Projection).

## Rationale

The Agenda is the unified daily view; meals belong in it without becoming calendar items.
