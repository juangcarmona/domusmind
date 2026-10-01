---
id: "BR-MEALS-AGENDA-PROJECTION"
type: "business-rule"
title: "Meal slots appear in the Agenda as non-timed household entries"
status: "draft"
applies-to:
  - "UC-MEALS-SEE-MEALS-IN-AGENDA"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-AGENDA"
  - "TERM-MEAL-SOURCE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Agenda Projection); docs/_legacy/04_contexts/meal-planning.md (Agenda Projection); docs/_legacy/00_product/surfaces/meal-planning.md (Relationship with Agenda); interview: product owner review decision (merges)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Meal slots project into the Agenda as non-timed, household-level entries that are visually distinct from Plans and Tasks. Unplanned slots without notes are not projected (observed: openspec/specs/meal-planning/spec.md, Agenda Projection). Projected meals are not calendar Plans (observed: docs/_legacy/04_contexts/meal-planning.md, Agenda Projection). That a projected meal is read-only in the Agenda and changed only in Meal Planning is BR-AGENDA-PROJECTION-READ-ONLY.

## Rationale

People see what the household eats alongside the rest of the day without meals turning into calendar items (inferred).

## Examples

Tuesday's "Recipe: lentil soup" dinner appears in Tuesday's Agenda; an Unplanned Tuesday snack with no notes does not.

## Exceptions

An Unplanned slot that has notes is still projected (observed: the exclusion covers only Unplanned slots with no notes).
