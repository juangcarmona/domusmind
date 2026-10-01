---
id: SB-TASKS-AGENDA-HIDES-PAUSED-ROUTINE
type: structured-behaviour
title: Paused routine does not appear in the Agenda
status: draft
illustrates:
  - UC-TASKS-SEE-WORK-IN-AGENDA
  - BR-TASKS-PAUSE-RESUME
given:
  - "A routine in paused status"
when: A person views the Agenda for a date matching its recurrence
then:
  - "The routine does not appear"
uses-terms:
  - TERM-ROUTINE
  - TERM-AGENDA
provenance:
  source: openspec/specs/tasks/spec.md (Agenda Projection, scenario Paused routine does not appear in Agenda)
  confidence: high
  recovered-from: documentation
---

## Intent

Paused routines stay out of the way.

## Boundaries

None.
