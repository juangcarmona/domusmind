---
id: "UC-RECIPES-VIEW-RECIPE"
type: "use-case"
title: "View a recipe's details"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by: []
uses-terms:
  - "TERM-RECIPE"
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Detail Retrieval, Recipe Library Surface); docs/_legacy/04_contexts/meal-planning.md (Queries: GetRecipeDetail); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetRecipeDetail; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See everything about one recipe, including its ingredients.

## Trigger

A person selects a recipe in the library.

## Preconditions

The recipe exists in the household's library.

## Main Flow

1. The person selects the recipe.
2. The product shows its name, description, times, servings, tags, allowed meal types, favourite flag and full ingredient list with optional quantity and unit.

## Alternative Flows

None.

## Failure Conditions

- The recipe does not exist in the household's library: the product reports that it was not found.

## Postconditions

The person sees the recipe. Delivered with the `recipe-management` change (decided: Q-0039).
