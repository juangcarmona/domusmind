---
id: "BR-RECIPES-INGREDIENT-NAME-UNIQUE"
type: "business-rule"
title: "Ingredient names are unique within a recipe"
status: "draft"
applies-to:
  - "UC-RECIPES-CREATE-RECIPE"
  - "UC-RECIPES-MANAGE-INGREDIENTS"
uses-terms:
  - "TERM-INGREDIENT"
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Ingredient Management); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (AddIngredient); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; src/backend/DomusMind.Application/Features/MealPlanning/CreateRecipe; src/backend/DomusMind.Application/Features/MealPlanning/AddRecipeIngredient; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A recipe cannot contain two ingredients whose names match case-insensitively; the name identifies the ingredient within the recipe, so renaming an ingredient means removing it and adding a new one (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Ingredient Management; src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs; openspec/changes/recipe-management/design.md, Risks).

## Rationale

Supports deduplication during shopping list derivation (observed: openspec/specs/meal-planning/spec.md, Recipe Library).

## Examples

Adding "olive oil" to a recipe that already has "Olive oil" is rejected.

## Exceptions

None.
