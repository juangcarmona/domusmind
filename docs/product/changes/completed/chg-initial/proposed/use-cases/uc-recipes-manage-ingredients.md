---
id: "UC-RECIPES-MANAGE-INGREDIENTS"
type: "use-case"
title: "Edit a recipe's ingredients"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-RECIPES-INGREDIENT-NAME-UNIQUE"
uses-terms:
  - "TERM-INGREDIENT"
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Ingredient Management); openspec/changes/recipe-management/design.md (Decision 2); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (AddIngredient, RemoveIngredient, UpdateIngredient); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/AddRecipeIngredient; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipeIngredient; src/backend/DomusMind.Application/Features/MealPlanning/RemoveRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Keep a recipe's ingredients accurate so shopping lists are right.

## Trigger

A person edits the ingredients of an existing recipe.

## Preconditions

The recipe exists in the household's library.

## Main Flow

1. The person adds an ingredient with a name and optional quantity and unit.
2. The product adds it to the recipe.

## Alternative Flows

- 1a. The person changes an ingredient's quantity or unit; its name stays the same.
- 1b. The person removes an ingredient by name.
- 1c. To rename an ingredient the person removes it and adds a new one (observed: design.md, Risks).

## Failure Conditions

- An ingredient with the same name (ignoring case) already exists: the addition is rejected and the recipe is unchanged.

## Postconditions

The recipe's ingredient list reflects the change. Delivered with the `recipe-management` change (decided: Q-0039).
