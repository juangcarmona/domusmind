---
id: "SB-RECIPES-UNRESTRICTED-SHOWN-EVERYWHERE"
type: "structured-behaviour"
title: "A recipe without meal-type restriction is offered for any slot"
status: "draft"
illustrates:
  - "UC-RECIPES-BROWSE-LIBRARY"
  - "BR-RECIPES-ALLOWED-MEAL-TYPES"
given:
  - "a recipe has no allowed meal types set"
when: "a person browses recipes for a slot of any meal type"
then:
  - "the recipe is offered for that slot"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/changes/recipe-management/specs/meal-planning/spec.md (scenario: Recipe with no allowedMealTypes restriction is shown for any slot); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

An empty restriction means no restriction.

## Boundaries

None.
