---
id: "SB-RECIPES-UPDATE-INGREDIENT"
type: "structured-behaviour"
title: "Changing an ingredient's quantity or unit keeps its name"
status: "draft"
illustrates:
  - "UC-RECIPES-MANAGE-INGREDIENTS"
given:
  - "a recipe contains an ingredient named \"Flour\""
when: "a person changes its quantity and unit"
then:
  - "the ingredient has the new quantity and unit"
  - "the ingredient name is unchanged"
uses-terms:
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household updates an ingredient's quantity or unit); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipeIngredient; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Quantities can be corrected in place.

## Boundaries

None.
