---
id: SB-TASKS-RESUME-ROUTINE
type: structured-behaviour
title: Paused routine resumed
status: draft
illustrates:
  - UC-TASKS-RESUME-ROUTINE
  - BR-TASKS-PAUSE-RESUME
given:
  - "A routine in paused status"
when: A person resumes the routine
then:
  - "The routine's status becomes active"
  - "It appears in the Agenda again"
  - "No occurrences from the paused period are added retroactively"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Resume, scenario Paused routine is resumed)
  confidence: high
  recovered-from: documentation
---

## Intent

Resuming restores the routine going forward only.

## Boundaries

None.
