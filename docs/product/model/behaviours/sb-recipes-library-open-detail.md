---
id: "SB-RECIPES-LIBRARY-OPEN-DETAIL"
type: "structured-behaviour"
title: "Selecting a recipe in the library shows its full detail"
status: "draft"
illustrates:
  - "UC-RECIPES-BROWSE-LIBRARY"
  - "UC-RECIPES-VIEW-RECIPE"
given:
  - "a recipe exists in the library"
when: "a person selects it from the list"
then:
  - "the full recipe detail is shown including the complete ingredient list"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household views a recipe's full detail); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetRecipeDetail; src/web/app/src/features/recipe-library; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Browsing leads straight to the detail.

## Boundaries

None.
