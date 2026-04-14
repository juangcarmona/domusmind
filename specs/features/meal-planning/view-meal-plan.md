# Spec - View Meal Plan

## Purpose

Retrieve the full detail of a meal plan for a specific week, including all meal slots, assigned recipes, and shopping list reference.

## Context

- Module: Meal Planning
- Slice: `view-meal-plan`
- Query: `GetMealPlanDetail`

## Inputs

Required (one of):

- `mealPlanId` — retrieve by plan identifier
- `familyId` + `weekStart` — retrieve by family and week

## Preconditions

- requesting family must match the plan's family

## Response Shape

```
MealPlanDetail {
  planId
  familyId
  weekStart
  weekEnd
  status
  appliedTemplateId (optional)
  shoppingListId (optional)
  shoppingListVersion
  lastDerivedAt (optional)
  slots [
    {
      dayOfWeek
      mealType
      mealSourceType
      recipe {
        recipeId
        name
        servings
        prepTimeMinutes
        totalTimeMinutes
        allowedMealTypes
      } (optional — present when mealSourceType = Recipe)
      freeText (optional — present when mealSourceType = FreeText)
      notes (optional)
      isOptional
      isLocked
    }
  ]
}
```

Slots are returned ordered by day (from household's configured first day of week) then by meal type (Breakfast, MidMorningSnack, Lunch, AfternoonSnack, Dinner).

All slots are included in the response (materialized grid). Unplanned slots are included to show the full week structure.

## Failure Cases

- meal plan not found
- plan does not belong to requesting family

## Notes

This query is the primary data source for the weekly meal planning surface.
It may also feed the Agenda projection for meal slots appearing on a given day.
