---
id: "SB-MEALS-CREATE-PLAN-EXISTING-WEEK"
type: "structured-behaviour"
title: "Creating a plan for a week that has one returns the existing plan"
status: "draft"
illustrates:
  - "UC-MEALS-CREATE-PLAN"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
given:
  - "a meal plan already exists for the household and week"
when: "a person creates another meal plan for the same week"
then:
  - "the existing plan is returned"
  - "no duplicate plan is created"
uses-terms:
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: A meal plan already exists for the target week)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Creation is safe to repeat and never forks a week.

## Boundaries

None.
