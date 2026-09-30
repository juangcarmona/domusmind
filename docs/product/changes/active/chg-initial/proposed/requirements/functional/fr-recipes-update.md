---
id: "FR-RECIPES-UPDATE"
type: "functional-requirement"
title: "Edit a recipe"
status: "draft"
derived-from:
  - "UC-RECIPES-UPDATE-RECIPE"
  - "BR-RECIPES-NAME-UNIQUE"
verification:
  - scenario-ref: "SB-RECIPES-UPDATE"
  - scenario-ref: "SB-RECIPES-RENAME-CONFLICT"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (ADDED Requirement: Recipe Update); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (Update); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household replace a recipe's name, description, preparation and cook times, servings, favourite flag, allowed meal types and tags in full, keeping the name unique within the household (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Update).

## Rationale

Recipes evolve; delivered with the recipe-management change (decided: Q-0039).
