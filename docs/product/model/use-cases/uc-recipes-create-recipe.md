---
id: "UC-RECIPES-CREATE-RECIPE"
type: "use-case"
title: "Add a recipe to the household library"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-RECIPES-NAME-UNIQUE"
  - "BR-RECIPES-INGREDIENT-NAME-UNIQUE"
  - "BR-RECIPES-TOTAL-TIME"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-INGREDIENT"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Library Surface: create); openspec/changes/recipe-management/proposal.md; src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (Create); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/CreateRecipe; interview: product owner decision Q-0039 (E-0158); interview: product owner decision Q-0038 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Make a household dish available for planning and shopping.

## Trigger

A person adds a recipe from the slot inspector or, from the recipe library surface (decided: Q-0039).

## Preconditions

The person belongs to a household.

## Main Flow

1. The person enters a name and optionally a description, preparation and cook times, servings, tags, allowed meal types, the favourite flag and ingredients.
2. The product adds the recipe to the household's recipe library.
3. The recipe becomes available for assignment to meal slots.

## Alternative Flows

- 1a. Quick add from the slot inspector captures fewer fields and no ingredients; ingredients are added later in the library, and until then the recipe contributes nothing to shopping lists; a recipe without ingredients can still be assigned to a meal slot (decided: Q-0038; observed: openspec/changes/recipe-management/design.md, Risks; openspec/changes/recipe-management/proposal.md).

## Failure Conditions

- A recipe with the same name already exists: creation is rejected and the existing recipe is unchanged.
- Two ingredients share a name: the recipe is rejected (inferred from BR-RECIPES-INGREDIENT-NAME-UNIQUE).

## Postconditions

The recipe is in the library.
