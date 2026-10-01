---
id: "UC-RECIPES-UPDATE-RECIPE"
type: "use-case"
title: "Edit a recipe"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-RECIPES-NAME-UNIQUE"
  - "BR-RECIPES-TOTAL-TIME"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Update, Recipe Library Surface); openspec/specs/meal-planning/spec.md (Recipe Library: 'Recipes may be updated after creation'); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (Update); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Keep a recipe's details correct.

## Trigger

A person edits a recipe from the library.

## Preconditions

The recipe exists in the household's library.

## Main Flow

1. The person changes the name, description, times, servings, favourite flag, allowed meal types or tags.
2. The product replaces all of these details with the submitted values and records when the recipe was last updated.

## Alternative Flows

- Ingredients are edited separately (UC-RECIPES-MANAGE-INGREDIENTS).

## Failure Conditions

- The new name belongs to another recipe: the change is rejected and neither recipe changes.

## Postconditions

The recipe shows the new details. Delivered with the `recipe-management` change (decided: Q-0039).
