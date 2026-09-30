---
id: "FR-MEALS-SHOPPING-LIST-DERIVATION"
type: "functional-requirement"
title: "Derive a shopping list from a meal plan"
status: "draft"
derived-from:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
  - "BR-MEALS-SHOPPING-LIST-REQUIRES-RECIPE"
  - "BR-MEALS-SHOPPING-LIST-CONSOLIDATION"
  - "BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME"
verification:
  - scenario-ref: "SB-MEALS-SHOPPING-LIST-FROM-PLAN"
  - scenario-ref: "SB-MEALS-SHOPPING-LIST-RE-REQUEST"
  - scenario-ref: "SB-MEALS-SHOPPING-LIST-NO-RECIPES"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-LIST"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Shopping List Derivation); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (RequestShoppingList)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household derive a shopping list from a plan with at least one recipe slot, consolidating ingredients by name (ignoring case) within the same unit; each request MUST create a new shopping List in Lists without changing earlier ones, and the plan MUST keep a reference to the latest list and a derivation version (observed: openspec/specs/meal-planning/spec.md, Shopping List Derivation).

## Rationale

Turns the week's plan into what to buy in one action, handing ownership to Lists.
