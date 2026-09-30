---
id: "SB-RECIPES-ADD-TO-LIBRARY"
type: "structured-behaviour"
title: "A recipe with a new name joins the library and can be planned"
status: "draft"
illustrates:
  - "UC-RECIPES-CREATE-RECIPE"
given:
  - "a household exists"
when: "a person creates a recipe with a unique name"
then:
  - "the recipe is in the household's recipe library"
  - "it is available for assignment to meal slots"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household adds a recipe to the library); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/CreateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Recipes become plannable as soon as they exist.

## Boundaries

None.
