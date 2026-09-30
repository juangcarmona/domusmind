---
id: "SB-RECIPES-DELETE-UNREFERENCED"
type: "structured-behaviour"
title: "Deleting a recipe no Active plan uses removes it"
status: "draft"
illustrates:
  - "UC-RECIPES-DELETE-RECIPE"
  - "BR-RECIPES-DELETE-GUARD"
given:
  - "a recipe exists and no Active plan slot references it"
when: "a person deletes it"
then:
  - "the recipe is removed from the library"
  - "it is no longer available for meal slots"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household deletes an unreferenced recipe); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Households can prune their library.

## Boundaries

None.
