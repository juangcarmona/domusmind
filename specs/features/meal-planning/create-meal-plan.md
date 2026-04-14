# Spec - Create Meal Plan

## Purpose

Create a new weekly meal plan for the household.

A meal plan represents the household's intended meals for a specific calendar week, identified by its Monday start date.

## Context

- Module: Meal Planning
- Aggregate: `MealPlan`
- Slice: `create-meal-plan`
- Command: `CreateMealPlan`

## Inputs

Required:

- `mealPlanId`
- `familyId`
- `weekStart` (must be a Monday)

Optional:

- `responsibilityDomainId` (soft area reference, informational only)

## Preconditions

- target family must exist
- `mealPlanId` must be unique
- `weekStart` must be a valid Monday date
- no existing active meal plan for the same family and week

## State Changes

On success, the system creates a new `MealPlan` aggregate with:

- stable identifier
- family association
- week definition (Monday–Sunday)
- status: `Draft`
- full meal slot grid materialized: all seven days × all meal types (Breakfast, MidMorningSnack, Lunch, AfternoonSnack, Dinner), each initialized to `mealSourceType = Unplanned`
- optional responsibility domain reference

## Invariants

- every meal plan belongs to exactly one family
- week start must be a Monday
- only one Active plan per family per week
- the full slot grid (7 days × 5 meal types = 35 slots) is always materialized at creation

## Events

Emit:

- `MealPlanCreated`

## Success Result

Return:

- `mealPlanId`
- `familyId`
- `weekStart`
- `weekEnd` (derived: weekStart + 6 days)
- `status`

## Failure Cases

- family not found
- duplicate `mealPlanId`
- `weekStart` is not a Monday
- active meal plan already exists for this family and week

## Notes

Meal slots are populated via `UpdateMealSlot` after plan creation, or by `ApplyWeeklyTemplate` or `CopyMealPlanFromPreviousWeek` at creation time if a template or previous week is provided.

A newly created plan is in `Draft` status. It must be explicitly promoted to `Active` before it becomes the household's working plan for the week.
