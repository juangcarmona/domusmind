---
id: "SB-MEALS-SHOPPING-LIST-RE-REQUEST"
type: "structured-behaviour"
title: "Requesting another shopping list leaves the earlier one untouched"
status: "draft"
illustrates:
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
  - "BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME"
given:
  - "a meal plan already has a shopping list"
when: "a person requests a new shopping list"
then:
  - "a new shopping list is created"
  - "the previous shopping list is not modified"
  - "the meal plan's derivation version increases"
uses-terms:
  - "TERM-SHOPPING-LIST-DERIVATION"
  - "TERM-LIST"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household re-requests a shopping list); interview: product owner decision Q-0036 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Replanning never disturbs a list already in use.

## Boundaries

The legacy surface does not expose this re-request yet; each request generating a new list is current product (decided: Q-0036).
