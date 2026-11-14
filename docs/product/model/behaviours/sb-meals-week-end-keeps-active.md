---
id: SB-MEALS-WEEK-END-KEEPS-ACTIVE
type: structured-behaviour
title: An Active plan stays open at the week boundary
status: active
illustrates:
  - BR-MEALS-PLAN-LIFECYCLE
given:
  - A meal plan is Active.
  - No member has chosen Finish week.
when: The calendar week ends.
then:
  - The meal plan remains Active.
  - The plan is not completed automatically.
uses-terms:
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Leave closure with the household even when its calendar week has ended.

## Boundaries

The example sets no deadline for the member to finish the week.
