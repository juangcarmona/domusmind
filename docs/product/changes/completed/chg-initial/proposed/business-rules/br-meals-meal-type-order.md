---
id: "BR-MEALS-MEAL-TYPE-ORDER"
type: "business-rule"
title: "Meal plan views follow day order then meal-type order"
status: "draft"
applies-to:
  - "UC-MEALS-VIEW-PLAN"
uses-terms:
  - "TERM-MEAL-TYPE"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Viewing); docs/_legacy/04_contexts/meal-planning.md (Meal Type Taxonomy, MealPlanDetail)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A meal plan is presented day by day starting from the household's first day of week, and within a day by meal type in the fixed order Breakfast, Mid-morning snack, Lunch, Afternoon snack, Dinner, including Unplanned slots (observed: openspec/specs/meal-planning/spec.md, Meal Plan Viewing; docs/_legacy/04_contexts/meal-planning.md, Meal Type Taxonomy).

## Rationale

A dense, predictable grid is scannable in seconds (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Success Criteria).

## Examples

Wednesday's lunch always appears between Wednesday's mid-morning snack and afternoon snack.

## Exceptions

None.
