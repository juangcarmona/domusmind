---
id: "SB-MEALS-CREATE-PLAN-FOR-WEEK"
type: "structured-behaviour"
title: "Creating a plan for a week gives a Draft with every slot Unplanned"
status: "draft"
illustrates:
  - "UC-MEALS-CREATE-PLAN"
  - "BR-MEALS-FULL-SLOT-GRID"
given:
  - "a household exists with a configured first day of week"
when: "a person creates a meal plan for a valid week start"
then:
  - "a Draft meal plan exists for that household and week"
  - "all 35 slots (7 days by 5 meal types) are Unplanned"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household creates a meal plan for the current week)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that a plan always starts complete in structure and empty in content.

## Boundaries

None.
