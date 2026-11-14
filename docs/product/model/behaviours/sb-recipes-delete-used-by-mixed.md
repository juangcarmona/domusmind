---
id: SB-RECIPES-DELETE-USED-BY-MIXED
type: structured-behaviour
title: Draft and Completed references allow recipe deletion
status: active
illustrates:
  - UC-RECIPES-DELETE-RECIPE
  - BR-RECIPES-DELETE-GUARD
given:
  - A recipe is referenced by Draft and Completed meal plans, with no Active plan reference.
when: a person deletes it
then:
  - The recipe is removed from the library.
  - The referring slots keep their reference, but recipe details can no longer be shown.
uses-terms:
  - TERM-RECIPE
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household deletes a recipe referenced by a Draft or Completed plan); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Apply the same deletion guard when both Draft and Completed plans refer to the recipe.

## Boundaries

The example concerns the stated action and context. Other actions have their own examples.
