# Spec - Update Meal Slot

## Purpose

Assign or clear a meal slot within an existing meal plan.

A meal slot is identified by its day of week and meal type. Each slot carries a source type (recipe, free text, external, leftovers, or unplanned), optional recipe reference, optional free text, optional notes, and behavioral flags.

Slots are always present (materialized at plan creation). This command updates their content.

## Context

- Module: Meal Planning
- Aggregate: `MealPlan`
- Slice: `update-meal-slot`
- Command: `UpdateMealSlot`

## Inputs

Required:

- `mealPlanId`
- `dayOfWeek` (any day in the household week)
- `mealType` (Breakfast, MidMorningSnack, Lunch, AfternoonSnack, Dinner)
- `mealSourceType` (Recipe, FreeText, External, Leftovers, Unplanned)

Optional:

- `recipeId` (required when `mealSourceType = Recipe`; null otherwise)
- `freeText` (required when `mealSourceType = FreeText`; null otherwise)
- `notes`
- `isOptional` (boolean)
- `isLocked` (boolean)

## Preconditions

- target meal plan must exist
- meal plan must belong to the requesting family
- meal plan must be in `Active` status (Draft plans allow mutation; Completed plans do not)
- `dayOfWeek` must be a valid value
- `mealType` must be a valid enum value
- if `recipeId` is provided, the recipe must exist and belong to the same family
- if the slot is locked, the update must explicitly set `isLocked = false` before mutating content

## State Changes

On success, the system updates the `MealSlot` within the `MealPlan`:

- `mealSourceType` is set to the provided value
- `recipeId` is set when `mealSourceType = Recipe`; cleared otherwise
- `freeText` is set when `mealSourceType = FreeText`; cleared otherwise
- `notes` is updated if provided
- `isOptional` and `isLocked` flags are updated if provided
- setting `mealSourceType = Unplanned` clears the slot content but retains the slot in the grid

## Invariants

- a meal slot is uniquely identified by (mealPlanId, dayOfWeek, mealType)
- `recipeId` must be null when `mealSourceType ≠ Recipe`
- `freeText` must be non-empty when `mealSourceType = FreeText`
- recipe must belong to the same family as the meal plan
- a locked slot may not have its content changed unless `isLocked = false` is included in the same operation

## Events

Emit:

- `MealSlotAssigned` (when mealSourceType is set to any non-Unplanned value)
- `MealSlotCleared` (when mealSourceType is set to Unplanned)

## Success Result

Return:

- `mealPlanId`
- `dayOfWeek`
- `mealType`
- `mealSourceType`
- `recipeId` (null if not applicable)
- `freeText` (null if not applicable)
- `notes`
- `isOptional`
- `isLocked`

## Failure Cases

- meal plan not found
- meal plan does not belong to requesting family
- meal plan is Completed (mutation not allowed)
- invalid `dayOfWeek` or `mealType`
- `mealSourceType = Recipe` but no `recipeId` provided
- `mealSourceType = FreeText` but no `freeText` provided
- recipe not found
- recipe belongs to a different family
- slot is locked and update does not unlock it

## Notes

Slots are always present in the grid (materialized at plan creation). This command updates content, not structure.

Setting `mealSourceType = Unplanned` clears assignment. The slot remains in the grid to preserve week structure visibility.

A recipe's `allowedMealTypes` is advisory: assignment to a non-allowed meal type is permitted but may be flagged at the UX level.
