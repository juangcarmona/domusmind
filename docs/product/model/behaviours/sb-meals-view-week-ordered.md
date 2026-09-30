---
id: "SB-MEALS-VIEW-WEEK-ORDERED"
type: "structured-behaviour"
title: "Viewing a week returns every slot in day and meal-type order"
status: "draft"
illustrates:
  - "UC-MEALS-VIEW-PLAN"
  - "BR-MEALS-MEAL-TYPE-ORDER"
given:
  - "an Active meal plan exists for the current week"
when: "a person views it for the household and week start"
then:
  - "all 35 slots are shown ordered by day from the household's first day of week, then by meal type"
  - "Unplanned slots are included"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household views the current week's meal plan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The whole week is always visible, including the gaps.

## Boundaries

None.
