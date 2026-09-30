---
id: "FR-MEALS-VIEW-PLAN"
type: "functional-requirement"
title: "View a week's meal plan"
status: "draft"
derived-from:
  - "UC-MEALS-VIEW-PLAN"
  - "BR-MEALS-MEAL-TYPE-ORDER"
verification:
  - scenario-ref: "SB-MEALS-VIEW-WEEK-ORDERED"
  - scenario-ref: "SB-MEALS-VIEW-RECIPE-SLOT-DETAILS"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Meal Plan Viewing)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show the full detail of a meal plan for any week, found by week or by plan: every slot including Unplanned ones, ordered by day from the household's first day then by meal type, with the recipe name, servings and times shown for recipe slots (observed: openspec/specs/meal-planning/spec.md, Meal Plan Viewing).

## Rationale

Answers "what are we eating this week, and what is missing" at a glance.
