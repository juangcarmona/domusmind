---
id: "FR-RECIPES-LIBRARY-SURFACE"
type: "functional-requirement"
title: "Standalone recipe library surface"
status: "draft"
derived-from:
  - "UC-RECIPES-BROWSE-LIBRARY"
  - "UC-RECIPES-VIEW-RECIPE"
  - "UC-RECIPES-CREATE-RECIPE"
  - "UC-RECIPES-UPDATE-RECIPE"
  - "UC-RECIPES-DELETE-RECIPE"
verification:
  - scenario-ref: "SB-RECIPES-BROWSE-LIBRARY"
  - scenario-ref: "SB-RECIPES-LIBRARY-OPEN-DETAIL"
  - scenario-ref: "SB-RECIPES-LIBRARY-CREATE"
  - scenario-ref: "SB-RECIPES-LIBRARY-EDIT"
  - scenario-ref: "SB-RECIPES-LIBRARY-DELETE"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (ADDED Requirement: Recipe Library Surface); openspec/changes/recipe-management/proposal.md; openspec/changes/recipe-management/design.md (Decision 5); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/web/app/src/features/recipe-library; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST provide a recipe library surface, independent of the weekly meal plan, where a household can browse recipes ordered by name, view a recipe's full detail, and create (including ingredients), edit and delete recipes, with a clear message when deletion is blocked (observed: openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Library Surface; openspec/changes/recipe-management/tasks.md 7.5).

## Rationale

Recipes are a peer capability, not a sub-view of the weekly grid (observed: openspec/changes/recipe-management/design.md, Decision 5). Delivered with the recipe-management change (decided: Q-0039).
