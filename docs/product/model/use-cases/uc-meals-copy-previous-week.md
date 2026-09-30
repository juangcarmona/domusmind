---
id: "UC-MEALS-COPY-PREVIOUS-WEEK"
type: "use-case"
title: "Plan a week by copying the previous one"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-COPY-EXCLUDES-SHOPPING-LIST"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
  - "BR-MEALS-PLAN-LIFECYCLE"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Copy from Previous Week); docs/_legacy/00_product/surfaces/meal-planning.md (Header, states 2, 4, 5); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (CopyFromPlan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Reuse last week's meals for a new week with one action.

## Trigger

A person chooses "Copy previous week" for a week (observed: surface, Header and state 2).

## Preconditions

The person belongs to a household.

## Main Flow

1. The person chooses to copy the previous week for the target week.
2. The product takes the plan of the immediately preceding week (or an explicitly chosen plan).
3. The product creates a Draft plan for the target week with the same slot sources, recipes, free text, notes and flags, and no shopping list.
4. The product shows the new plan.

## Alternative Flows

- 2a. There is no plan for the previous week: the product reports "No meal plan found for the previous week." as a recoverable notice; the person can start from scratch or apply a template.
- 3a. The target week already has a plan: the product shows the existing plan with a notice that the week already has a meal plan.

## Failure Conditions

- The action fails for another reason: a generic inline notice appears; the page stays as it was.

## Postconditions

A Draft plan for the target week mirrors the source week.
