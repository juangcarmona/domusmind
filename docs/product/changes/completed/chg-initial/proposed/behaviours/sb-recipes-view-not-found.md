---
id: "SB-RECIPES-VIEW-NOT-FOUND"
type: "structured-behaviour"
title: "Opening a recipe that is not in the library reports not found"
status: "draft"
illustrates:
  - "UC-RECIPES-VIEW-RECIPE"
given:
  - "no recipe with that identity exists in the household's library"
when: "a person tries to open it"
then:
  - "the product reports that the recipe was not found"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Recipe not found); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetRecipeDetail; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Recipes of other households are never revealed.

## Boundaries

None.
