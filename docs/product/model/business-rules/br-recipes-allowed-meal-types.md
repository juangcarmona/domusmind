---
id: "BR-RECIPES-ALLOWED-MEAL-TYPES"
type: "business-rule"
title: "Recipes are offered only for compatible meal types"
status: "draft"
applies-to:
  - "UC-RECIPES-BROWSE-LIBRARY"
  - "UC-MEALS-UPDATE-SLOT"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/meal-planning/spec.md (Recipe Library, MODIFIED); openspec/changes/recipe-management/design.md (Decision 3); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When browsing recipes for a slot, only recipes compatible with the slot's meal type are offered: a recipe is compatible when its allowed meal types are empty (no restriction) or include that meal type (observed: openspec/changes/recipe-management/specs/meal-planning/spec.md, Recipe Library).

## Rationale

Keeps the recipe picker short and relevant (inferred).

## Examples

A recipe restricted to Dinner is not offered for a Breakfast slot; an unrestricted recipe is offered everywhere.

## Exceptions

The rule constrains browsing, not assignment: the sources do not say whether assigning an incompatible recipe directly is rejected. Delivered with the `recipe-management` change (decided: Q-0039).
