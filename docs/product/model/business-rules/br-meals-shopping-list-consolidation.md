---
id: "BR-MEALS-SHOPPING-LIST-CONSOLIDATION"
type: "business-rule"
title: "Derived shopping lists consolidate ingredients by name and unit"
status: "draft"
applies-to:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-INGREDIENT"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Shopping List Derivation, Notes: Partial specification); docs/_legacy/04_contexts/meal-planning.md (Integration with Lists)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Derivation gathers the ingredients of every recipe slot in the plan; ingredients with the same name (case-insensitive) and the same unit become one list item, and ingredients with different units stay separate items (observed: openspec/specs/meal-planning/spec.md, Shopping List Derivation).

## Rationale

One item per thing to buy keeps the shopping list short (inferred).

## Examples

"Flour 500 g" in two recipes becomes one "flour" item; "500 g flour" and "2 cups flour" remain two items (observed: openspec/specs/meal-planning/spec.md, Notes).

## Exceptions

How quantities are combined when units match is referenced but not specified (observed: openspec/specs/meal-planning/spec.md, Notes "Partial specification: Shopping list unit consolidation"). Confidence medium for that reason.
