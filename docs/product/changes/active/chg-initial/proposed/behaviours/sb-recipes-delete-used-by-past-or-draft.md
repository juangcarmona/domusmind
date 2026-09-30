---
id: "SB-RECIPES-DELETE-USED-BY-PAST-OR-DRAFT"
type: "structured-behaviour"
title: "Deleting a recipe used only by Draft or Completed plans succeeds"
status: "draft"
illustrates:
  - "UC-RECIPES-DELETE-RECIPE"
  - "BR-RECIPES-DELETE-GUARD"
given:
  - "a recipe is referenced only by slots in Draft or Completed meal plans"
when: "a person deletes it"
then:
  - "the recipe is removed from the library"
  - "the affected Draft or Completed slots keep their reference but the recipe details can no longer be shown"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household deletes a recipe referenced by a Draft or Completed plan); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Only the Active plan blocks deletion.

## Boundaries

None.
