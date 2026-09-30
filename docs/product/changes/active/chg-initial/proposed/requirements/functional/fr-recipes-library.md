---
id: "FR-RECIPES-LIBRARY"
type: "functional-requirement"
title: "Household recipe library"
status: "draft"
derived-from:
  - "UC-RECIPES-CREATE-RECIPE"
  - "UC-RECIPES-BROWSE-LIBRARY"
  - "BR-RECIPES-NAME-UNIQUE"
  - "BR-RECIPES-INGREDIENT-NAME-UNIQUE"
  - "BR-RECIPES-TOTAL-TIME"
  - "BR-RECIPES-ALLOWED-MEAL-TYPES"
verification:
  - scenario-ref: "SB-RECIPES-ADD-TO-LIBRARY"
  - scenario-ref: "SB-RECIPES-DUPLICATE-NAME-REJECTED"
  - scenario-ref: "SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN"
  - scenario-ref: "SB-RECIPES-UNRESTRICTED-SHOWN-EVERYWHERE"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-INGREDIENT"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Recipe Library); openspec/changes/recipe-management/specs/meal-planning/spec.md (MODIFIED Requirement: Recipe Library); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs; src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST maintain a household recipe library of recipes with unique names, optional description, preparation and cook times (with derived total time), servings, ingredients with unique names and optional quantity and unit, tags, allowed meal types and a favourite flag. When browsing recipes for a slot it MUST offer only recipes compatible with the slot's meal type (observed: openspec/specs/meal-planning/spec.md, Recipe Library; openspec/changes/recipe-management/specs/meal-planning/spec.md adds the compatibility rule for unrestricted recipes).

## Rationale

Recipes are the household's reusable ingredient library and the only source of shopping list items. The meal-type filtering part was delivered with the recipe-management change (decided: Q-0039).
