---
id: "FR-MEALS-CREATE-PLAN"
type: "functional-requirement"
title: "Create a weekly meal plan"
status: "draft"
derived-from:
  - "UC-MEALS-CREATE-PLAN"
  - "BR-MEALS-FULL-SLOT-GRID"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
  - "BR-MEALS-WEEK-DEFINITION"
verification:
  - scenario-ref: "SB-MEALS-CREATE-PLAN-FOR-WEEK"
  - scenario-ref: "SB-MEALS-CREATE-PLAN-EXISTING-WEEK"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Meal Plan Creation); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household create a meal plan for any calendar week. The plan MUST cover seven days from a week start aligned with the household's first day of week, MUST start in Draft with one Unplanned slot for every day and meal type, and MUST return the existing plan instead of creating a second one for the same week (observed: openspec/specs/meal-planning/spec.md, Meal Plan Creation).

## Rationale

The week grid is the unit of meal coordination and must always exist in full.
