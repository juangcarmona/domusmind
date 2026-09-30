---
id: BR-TASKS-VALID-ROUTINE-RECURRENCE
type: business-rule
title: A routine needs a valid recurrence
status: draft
applies-to:
  - UC-TASKS-CREATE-ROUTINE
  - UC-TASKS-UPDATE-ROUTINE
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-RECURRENCE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Creation); docs/_legacy/04_contexts/tasks.md (Routine Integrity); src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs
  confidence: high
  recovered-from: observation
---

## Rule

A routine MUST have a recurrence with a frequency of Daily, Weekly, Monthly or Yearly; Weekly MUST select at least one day of the week, Monthly at least one day of the month, and Yearly a month of the year and at least one day of the month.

## Rationale

A routine without a resolvable set of dates could never be shown (observed: openspec/specs/tasks/spec.md, Routine Creation; docs/_legacy/04_contexts/tasks.md, Invariants, Routine Integrity).

## Examples

- A weekly routine with no days selected is rejected (observed: openspec/specs/tasks/spec.md, scenario Routine with invalid recurrence is rejected).
- Days of the month must be 1 to 31 and the month 1 to 12 (observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs).
- If an end time is given, a time must be given too and the end time must be later (observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs).

## Exceptions

None.
