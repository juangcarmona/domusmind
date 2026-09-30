---
id: "SB-MEALS-COPY-NO-PREVIOUS-PLAN"
type: "structured-behaviour"
title: "Copying when last week has no plan is a recoverable outcome"
status: "draft"
illustrates:
  - "UC-MEALS-COPY-PREVIOUS-WEEK"
given:
  - "no meal plan exists for the preceding week"
when: "a person copies the previous week"
then:
  - "the product reports that there is no previous plan, without a hard failure"
  - "the person can still start from scratch or apply a template"
uses-terms:
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: No source plan exists for the preceding week)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

A missing source week never blocks planning.

## Boundaries

None.
