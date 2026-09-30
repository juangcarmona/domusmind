---
id: "TERM-SHOPPING-LIST-DERIVATION"
type: "domain-term"
title: "Shopping List Derivation"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "RequestShoppingList"
  - "generate shopping list"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-INGREDIENT"
  - "TERM-LIST"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Shopping List Derivation); docs/_legacy/04_contexts/meal-planning.md (Integration with Lists); docs/_legacy/00_product/surfaces/meal-planning.md (Relationship with Lists); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The household action that turns the ingredients of all recipe slots in a Meal Plan into a new shopping List in the Lists context, consolidating ingredients with the same name and unit. Each derivation creates a new list and advances the plan's derivation version (observed: openspec/specs/meal-planning/spec.md, Shopping List Derivation).

## Distinguish From

- List (TERM-LIST): the result; once created it is an ordinary shared list owned by the Lists context.
- Purchase tracking: checking off items is Lists behaviour and never flows back to Meal Planning.

## Usage

Triggered by "Generate shopping list" on the Meal Planning surface (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Header).
