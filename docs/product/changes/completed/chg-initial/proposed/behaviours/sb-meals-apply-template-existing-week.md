---
id: "SB-MEALS-APPLY-TEMPLATE-EXISTING-WEEK"
type: "structured-behaviour"
title: "Applying a template to a week that has a plan returns that plan"
status: "draft"
illustrates:
  - "UC-MEALS-APPLY-TEMPLATE"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
given:
  - "a meal plan already exists for the household and target week"
when: "a person applies a template to the same week"
then:
  - "the existing plan is returned"
  - "the template is not re-applied"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: A plan already exists for the target week)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Applying a template never overwrites a week already planned.

## Boundaries

None.
