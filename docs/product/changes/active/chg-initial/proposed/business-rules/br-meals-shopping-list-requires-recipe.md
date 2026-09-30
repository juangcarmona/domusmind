---
id: "BR-MEALS-SHOPPING-LIST-REQUIRES-RECIPE"
type: "business-rule"
title: "A shopping list needs at least one recipe slot"
status: "draft"
applies-to:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-MEAL-SOURCE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Shopping List Derivation); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (RequestShoppingList)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A shopping list can be derived only when the plan has at least one slot with source Recipe (observed: openspec/specs/meal-planning/spec.md, Shopping List Derivation; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs).

## Rationale

Only recipes carry ingredients; free-text, external and leftover meals have nothing to buy (inferred).

## Examples

A week planned entirely with free-text meals cannot produce a shopping list.

## Exceptions

None.
