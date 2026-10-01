---
id: "BR-MEALS-TEMPLATE-SNAPSHOT"
type: "business-rule"
title: "Applying a template copies it as a snapshot"
status: "draft"
applies-to:
  - "UC-MEALS-APPLY-TEMPLATE"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-MEAL-PLAN"
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Apply Weekly Template); docs/_legacy/04_contexts/meal-planning.md (Commands: ApplyWeeklyTemplate); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (CreateFromTemplate)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Applying a template creates a new Draft plan whose slots are copied from the template, with uncovered slots Unplanned, and records which template was used; later changes to the template never affect plans already created. Every recipe the template references must still exist at application time, otherwise nothing is created. Slots of the resulting plan remain individually editable (observed: openspec/specs/meal-planning/spec.md, Apply Weekly Template).

## Rationale

Reuse must be faster than creation while keeping each week independent (observed: docs/_legacy/00_product/surfaces/meal-planning.md, UX Rules).

## Examples

Editing the "School week" template on Wednesday does not change the plan created from it on Monday.

## Exceptions

None.
