---
id: "SB-RECIPES-DUPLICATE-INGREDIENT-REJECTED"
type: "structured-behaviour"
title: "Adding an ingredient whose name differs only in case is rejected"
status: "draft"
illustrates:
  - "UC-RECIPES-MANAGE-INGREDIENTS"
  - "BR-RECIPES-INGREDIENT-NAME-UNIQUE"
given:
  - "a recipe contains an ingredient named \"Olive oil\""
when: "a person adds an ingredient named \"olive oil\""
then:
  - "the addition is rejected"
  - "the recipe is unchanged"
uses-terms:
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household attempts to add a duplicate ingredient); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/AddRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Ingredient names compare without regard to case.

## Boundaries

None.
