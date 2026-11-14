---
id: BR-MEALS-PLAN-LIFECYCLE
type: business-rule
title: Meal plans move from Draft to Active to Completed
status: active
uses-terms:
  - TERM-MEAL-PLAN
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: 'openspec/specs/meal-planning/spec.md (Meal Plan Lifecycle, Notes); docs/_legacy/04_contexts/meal-planning.md (Plan Lifecycle, MealPlan invariants); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs; interview: product owner decision Q-0032 (E-0158); interview: product owner decision Q-0033 (E-0158); product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: medium
  recovered-from: documentation
---

## Rule

A new meal plan (from scratch, from a template or copied) starts in Draft. A plan becomes the household's working plan only once the household explicitly activates it (decided: Q-0033). An Active plan becomes Completed only when a household member explicitly chooses Finish week. Completion MUST NOT happen automatically when the week changes. A Completed plan is read-only, refuses every change and MUST NOT reopen. Draft and Active plans both accept slot changes (decided: Q-0032; observed: openspec/specs/meal-planning/spec.md, Meal Plan Lifecycle, Notes "Resolved: Mutation on Draft vs. Active plans"; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs rejects changes only on Completed).

## Rationale

Households prepare next week's plan before it starts, then treat it as the working plan; past weeks remain as history (inferred from docs/_legacy/04_contexts/meal-planning.md, Plan Lifecycle).

## Examples

A household fills a Draft for next week over several days and promotes it on Sunday evening (inferred).

## Exceptions

The owner resolved the previously deferred completion trigger on 2026-11-14: Finish week is manual and Completed is final (CHG-STATE-MODEL-COMPLETION). Activation remains explicit under Q-0033. The inspected domain class still lacks activation and completion commands.
