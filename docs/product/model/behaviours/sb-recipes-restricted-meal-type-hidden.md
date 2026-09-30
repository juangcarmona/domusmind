---
id: "SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN"
type: "structured-behaviour"
title: "A Dinner-only recipe is not offered for a Breakfast slot"
status: "draft"
illustrates:
  - "UC-RECIPES-BROWSE-LIBRARY"
  - "BR-RECIPES-ALLOWED-MEAL-TYPES"
given:
  - "a recipe allows only the Dinner meal type"
when: "a person browses recipes for a Breakfast slot"
then:
  - "the recipe is not offered for that slot"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Recipe is assigned to allowed meal types); openspec/changes/recipe-management/specs/meal-planning/spec.md (same scenario); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Restricts the recipe picker to sensible choices.

## Boundaries

Does not assert that assigning an incompatible recipe by other means is rejected.
