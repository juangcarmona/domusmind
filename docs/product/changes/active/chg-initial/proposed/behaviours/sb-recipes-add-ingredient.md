---
id: "SB-RECIPES-ADD-INGREDIENT"
type: "structured-behaviour"
title: "Adding a new ingredient grows the recipe's ingredient list"
status: "draft"
illustrates:
  - "UC-RECIPES-MANAGE-INGREDIENTS"
given:
  - "a recipe exists"
when: "a person adds an ingredient with a new name"
then:
  - "the ingredient is part of the recipe"
  - "the recipe has one more ingredient"
uses-terms:
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household adds an ingredient to an existing recipe); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/AddRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Ingredients can be added after creation.

## Boundaries

None.
