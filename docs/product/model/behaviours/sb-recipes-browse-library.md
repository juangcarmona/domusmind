---
id: "SB-RECIPES-BROWSE-LIBRARY"
type: "structured-behaviour"
title: "The recipe library lists every recipe by name with summary details"
status: "draft"
illustrates:
  - "UC-RECIPES-BROWSE-LIBRARY"
given:
  - "the household has one or more recipes"
when: "a person opens the recipe library"
then:
  - "every recipe is listed with name, cook time, servings, tag count and ingredient count"
  - "the list is ordered by name"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household browses the recipe library); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The library is reachable independently of the weekly plan.

## Boundaries

None.
