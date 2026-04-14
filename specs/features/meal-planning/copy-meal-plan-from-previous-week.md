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
- `weekStart` (target week; must align with the household's configured first day of week)

Optional:

- `sourceMealPlanId` (explicit source plan; if omitted, the system resolves the plan for the immediately preceding week)

## Preconditions

- target family must exist
- `mealPlanId` must be unique
- `weekStart` must be a valid date aligned to the household's configured first day of week
- no existing meal plan for the same family and target week (if one exists, it is returned without copying)
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
- week start must equal the household's configured first day of week
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

## Recoverable Operational Outcomes

- **No source plan found**: when no plan exists for the preceding week (or the explicit source), the command returns `Success = false, ErrorCode = "NoPreviousPlan"`. This is not an error — the UI stays on the current week surface and shows a compact inline notice. The user can still create a plan from scratch or apply a template.
- **Plan already exists for target week**: when a plan already exists for the target week, the existing plan is returned with `AlreadyExisted = true`. The UI shows a compact notice and keeps the loaded plan.

## Failure Cases

- family not found
- duplicate `mealPlanId` (idempotent guard)
- `weekStart` is not aligned to the household's configured first day of week
- source plan belongs to a different family

("No previous plan found" and "plan already exists for target week" are recoverable operational outcomes, not failure cases — see above.)

## Notes

This command is distinct from `ApplyWeeklyTemplate`. Templates are explicit named patterns; previous-week copy is ad-hoc reuse.

Both coexist. A household may apply a template or copy from last week — these are independent strategies.

After creation, individual slots can still be modified via `UpdateMealSlot`.
