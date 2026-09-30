---
id: "BR-MEALS-PLAN-LIFECYCLE"
type: "business-rule"
title: "Meal plans move from Draft to Active to Completed"
status: "draft"
applies-to:
  - "UC-MEALS-CREATE-PLAN"
  - "UC-MEALS-PROMOTE-PLAN"
  - "UC-MEALS-UPDATE-SLOT"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Lifecycle, Notes); docs/_legacy/04_contexts/meal-planning.md (Plan Lifecycle, MealPlan invariants); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs; interview: product owner decision Q-0032 (E-0158); interview: product owner decision Q-0033 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

A new meal plan (from scratch, from a template or copied) starts in Draft. A plan becomes the household's working plan only once the household explicitly activates it (decided: Q-0033). A Completed plan is read-only and refuses every change. Draft and Active plans both accept slot changes (decided: Q-0032; observed: openspec/specs/meal-planning/spec.md, Meal Plan Lifecycle, Notes "Resolved: Mutation on Draft vs. Active plans"; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs rejects changes only on Completed).

## Rationale

Households prepare next week's plan before it starts, then treat it as the working plan; past weeks remain as history (inferred from docs/_legacy/04_contexts/meal-planning.md, Plan Lifecycle).

## Examples

A household fills a Draft for next week over several days and promotes it on Sunday evening (inferred).

## Exceptions

Undecided, deferred (Q-0033): what completes a plan (the trigger for Completed) is left to the Meal Planning refinement change. Activation is explicit only; the legacy idea of automatic promotion "when the week begins" is not current product (decided: Q-0033). The domain code contains no promotion or completion behaviour today (observed: src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs).
