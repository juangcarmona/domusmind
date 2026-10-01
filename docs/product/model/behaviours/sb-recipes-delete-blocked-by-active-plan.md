---
id: "SB-RECIPES-DELETE-BLOCKED-BY-ACTIVE-PLAN"
type: "structured-behaviour"
title: "Deleting a recipe used by an Active plan is rejected"
status: "draft"
illustrates:
  - "UC-RECIPES-DELETE-RECIPE"
  - "BR-RECIPES-DELETE-GUARD"
given:
  - "a recipe is referenced by at least one slot in an Active meal plan"
when: "a person tries to delete it"
then:
  - "the deletion is rejected"
  - "the recipe remains in the library"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household attempts to delete a recipe referenced by an Active plan); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The working week is protected.

## Boundaries

None.
