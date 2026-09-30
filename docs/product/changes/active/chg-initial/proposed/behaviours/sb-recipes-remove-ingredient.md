---
id: "SB-RECIPES-REMOVE-INGREDIENT"
type: "structured-behaviour"
title: "Removing an ingredient shrinks the recipe's ingredient list"
status: "draft"
illustrates:
  - "UC-RECIPES-MANAGE-INGREDIENTS"
given:
  - "a recipe contains an ingredient"
when: "a person removes it by name"
then:
  - "the ingredient is no longer part of the recipe"
  - "the recipe has one fewer ingredient"
uses-terms:
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household removes an ingredient from a recipe); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/RemoveRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Ingredients can be removed after creation.

## Boundaries

None.
