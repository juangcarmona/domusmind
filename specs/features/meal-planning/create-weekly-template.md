# Spec - Create Weekly Template

## Purpose

Create a reusable weekly meal pattern that can be applied to generate new meal plans.

A weekly template captures a household's typical week of meals to reduce planning effort for future weeks.

## Context

- Module: Meal Planning
- Aggregate: `WeeklyTemplate`
- Slice: `create-weekly-template`
- Command: `CreateWeeklyTemplate`

## Inputs

Required:

- `templateId`
- `familyId`
- `name`

Optional:

- `slots` (array of: `dayOfWeek`, `mealType`, `mealSourceType`, `recipeId`, `freeText`, `notes`, `isOptional`, `isLocked`)

## Preconditions

- target family must exist
- `templateId` must be unique
- `name` must be non-empty
- `name` must be unique within the family
- if `slots` provided:
  - each `dayOfWeek` / `mealType` combination must be unique within the template
  - each referenced `recipeId` must exist and belong to the same family
  - `recipeId` must be null when `mealSourceType ≠ Recipe`
  - `freeText` must be non-empty when `mealSourceType = FreeText`

## State Changes

On success, the system creates a new `WeeklyTemplate` aggregate with:

- stable identifier
- family association
- name
- collection of meal slot templates (may be empty at creation)

## Invariants

- template must belong to exactly one family
- template name must be unique within the family
- slot combinations (dayOfWeek + mealType) must be unique within a template
- `recipeId` must be null when `mealSourceType ≠ Recipe`
- `freeText` must be non-empty when `mealSourceType = FreeText`

## Events

Emit:

- `WeeklyTemplateCreated`

## Success Result

Return:

- `templateId`
- `familyId`
- `name`
- `slotCount`

## Failure Cases

- family not found
- duplicate `templateId`
- template name already exists in family
- duplicate slot combination in provided slots
- referenced recipe not found or belongs to different family

## Notes

Template slots can be added or modified later via `UpdateWeeklyTemplate`.
