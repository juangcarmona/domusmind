---
id: "SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE"
type: "structured-behaviour"
title: "A Completed plan refuses slot changes"
status: "draft"
illustrates:
  - "UC-MEALS-UPDATE-SLOT"
  - "BR-MEALS-PLAN-LIFECYCLE"
given:
  - "a meal plan is in Completed status"
when: "a person tries to update any slot"
then:
  - "the change is rejected"
  - "the plan remains unchanged"
uses-terms:
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household attempts to mutate a Completed plan); interview: product owner decision Q-0033 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Past weeks are history and stay as they were.

## Boundaries

Does not say how a plan becomes Completed. Undecided, deferred (Q-0033): the Completed trigger is left to the Meal Planning refinement change.
