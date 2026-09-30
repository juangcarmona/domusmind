---
id: "SB-RECIPES-VIEW-DETAIL"
type: "structured-behaviour"
title: "Viewing a recipe shows all its details and ingredients"
status: "draft"
illustrates:
  - "UC-RECIPES-VIEW-RECIPE"
given:
  - "a recipe exists in the household's library"
when: "a person opens it"
then:
  - "the name, description, times, servings, tags, allowed meal types, favourite flag and complete ingredient list are shown"
  - "each ingredient shows its name and any quantity and unit"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household retrieves a recipe by ID); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetRecipeDetail; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

A single place to read the whole recipe.

## Boundaries

None.
