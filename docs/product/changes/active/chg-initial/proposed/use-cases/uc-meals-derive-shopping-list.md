---
id: "UC-MEALS-DERIVE-SHOPPING-LIST"
type: "use-case"
title: "Generate a shopping list from the week's meals"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-SHOPPING-LIST-REQUIRES-RECIPE"
  - "BR-MEALS-SHOPPING-LIST-CONSOLIDATION"
  - "BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-MEAL-PLAN"
  - "TERM-INGREDIENT"
  - "TERM-LIST"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Shopping List Derivation); docs/_legacy/00_product/surfaces/meal-planning.md (Header, Relationship with Lists, state 7); docs/_legacy/04_contexts/meal-planning.md (Integration with Lists); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (RequestShoppingList); interview: product owner decision Q-0036 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Know what to buy for the planned meals, as a shared list, in one action.

## Trigger

A person chooses "Generate shopping list" on the week's plan.

## Preconditions

A meal plan exists with at least one recipe slot.

## Main Flow

1. The person requests a shopping list for the plan.
2. The product gathers the ingredients of every recipe slot and consolidates those with the same name and unit.
3. The product creates a new shopping list in Lists with one item per consolidated ingredient.
4. The plan records the new list as its latest shopping list and shows that the list is ready.

## Alternative Flows

- 1a. The plan already has a shopping list and the person requests another: a new list is created and the earlier one is untouched (decided: Q-0036; observed: openspec). The legacy surface does not yet offer re-generation; that is a UI gap.

## Failure Conditions

- The plan has no recipe slot: the request is rejected and no list is created.

## Postconditions

A new shopping list exists in Lists and appears on the Lists surface; checking off items there does not affect the meal plan.
