---
id: BR-TASKS-PAUSE-UNTIL-RESUMES
type: business-rule
title: A routine paused until a date resumes on that date
status: draft
applies-to:
  - UC-TASKS-PAUSE-ROUTINE
  - UC-TASKS-RESUME-ROUTINE
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: "openspec/specs/tasks/spec.md (Routine Pause, Notes 8); interview: product owner decision Q-0007 (E-0158)"
  confidence: medium
  recovered-from: interview
---

## Rule

A pause MAY name a date until which it lasts; a routine paused until a date MUST resume automatically on that date, without a person resuming it.

## Rationale

Pause until a date is part of the baseline, and the routine resumes automatically on that date (decided: Q-0007). The openspec names the pause-until date but left its effect open (observed: openspec/specs/tasks/spec.md, Routine Pause, Notes 8).

## Examples

- A routine "Weekly trash" paused until a date during a holiday appears again in the Agenda from that date on (inferred example).
- The current model has no pause-until date yet (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs).

## Exceptions

None. Occurrences from the paused period are not recreated, as for any resume (BR-TASKS-PAUSE-RESUME).
