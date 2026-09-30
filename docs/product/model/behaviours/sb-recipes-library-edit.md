---
id: "SB-RECIPES-LIBRARY-EDIT"
type: "structured-behaviour"
title: "A recipe edited from the library shows the new values"
status: "draft"
illustrates:
  - "UC-RECIPES-UPDATE-RECIPE"
given:
  - "a recipe exists in the library"
when: "a person edits and saves it"
then:
  - "the recipe reflects the updated values"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household edits a recipe from the library surface); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; src/web/app/src/features/recipe-library; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Editing is available from the library.

## Boundaries

None.
