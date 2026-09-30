---
id: "SB-MEALS-PROMOTE-TO-ACTIVE"
type: "structured-behaviour"
title: "Promoting a Draft plan makes it the Active plan for the week"
status: "draft"
illustrates:
  - "UC-MEALS-PROMOTE-PLAN"
  - "BR-MEALS-PLAN-LIFECYCLE"
given:
  - "a meal plan exists in Draft status"
when: "a person promotes it to Active"
then:
  - "the plan becomes the Active plan for that household and week"
  - "no other Active plan exists for the same household and week"
uses-terms:
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Plan is promoted from Draft to Active); interview: product owner decision Q-0033 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Defines the moment a plan becomes the household's working plan.

## Boundaries

Activation is an explicit household action only; there is no automatic promotion (decided: Q-0033).
