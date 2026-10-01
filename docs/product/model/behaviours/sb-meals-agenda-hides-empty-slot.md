---
id: "SB-MEALS-AGENDA-HIDES-EMPTY-SLOT"
type: "structured-behaviour"
title: "An Unplanned slot without notes does not appear in the Agenda"
status: "draft"
illustrates:
  - "UC-MEALS-SEE-MEALS-IN-AGENDA"
  - "BR-MEALS-AGENDA-PROJECTION"
given:
  - "a slot is Unplanned and has no notes"
when: "a person views the Agenda for that day"
then:
  - "the slot does not appear"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-AGENDA"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Unplanned slot without notes is excluded from Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The Agenda shows only meals with something to say.

## Boundaries

None.
