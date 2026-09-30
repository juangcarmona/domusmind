---
id: "FR-RECIPES-DETAIL"
type: "functional-requirement"
title: "View recipe detail"
status: "draft"
derived-from:
  - "UC-RECIPES-VIEW-RECIPE"
verification:
  - scenario-ref: "SB-RECIPES-VIEW-DETAIL"
  - scenario-ref: "SB-RECIPES-VIEW-NOT-FOUND"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-INGREDIENT"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (ADDED Requirement: Recipe Detail Retrieval); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetRecipeDetail; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show the full detail of a single recipe in the household's library, including its complete ingredient list, and MUST report not found for a recipe outside the household's library (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Detail Retrieval).

## Rationale

Delivered with the recipe-management change (decided: Q-0039).
