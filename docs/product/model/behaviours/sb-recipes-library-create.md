---
id: "SB-RECIPES-LIBRARY-CREATE"
type: "structured-behaviour"
title: "A recipe created from the library appears in it"
status: "draft"
illustrates:
  - "UC-RECIPES-CREATE-RECIPE"
given:
  - "a person is on the recipe library surface"
when: "the person submits a new recipe with a name and optional details including ingredients"
then:
  - "the recipe is created"
  - "it appears in the library"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household creates a recipe from the library surface); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/CreateRecipe; src/web/app/src/features/recipe-library; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The library is a full creation entry point, including ingredients.

## Boundaries

None.
