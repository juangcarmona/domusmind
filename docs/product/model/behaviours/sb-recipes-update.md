---
id: "SB-RECIPES-UPDATE"
type: "structured-behaviour"
title: "Editing a recipe replaces its details and refreshes its update time"
status: "draft"
illustrates:
  - "UC-RECIPES-UPDATE-RECIPE"
given:
  - "a recipe exists in the household's library"
when: "a person submits updated details"
then:
  - "the recipe has the new values"
  - "the recipe's last-updated time is refreshed"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household updates a recipe); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Recipes stay editable after creation.

## Boundaries

None.
