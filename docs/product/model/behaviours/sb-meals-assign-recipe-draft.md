---
id: SB-MEALS-ASSIGN-RECIPE-DRAFT
type: structured-behaviour
title: A draft meal plan accepts a recipe assignment
status: active
illustrates:
  - UC-MEALS-UPDATE-SLOT
given:
  - A meal plan in Draft status exists.
  - A recipe from the household library is available.
when: a person assigns the recipe to a specific day and meal type
then:
  - the slot's meal source is Recipe with that recipe
  - the slot is updated within the plan
uses-terms:
  - TERM-MEAL-SLOT
  - TERM-RECIPE
  - TERM-MEAL-SOURCE
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household assigns a recipe to a slot)"
  confidence: high
  recovered-from: documentation
---

## Intent

The member can assign a recipe while the meal plan is draft.

## Boundaries

The example covers an unlocked slot. Completed plans and locked slots have separate refusal examples.
