---
id: "SB-MEALS-VIEW-RECIPE-SLOT-DETAILS"
type: "structured-behaviour"
title: "Recipe slots show the recipe's name, servings and times"
status: "draft"
illustrates:
  - "UC-MEALS-VIEW-PLAN"
given:
  - "a meal plan has one or more recipe slots"
when: "a person views the plan"
then:
  - "each recipe slot shows the recipe name, servings and time information"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household views a plan with recipe slots)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

People can judge the week without opening each recipe.

## Boundaries

None.
