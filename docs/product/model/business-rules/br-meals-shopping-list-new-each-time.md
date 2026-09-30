---
id: "BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME"
type: "business-rule"
title: "Each derivation creates a new list owned by Lists"
status: "draft"
applies-to:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-LIST"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Shopping List Derivation); docs/_legacy/04_contexts/meal-planning.md (Integration with Lists); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs; interview: product owner decision Q-0036 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Every shopping list request creates a new List in Lists and never changes lists derived earlier; the meal plan keeps a reference to the most recent list and a derivation version that increases on each request. From creation the list belongs to Lists; Meal Planning gets no feedback when items are checked (observed: openspec/specs/meal-planning/spec.md, Shopping List Derivation; docs/_legacy/04_contexts/meal-planning.md, Integration with Lists).

## Rationale

Purchase tracking belongs entirely to Lists; replanning must not disturb a list someone is already shopping from (inferred).

## Examples

Requesting a second list after changing Friday's dinner leaves the first list untouched and creates a new one.

## Exceptions

Conflict: the legacy surface does not expose re-generation after a list exists ("creating a second list is not supported here") (observed: docs/_legacy/00_product/surfaces/meal-planning.md, state 7). Each request generates a new shopping list (decided: Q-0036); the surface limitation is a UI gap, not a product rule.
