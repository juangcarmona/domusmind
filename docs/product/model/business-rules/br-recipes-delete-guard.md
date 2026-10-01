---
id: "BR-RECIPES-DELETE-GUARD"
type: "business-rule"
title: "A recipe used by an Active meal plan cannot be deleted"
status: "draft"
applies-to:
  - "UC-RECIPES-DELETE-RECIPE"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-PLAN-STATUS"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Deletion); openspec/changes/recipe-management/design.md (Decision 1); docs/_legacy/04_contexts/meal-planning.md (Commands: DeleteRecipe); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A recipe referenced by any slot of an Active meal plan cannot be deleted; references from Draft or Completed plans do not block deletion, and those slots keep a reference whose recipe details can no longer be shown (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Deletion). Deletion removes the recipe from the library entirely (observed: openspec/changes/recipe-management/design.md, Decision 1).

## Rationale

Protects the household's working week from losing a planned meal (inferred).

## Examples

A recipe planned for this week's Active plan cannot be deleted; one only used in last week's Completed plan can.

## Exceptions

A template that references a deleted recipe can no longer be applied (observed: openspec/specs/meal-planning/spec.md, Apply Weekly Template). Delivered with the `recipe-management` change (decided: Q-0039).
