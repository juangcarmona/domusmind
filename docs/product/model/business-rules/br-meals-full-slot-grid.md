---
id: "BR-MEALS-FULL-SLOT-GRID"
type: "business-rule"
title: "Every meal plan has the full grid of slots from creation"
status: "draft"
applies-to:
  - "UC-MEALS-CREATE-PLAN"
  - "UC-MEALS-UPDATE-SLOT"
  - "UC-MEALS-VIEW-PLAN"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Creation, Meal Slot Assignment); docs/_legacy/04_contexts/meal-planning.md (MealPlan invariants); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When a meal plan is created it holds one slot for every day and meal type (7 days times 5 meal types, 35 slots), all Unplanned; no two slots share a day and meal type, and slot structure never changes afterwards: assigning or clearing changes content only (observed: openspec/specs/meal-planning/spec.md, Meal Plan Creation and Meal Slot Assignment; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs).

## Rationale

The surface is structure-first: the full week is always visible, even when empty (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Role).

## Examples

Clearing Tuesday's dinner turns it back to Unplanned; the Tuesday dinner slot remains in the grid.

## Exceptions

None.
