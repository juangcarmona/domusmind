---
id: BR-TASKS-PAUSE-RESUME
type: business-rule
title: Only active routines pause and only paused routines resume
status: draft
applies-to:
  - UC-TASKS-PAUSE-ROUTINE
  - UC-TASKS-RESUME-ROUTINE
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: "openspec/specs/tasks/spec.md (Routine Pause, Routine Resume); src/backend/DomusMind.Domain/Tasks/Routine.cs; interview: product owner decision Q-0007 (E-0158)"
  confidence: high
  recovered-from: observation
---

## Rule

A routine MUST be active to be paused and paused to be resumed. A paused routine MUST NOT appear in the Agenda, and resuming it MUST NOT recreate the occurrences missed while it was paused.

## Rationale

Pausing is a temporary stop that keeps the routine's identity and definition (observed: openspec/specs/tasks/spec.md, Routine Pause, Routine Resume).

## Examples

- Pausing a paused routine or resuming an active routine is rejected (observed: openspec/specs/tasks/spec.md, scenarios Paused routine cannot be paused again; Non-paused routine cannot be resumed).

## Exceptions

None. A pause may name a date until which it lasts; on that date the routine resumes automatically (decided: Q-0007; see BR-TASKS-PAUSE-UNTIL-RESUMES).
