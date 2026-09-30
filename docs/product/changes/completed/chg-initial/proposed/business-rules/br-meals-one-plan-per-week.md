---
id: "BR-MEALS-ONE-PLAN-PER-WEEK"
type: "business-rule"
title: "A household has at most one meal plan per week"
status: "draft"
applies-to:
  - "UC-MEALS-CREATE-PLAN"
  - "UC-MEALS-APPLY-TEMPLATE"
  - "UC-MEALS-COPY-PREVIOUS-WEEK"
  - "UC-MEALS-PROMOTE-PLAN"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-HOUSEHOLD"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Creation, Apply Weekly Template, Copy from Previous Week); docs/_legacy/04_contexts/meal-planning.md (MealPlan invariants); interview: product owner decision Q-0034 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A household has at most one meal plan per week, of any status (decided: Q-0034), and any attempt to create a plan (from scratch, from a template, or by copying) for a week that already has a plan returns the existing plan instead of creating another (observed: openspec/specs/meal-planning/spec.md, Meal Plan Creation and scenarios "A meal plan already exists for the target week").

## Rationale

The household needs one unambiguous answer to "what are we eating this week?" (inferred).

## Examples

A household that already started a Draft for next week and then chooses "Copy previous week" gets its existing Draft back, with a notice that the week already has a plan (observed: docs/_legacy/00_product/surfaces/meal-planning.md, state 5).

## Exceptions

None. A Draft and an Active plan can never coexist for the same week (decided: Q-0034).
