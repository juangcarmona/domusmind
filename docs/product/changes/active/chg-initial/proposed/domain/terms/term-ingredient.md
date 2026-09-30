---
id: "TERM-INGREDIENT"
type: "domain-term"
title: "Ingredient"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "Ingredient"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); docs/_legacy/04_contexts/meal-planning.md (Internal Entities: Ingredient); openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Ingredient Management); src/backend/DomusMind.Domain/MealPlanning/Entities/Ingredient.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A named component of a Recipe with an optional quantity and optional unit. Within a recipe an ingredient is identified by its name, compared case-insensitively (observed: docs/_legacy/04_contexts/meal-planning.md, Ingredient; openspec/changes/recipe-management/specs/recipe-management/spec.md, Recipe Ingredient Management; src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs).

## Distinguish From

- List item (TERM-LIST-ITEM): derived shopping list items are created from ingredients but belong to the Lists context afterwards.

## Usage

Edited as part of a recipe; consolidated across a week's recipes during shopping list derivation.
