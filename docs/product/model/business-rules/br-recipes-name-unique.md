---
id: "BR-RECIPES-NAME-UNIQUE"
type: "business-rule"
title: "Recipe names are unique within a household"
status: "draft"
applies-to:
  - "UC-RECIPES-CREATE-RECIPE"
  - "UC-RECIPES-UPDATE-RECIPE"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-HOUSEHOLD"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Update); docs/_legacy/04_contexts/meal-planning.md (Recipe invariants); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; src/backend/DomusMind.Application/Features/MealPlanning/CreateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

No two recipes in a household's library share a name, on creation and on rename (observed: openspec/specs/meal-planning/spec.md, Recipe Library scenario "duplicate name"; openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Update scenario "conflicting name").

## Rationale

Recipes are picked by name in the slot inspector and library (inferred).

## Examples

A second "Pasta Bolognese" is rejected and the existing one is unchanged.

## Exceptions

None.
