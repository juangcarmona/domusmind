---
id: SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE
type: structured-behaviour
title: A Completed plan refuses slot changes
status: active
illustrates:
  - UC-MEALS-UPDATE-SLOT
  - BR-MEALS-PLAN-LIFECYCLE
given:
  - a meal plan is in Completed status
when: a person tries to update any slot
then:
  - the change is rejected
  - the plan remains unchanged
uses-terms:
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: 'openspec/specs/meal-planning/spec.md (scenario: Household attempts to mutate a Completed plan); interview: product owner decision Q-0033 (E-0158); product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: documentation
---

## Intent

Past weeks are history and stay as they were.

## Boundaries

A household member closes an Active plan with Finish week. Completed is final; reopening has its own refusal example (CHG-STATE-MODEL-COMPLETION).
