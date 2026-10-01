---
id: "SB-MEALS-AGENDA-SHOWS-ASSIGNED-MEAL"
type: "structured-behaviour"
title: "An assigned meal appears in that day's Agenda as a household entry"
status: "draft"
illustrates:
  - "UC-MEALS-SEE-MEALS-IN-AGENDA"
  - "BR-MEALS-AGENDA-PROJECTION"
given:
  - "a meal plan has a recipe slot on a specific day"
when: "a person views the Agenda for that day"
then:
  - "the meal appears as a non-timed household entry"
  - "it is visually distinct from Plans and Tasks"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-AGENDA"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Assigned meal slot appears in the Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Meals are visible in the daily view without becoming calendar items.

## Boundaries

None.
