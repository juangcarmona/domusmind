---
id: "SB-RECIPES-DUPLICATE-NAME-REJECTED"
type: "structured-behaviour"
title: "Creating a recipe with an existing name is rejected"
status: "draft"
illustrates:
  - "UC-RECIPES-CREATE-RECIPE"
  - "BR-RECIPES-NAME-UNIQUE"
given:
  - "a recipe named \"Pasta Bolognese\" already exists in the household's library"
when: "a person creates another recipe with the same name"
then:
  - "the creation is rejected"
  - "the existing recipe is unchanged"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household attempts to create a recipe with a duplicate name); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/CreateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Recipe names identify recipes within a household.

## Boundaries

None.
