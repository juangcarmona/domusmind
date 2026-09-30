---
id: "TERM-MEAL-SOURCE"
type: "domain-term"
title: "Meal Source"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "MealSourceType"
  - "meal source type"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Slot Assignment); docs/_legacy/04_contexts/meal-planning.md (MealSlot); src/backend/DomusMind.Domain/MealPlanning/Enums/MealSourceType.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

How a Meal Slot is filled: Recipe (references a recipe from the household library), Free text (a label such as "pasta night"), External (eaten outside the household, such as school lunch or a restaurant), Leftovers (leftovers from a prior meal), or Unplanned (explicitly left open) (observed: openspec/specs/meal-planning/spec.md, Meal Slot Assignment).

## Distinguish From

- Recipe (TERM-RECIPE): only one of the five sources refers to a recipe; a slot never requires one.

## Usage

Chosen in the slot inspector or quick actions (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Interaction). The surface must never show "No recipe" as a slot label (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Anti-Patterns).
