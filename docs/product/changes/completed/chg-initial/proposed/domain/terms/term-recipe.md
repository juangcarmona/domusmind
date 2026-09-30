---
id: "TERM-RECIPE"
type: "domain-term"
title: "Recipe"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "Recipe"
  - "recipe library"
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-INGREDIENT"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/specs/recipe-management/spec.md; docs/_legacy/04_contexts/meal-planning.md (Aggregate Roots: Recipe); docs/_legacy/03_domain/ubiquitous-language.md (Recipe); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs; src/backend/DomusMind.Api/Controllers/RecipesController.cs; interview: product owner decision Q-0039 (E-0158); interview: product owner decision Q-0038 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A household-defined, named set of Ingredients with optional planning metadata: description, preparation time, cook time, total time (their sum when both exist), servings, tags, allowed meal types (empty means unrestricted) and a favourite flag (observed: openspec/specs/meal-planning/spec.md, Recipe Library; src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs). All recipes of a household form its recipe library; recipes are always household-scoped, never owned by an individual person (observed: openspec/changes/recipe-management/proposal.md, out of scope list).

## Distinguish From

- Meal Slot (TERM-MEAL-SLOT): a slot may reference a recipe, but a recipe exists independently in the library and may be referenced by many slots.
- Free-text meal: a slot label with no ingredients; it is not a recipe.

## Usage

Assigned to meal slots, referenced by weekly templates, and the source of ingredients for shopping list derivation. Creation is part of the current specification; update, deletion, detail view, ingredient editing and a standalone recipe library surface were delivered with the `recipe-management` change and are current product (decided: Q-0039; observed: openspec/changes/recipe-management/proposal.md; src/backend/DomusMind.Api/Controllers/RecipesController.cs). A recipe does not need ingredients to be used in a meal slot (decided: Q-0038).
