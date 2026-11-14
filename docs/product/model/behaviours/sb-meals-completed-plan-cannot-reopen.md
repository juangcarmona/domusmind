---
id: SB-MEALS-COMPLETED-PLAN-CANNOT-REOPEN
type: structured-behaviour
title: A Completed meal plan cannot reopen
status: active
illustrates:
  - BR-MEALS-PLAN-LIFECYCLE
given:
  - A meal plan is Completed.
when: A member attempts to reopen the plan.
then:
  - The request is refused.
  - The plan remains Completed and read-only.
uses-terms:
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Keep a closed meal plan as a stable record.

## Boundaries

Copying a previous week into a new Draft retains its existing separate rules.
