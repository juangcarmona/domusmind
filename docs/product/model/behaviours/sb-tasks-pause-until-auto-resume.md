---
id: SB-TASKS-PAUSE-UNTIL-AUTO-RESUME
type: structured-behaviour
title: Routine paused until a date resumes on that date
status: draft
illustrates:
  - UC-TASKS-PAUSE-ROUTINE
  - BR-TASKS-PAUSE-UNTIL-RESUMES
given:
  - "A routine is paused until a date"
when: That date arrives
then:
  - "The routine is active again without anyone resuming it"
  - "It appears in the Agenda on matching dates from that date on"
uses-terms:
  - TERM-ROUTINE
  - TERM-AGENDA
provenance:
  source: "openspec/specs/tasks/spec.md (Routine Pause, Notes 8); interview: product owner decision Q-0007 (E-0158)"
  confidence: medium
  recovered-from: interview
---

## Intent

A pause with an end date ends by itself (decided: Q-0007).

## Boundaries

Does not assert that occurrences from the paused period are shown; they are not recreated.
