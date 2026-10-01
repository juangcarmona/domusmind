---
id: SB-TASKS-PAUSE-ROUTINE
type: structured-behaviour
title: Active routine paused
status: draft
illustrates:
  - UC-TASKS-PAUSE-ROUTINE
  - BR-TASKS-PAUSE-RESUME
given:
  - "A routine in active status"
when: A person pauses the routine
then:
  - "The routine's status becomes paused"
  - "It no longer appears in the Agenda"
uses-terms:
  - TERM-ROUTINE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Pause, scenario Active routine is paused)
  confidence: high
  recovered-from: documentation
---

## Intent

Pausing hides the routine without deleting it.

## Boundaries

Does not cover a pause until a date (see SB-TASKS-PAUSE-UNTIL-AUTO-RESUME).
