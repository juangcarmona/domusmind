---
id: "FR-RECIPES-INGREDIENTS"
type: "functional-requirement"
title: "Edit recipe ingredients"
status: "draft"
derived-from:
  - "UC-RECIPES-MANAGE-INGREDIENTS"
  - "BR-RECIPES-INGREDIENT-NAME-UNIQUE"
verification:
  - scenario-ref: "SB-RECIPES-ADD-INGREDIENT"
  - scenario-ref: "SB-RECIPES-DUPLICATE-INGREDIENT-REJECTED"
  - scenario-ref: "SB-RECIPES-UPDATE-INGREDIENT"
  - scenario-ref: "SB-RECIPES-REMOVE-INGREDIENT"
uses-terms:
  - "TERM-INGREDIENT"
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (ADDED Requirement: Recipe Ingredient Management); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs; openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/AddRecipeIngredient; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipeIngredient; src/backend/DomusMind.Application/Features/MealPlanning/RemoveRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household add, update (quantity and unit) and remove individual ingredients of an existing recipe, identifying an ingredient by its name ignoring case and rejecting duplicates (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Ingredient Management).

## Rationale

Accurate ingredients make accurate shopping lists; delivered with the recipe-management change (decided: Q-0039).
