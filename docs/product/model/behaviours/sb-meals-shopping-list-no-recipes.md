---
id: "SB-MEALS-SHOPPING-LIST-NO-RECIPES"
type: "structured-behaviour"
title: "A plan without recipe slots cannot produce a shopping list"
status: "draft"
illustrates:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
  - "BR-MEALS-SHOPPING-LIST-REQUIRES-RECIPE"
given:
  - "a meal plan has no recipe slots"
when: "a person requests a shopping list"
then:
  - "the request is rejected"
  - "no shopping list is created"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Plan has no recipe slots assigned)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Only recipes produce things to buy.

## Boundaries

None.
