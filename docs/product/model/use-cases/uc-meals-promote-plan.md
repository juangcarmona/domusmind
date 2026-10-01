---
id: "UC-MEALS-PROMOTE-PLAN"
type: "use-case"
title: "Make a meal plan the household's working plan"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-PLAN-LIFECYCLE"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Lifecycle, Notes: Active plan promotion timing); docs/_legacy/04_contexts/meal-planning.md (Plan Lifecycle); interview: product owner decision Q-0033 (E-0158); interview: product owner decision Q-0034 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Mark a prepared meal plan as the household's working plan for its week.

## Trigger

A person decides the Draft plan for a week is ready.

## Preconditions

A Draft meal plan exists for the week.

## Main Flow

1. The person promotes the Draft plan.
2. The product makes it the Active plan for that household and week.

## Alternative Flows

None. Activation is an explicit household action only; there is no automatic promotion (decided: Q-0033).

## Failure Conditions

None stated. A week holds at most one meal plan, so no other plan can compete for the week (decided: Q-0034).

## Postconditions

The plan is Active. No activation capability exists in the domain code or the legacy surface today (observed: src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs; docs/_legacy/00_product/surfaces/meal-planning.md names no promote action), so this use case is medium confidence.
