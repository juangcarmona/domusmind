# Spec - Create Recipe

## Purpose

Add a recipe to the household recipe library.

A recipe is a named set of ingredients with optional preparation metadata that can be assigned to meal slots in a meal plan.

## Context

- Module: Meal Planning
- Aggregate: `Recipe`
- Slice: `create-recipe`
- Command: `CreateRecipe`

## Inputs

Required:

- `recipeId`
- `familyId`
- `name`

Optional:

- `description`
- `prepTimeMinutes`
- `cookTimeMinutes`
- `servings`
- `ingredients` (array of: `name`, `quantity`, `unit`)
- `allowedMealTypes` (array of meal types this recipe is appropriate for; absence means unrestricted)
- `tags` (array of free-form classification labels)
- `isFavorite` (boolean, default: false)

## Preconditions

- target family must exist
- `recipeId` must be unique
- `name` must be non-empty
- `name` must be unique within the family
- if `ingredients` provided, each ingredient name must be non-empty and unique within the list

## State Changes

On success, the system creates a new `Recipe` aggregate with:

- stable identifier
- family association
- name
- optional metadata fields (description, prepTimeMinutes, cookTimeMinutes, servings)
- `totalTimeMinutes` (derived: prepTimeMinutes + cookTimeMinutes when both present; otherwise null)
- ingredient collection (may be empty at creation)
- `allowedMealTypes` (empty means unrestricted)
- `tags`
- `isFavorite`

## Invariants

- recipe must belong to exactly one family
- recipe name must be unique within the family
- ingredient name must be unique within a recipe (deduplication constraint)
- ingredient quantity and unit are optional

## Events

Emit:

- `RecipeCreated`

## Success Result

Return:

- `recipeId`
- `familyId`
- `name`
- `ingredientCount`
- `totalTimeMinutes`
- `isFavorite`

## Failure Cases

- family not found
- duplicate `recipeId`
- recipe name already exists in family
- empty name
- duplicate ingredient name within the provided ingredient list

## Notes

Ingredients can be added later via `UpdateRecipe`.
Recipes may be assigned to meal slots after creation.
A recipe may not be deleted if it is currently referenced by any active meal plan slot.
