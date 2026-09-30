---
id: "UC-RECIPES-DELETE-RECIPE"
type: "use-case"
title: "Remove a recipe from the library"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-RECIPES-DELETE-GUARD"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Deletion, Recipe Library Surface); openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/design.md (Decision 1); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (Delete); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Keep the recipe library free of dishes the household no longer cooks.

## Trigger

A person deletes a recipe from the library.

## Preconditions

The recipe exists in the household's library.

## Main Flow

1. The person asks to delete the recipe and confirms.
2. The product checks that no Active meal plan uses it.
3. The product removes the recipe; it is no longer offered for slots.

## Alternative Flows

- 2a. Only Draft or Completed plans use it: deletion proceeds; those slots keep a reference whose details can no longer be shown.

## Failure Conditions

- An Active meal plan uses it: deletion is rejected with an understandable message and the recipe stays.

## Postconditions

The recipe is gone from the library. Delivered with the `recipe-management` change (decided: Q-0039).
