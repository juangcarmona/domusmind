---
id: SB-MEALS-COMPLETE-PLAN
type: structured-behaviour
title: A member closes the working meal plan
status: active
illustrates:
  - UC-MEALS-COMPLETE-PLAN
  - BR-MEALS-PLAN-LIFECYCLE
given:
  - A household member has access to an Active meal plan.
when: The member chooses Finish week.
then:
  - The meal plan becomes Completed.
  - Its content remains available as read-only history.
covers-transition:
  lifecycle: LC-MEAL-PLAN
  transition: COMPLETE
uses-terms:
  - TERM-MEAL-PLAN
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Let a member decide when the household has finished with a working meal plan.

## Boundaries

The action closes the meal plan; linked Lists retain their own data and lifecycle.
