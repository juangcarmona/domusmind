---
id: "UC-MEALS-CREATE-PLAN"
type: "use-case"
title: "Start a meal plan for a week"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
  - "BR-MEALS-WEEK-DEFINITION"
  - "BR-MEALS-FULL-SLOT-GRID"
  - "BR-MEALS-PLAN-LIFECYCLE"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-PLAN-STATUS"
  - "TERM-HOUSEHOLD"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Creation); docs/_legacy/00_product/surfaces/meal-planning.md (state 2: Start from scratch); docs/_legacy/04_contexts/meal-planning.md (Commands: CreateMealPlan); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Have an empty weekly meal grid for the household to fill in.

## Trigger

A person opens a week with no meal plan and chooses "Start from scratch" (observed: docs/_legacy/00_product/surfaces/meal-planning.md, state 2).

## Preconditions

The person belongs to a household. The chosen week start is valid for the household's week.

## Main Flow

1. The person selects a week in the Meal Planning surface.
2. The person chooses to start a plan from scratch.
3. The product creates a Draft meal plan for that week with every slot Unplanned.
4. The product shows the full weekly grid.

## Alternative Flows

- 3a. The week already has a meal plan: the product shows the existing plan and creates nothing new.
- 2a. The person instead copies the previous week (UC-MEALS-COPY-PREVIOUS-WEEK) or applies a template (UC-MEALS-APPLY-TEMPLATE).

## Failure Conditions

- The week start does not match the household's week: the plan is not created.
- The action fails (for example a network failure): a compact inline notice appears and the page stays as it was (observed: surface state 4).

## Postconditions

A Draft meal plan with the full slot grid exists for the week.
