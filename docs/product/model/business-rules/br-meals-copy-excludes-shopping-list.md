---
id: "BR-MEALS-COPY-EXCLUDES-SHOPPING-LIST"
type: "business-rule"
title: "Copying a week never carries over its shopping list"
status: "draft"
applies-to:
  - "UC-MEALS-COPY-PREVIOUS-WEEK"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-SHOPPING-LIST-DERIVATION"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Copy from Previous Week); docs/_legacy/04_contexts/meal-planning.md (Commands: CopyMealPlanFromPreviousWeek); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (CopyFromPlan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Copying a previous plan carries over slot sources, recipe references, free text, notes and flags, but never the shopping list reference: derivation starts fresh for the new week (observed: openspec/specs/meal-planning/spec.md, Copy from Previous Week).

## Rationale

Last week's list reflects last week's purchases (inferred).

## Examples

After copying last week, "Generate shopping list" is available again for the new week.

## Exceptions

None.
