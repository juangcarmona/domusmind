---
id: "SB-MEALS-SHOPPING-LIST-FROM-PLAN"
type: "structured-behaviour"
title: "Requesting a shopping list creates a list of consolidated ingredients"
status: "draft"
illustrates:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
  - "BR-MEALS-SHOPPING-LIST-CONSOLIDATION"
given:
  - "a meal plan has at least one recipe slot"
  - "the recipe has at least one ingredient"
when: "a person requests a shopping list"
then:
  - "a shopping list is created in Lists with one item per consolidated ingredient"
  - "the meal plan records the list and increases its derivation version"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-LIST"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household requests a shopping list from an active plan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Shopping follows from the plan in one action.

## Boundaries

Does not define how quantities combine when units match.
