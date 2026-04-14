# Spec - Apply Weekly Template

## Purpose

Create a new meal plan for a specific week pre-populated from an existing weekly template.

Applying a template is a shortcut for households that repeat a familiar weekly pattern, avoiding the need to assign each slot individually.

## Context

- Module: Meal Planning
- Aggregate: `MealPlan`
- Slice: `apply-weekly-template`
- Command: `ApplyWeeklyTemplate`

## Inputs

Required:

- `mealPlanId`
- `familyId`
- `weekStart` (must align with the household's configured first day of week)
- `templateId`

Optional:

- `responsibilityDomainId`

## Preconditions

- target family must exist
- `mealPlanId` must be unique
- `weekStart` must be a valid date aligned to the household's configured first day of week
- no existing meal plan for the same family and week (if one exists, it is returned as the result; template is not re-applied)
- `templateId` must exist and belong to the same family

## State Changes

On success, the system creates a new `MealPlan` aggregate with:

- stable identifier
- family association
- week definition
- status: `Draft`
- full meal slot grid materialized (all days × all meal types)
- slots copied from the template: `mealSourceType`, recipe references, free text, notes, `isOptional`, `isLocked` are transferred to the matching grid positions
- slots not covered by the template default to `mealSourceType = Unplanned`
- `appliedTemplateId` reference to the source template

Template application is a snapshot at the time of application.
Subsequent changes to the template do not affect already-created meal plans.

## Invariants

- every meal plan belongs to exactly one family
- week start must equal the household's configured first day of week
- copied recipe references must still be valid at the time of application

## Events

Emit:

- `MealPlanCreated`
- `WeeklyTemplateApplied`

## Success Result

Return:

- `mealPlanId`
- `familyId`
- `weekStart`
- `weekEnd`
- `appliedTemplateId`
- `slotCount`
- `status`

## Failure Cases

- family not found
- duplicate `mealPlanId`
- `weekStart` is not aligned to the household's configured first day of week
- template not found or belongs to a different family
- if a plan already exists for the week, it is returned without re-applying the template

## Notes

After applying a template, individual slots can still be modified via `UpdateMealSlot`.
The template reference on the plan records which template was last applied, supporting future template suggestions.
