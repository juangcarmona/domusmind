---
id: "FR-RECIPES-DELETE"
type: "functional-requirement"
title: "Delete a recipe with a guard"
status: "draft"
derived-from:
  - "UC-RECIPES-DELETE-RECIPE"
  - "BR-RECIPES-DELETE-GUARD"
verification:
  - scenario-ref: "SB-RECIPES-DELETE-UNREFERENCED"
  - scenario-ref: "SB-RECIPES-DELETE-BLOCKED-BY-ACTIVE-PLAN"
  - scenario-ref: "SB-RECIPES-DELETE-USED-BY-PAST-OR-DRAFT"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (ADDED Requirement: Recipe Deletion); openspec/specs/meal-planning/spec.md (Recipe Library); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (Delete); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/DeleteRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household delete a recipe from its library unless a slot of an Active meal plan references it; references from Draft or Completed plans MUST NOT block deletion (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Deletion).

## Rationale

Keeps the library tidy without breaking the working week; delivered with the recipe-management change (decided: Q-0039).
