---
id: "BR-MEALS-SLOT-SOURCE-CONSISTENCY"
type: "business-rule"
title: "A slot's content matches its meal source"
status: "draft"
applies-to:
  - "UC-MEALS-UPDATE-SLOT"
  - "UC-MEALS-CREATE-TEMPLATE"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-SOURCE"
  - "TERM-RECIPE"
provenance:
  source: "docs/_legacy/04_contexts/meal-planning.md (MealSlot invariants); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (UpdateSlot); src/backend/DomusMind.Domain/MealPlanning/Entities/MealSlot.cs"
  confidence: "high"
  recovered-from: "observation"
---

## Rule

A slot with source Recipe must reference a recipe; a slot with source Free text must have non-empty text; a recipe reference is kept only for Recipe slots and free text only for Free text slots, so switching source discards the other content (observed: docs/_legacy/04_contexts/meal-planning.md, MealSlot invariants; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs, src/backend/DomusMind.Domain/MealPlanning/Entities/MealSlot.cs).

## Rationale

Keeps each slot's meaning unambiguous for the grid, the Agenda and shopping list derivation (inferred).

## Examples

Switching a slot from "Recipe: lasagne" to "External: restaurant" removes the lasagne reference.

## Exceptions

None.
