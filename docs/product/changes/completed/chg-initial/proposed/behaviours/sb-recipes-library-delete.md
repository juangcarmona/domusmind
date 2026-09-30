---
id: "SB-RECIPES-LIBRARY-DELETE"
type: "structured-behaviour"
title: "A recipe deleted from the library disappears from the list"
status: "draft"
illustrates:
  - "UC-RECIPES-DELETE-RECIPE"
given:
  - "a recipe exists and is not blocked by the deletion guard"
when: "a person deletes it from the library"
then:
  - "it is removed"
  - "it no longer appears in the list"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household deletes a recipe from the library surface); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; src/web/app/src/features/recipe-library; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Deletion is available from the library.

## Boundaries

None.
