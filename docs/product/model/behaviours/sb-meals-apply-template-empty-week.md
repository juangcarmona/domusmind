---
id: "SB-MEALS-APPLY-TEMPLATE-EMPTY-WEEK"
type: "structured-behaviour"
title: "Applying a template to an empty week creates a pre-filled Draft"
status: "draft"
illustrates:
  - "UC-MEALS-APPLY-TEMPLATE"
  - "BR-MEALS-TEMPLATE-SNAPSHOT"
given:
  - "a weekly template exists"
  - "no plan exists for the target week"
when: "a person applies the template to the target week"
then:
  - "a Draft meal plan is created with slots filled from the template"
  - "slots not covered by the template are Unplanned"
  - "the plan records which template was applied"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household applies a template to an empty week)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Reuse of a pattern in one action.

## Boundaries

None.
