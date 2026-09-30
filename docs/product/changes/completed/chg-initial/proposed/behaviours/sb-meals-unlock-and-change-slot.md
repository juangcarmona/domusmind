---
id: "SB-MEALS-UNLOCK-AND-CHANGE-SLOT"
type: "structured-behaviour"
title: "Unlocking and changing a locked slot happen in one change"
status: "draft"
illustrates:
  - "UC-MEALS-UPDATE-SLOT"
  - "BR-MEALS-LOCKED-SLOT"
given:
  - "a slot is locked"
when: "a person submits a content change that also unlocks the slot"
then:
  - "the slot is unlocked"
  - "the content change is applied in the same operation"
uses-terms:
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household unlocks and mutates a locked slot)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

A deliberate change to a locked slot needs no separate unlock step.

## Boundaries

None.
