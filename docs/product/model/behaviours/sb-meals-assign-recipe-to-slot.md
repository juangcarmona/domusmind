---
id: "SB-MEALS-ASSIGN-RECIPE-TO-SLOT"
type: "structured-behaviour"
title: "Assigning a recipe fills the slot with that recipe"
status: "draft"
illustrates:
  - "UC-MEALS-UPDATE-SLOT"
given:
  - "a meal plan in Draft or Active status exists"
  - "a recipe from the household library is available"
when: "a person assigns the recipe to a specific day and meal type"
then:
  - "the slot's meal source is Recipe with that recipe"
  - "the slot is updated within the plan"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-RECIPE"
  - "TERM-MEAL-SOURCE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household assigns a recipe to a slot)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Shows both Draft and Active plans accept slot assignment.

## Boundaries

None.
