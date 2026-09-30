---
id: "SB-MEALS-COPY-PREVIOUS-WEEK"
type: "structured-behaviour"
title: "Copying the previous week reproduces its slots without its shopping list"
status: "draft"
illustrates:
  - "UC-MEALS-COPY-PREVIOUS-WEEK"
  - "BR-MEALS-COPY-EXCLUDES-SHOPPING-LIST"
given:
  - "a meal plan exists for the preceding week"
  - "no plan exists for the target week"
when: "a person copies the previous week"
then:
  - "a Draft meal plan is created for the target week"
  - "all slot assignments of the source plan are copied"
  - "the source plan's shopping list reference is not carried over"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-SHOPPING-LIST-DERIVATION"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household copies the previous week's plan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Ad-hoc reuse is one action and starts shopping fresh.

## Boundaries

None.
