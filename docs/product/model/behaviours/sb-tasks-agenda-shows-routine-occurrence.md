---
id: SB-TASKS-AGENDA-SHOWS-ROUTINE-OCCURRENCE
type: structured-behaviour
title: Active routine appears on its scheduled days
status: draft
illustrates:
  - UC-TASKS-SEE-WORK-IN-AGENDA
  - BR-TASKS-ROUTINES-PROJECTED
given:
  - "An active routine recurs weekly on Tuesdays"
when: A person views the Agenda for a Tuesday
then:
  - "The routine appears as a projected occurrence on that day"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
  - TERM-AGENDA
provenance:
  source: openspec/specs/tasks/spec.md (Agenda Projection, scenario Active routine appears on its scheduled dates)
  confidence: high
  recovered-from: documentation
---

## Intent

Routines are visible where they matter, on their days.

## Boundaries

The occurrence is not a task and is not stored.
