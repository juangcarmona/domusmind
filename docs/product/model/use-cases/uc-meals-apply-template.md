---
id: "UC-MEALS-APPLY-TEMPLATE"
type: "use-case"
title: "Plan a week from a template"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-TEMPLATE-SNAPSHOT"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
  - "BR-MEALS-PLAN-LIFECYCLE"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Apply Weekly Template); docs/_legacy/00_product/surfaces/meal-planning.md (Header, states 2, 5, 6); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (CreateFromTemplate); interview: product owner decision Q-0037 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Fill a week's meal plan from a known pattern in one action.

## Trigger

A person chooses "Apply template" for a week (observed: surface, Header).

## Preconditions

A weekly template exists for the household.

## Main Flow

1. The person selects a target week and a template.
2. The product creates a Draft plan for that week with slots copied from the template and the rest Unplanned.
3. The product records which template was applied and shows the plan.
4. The person may still change individual slots (UC-MEALS-UPDATE-SLOT).

## Alternative Flows

- 2a. The week already has a plan: the product shows the existing plan with a notice that the week already has a meal plan; the template is not applied.

## Failure Conditions

- A recipe referenced by the template has been deleted: nothing is created.

## Postconditions

A Draft plan for the week exists, based on the template. The legacy surface shows this action disabled as "coming soon" (observed: docs/_legacy/00_product/surfaces/meal-planning.md, state 6), so confidence medium. Weekly templates are current product at medium confidence, flagged for the Meal Planning refinement change (decided: Q-0037).
