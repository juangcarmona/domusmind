---
id: "SB-MEALS-CLEAR-SLOT"
type: "structured-behaviour"
title: "Clearing a slot returns it to Unplanned without removing it"
status: "draft"
illustrates:
  - "UC-MEALS-UPDATE-SLOT"
  - "BR-MEALS-FULL-SLOT-GRID"
given:
  - "a meal plan has a populated slot"
when: "a person clears that slot"
then:
  - "the slot's meal source is Unplanned"
  - "any previous recipe or free text is removed"
  - "the slot remains in the grid"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-SOURCE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household clears a slot)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Clearing changes content, never structure.

## Boundaries

None.
