# Spec - Copy Meal Plan From Previous Week

## Purpose

Create a new meal plan for a target week by cloning the slot structure and assignments from the previous week's plan.

This command is a fast-reuse shortcut for households with stable weekly meal patterns, without requiring a formal template.

## Context

- Module: Meal Planning
- Aggregate: `MealPlan`
- Slice: `copy-meal-plan-from-previous-week`
- Command: `CopyMealPlanFromPreviousWeek`

## Inputs

Required:

- `mealPlanId` (new plan identifier)
- `familyId`
- `weekStart` (target week; must be a Monday)

Optional:

- `sourceMealPlanId` (explicit source plan; if omitted, the system resolves the plan for the immediately preceding week)

## Preconditions

- target family must exist
- `mealPlanId` must be unique
- `weekStart` must be a valid Monday date
- no existing active meal plan for the same family and target week
- a source plan must exist for the resolved preceding week (or the explicit `sourceMealPlanId`)
- source plan must belong to the same family

## State Changes

On success, the system creates a new `MealPlan` aggregate with:

- stable identifier
- family association
- week definition (target week)
- status: `Draft`
- full meal slot grid materialized (all days × all meal types)
- each slot copied from the source plan:
  - `mealSourceType`
  - `recipeId` (if applicable)
  - `freeText` (if applicable)
  - `notes`
  - `isOptional`
  - `isLocked`
- `shoppingListId` and `shoppingListVersion` are NOT carried over (derivation starts fresh)
- `appliedTemplateId` is NOT set (copy-from-week is distinct from template application)

## Invariants

- every meal plan belongs to exactly one family
- week start must be a Monday
- only one Active plan per family per week
- source plan must belong to the same family as the target plan
- shopping list references are never transferred between plans

## Events

Emit:

- `MealPlanCreated`
- `MealPlanCopiedFromPreviousWeek` (carries `sourceMealPlanId` and `targetMealPlanId`)

## Success Result

Return:

- `mealPlanId`
- `familyId`
- `weekStart`
- `weekEnd`
- `sourceMealPlanId`
- `status`
- `slotCount`

## Failure Cases

- family not found
- duplicate `mealPlanId`
- `weekStart` is not a Monday
- active meal plan already exists for this family and target week
- no source plan found for the preceding week (and no explicit `sourceMealPlanId` provided)
- explicit `sourceMealPlanId` not found
- source plan belongs to a different family

## Notes

This command is distinct from `ApplyWeeklyTemplate`. Templates are explicit named patterns; previous-week copy is ad-hoc reuse.

Both coexist. A household may apply a template or copy from last week — these are independent strategies.

After creation, individual slots can still be modified via `UpdateMealSlot`.
