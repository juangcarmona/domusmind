---
id: FR-TASKS-PAUSE-ROUTINE-UNTIL
type: functional-requirement
title: Pause a routine until a date
status: draft
derived-from:
  - UC-TASKS-PAUSE-ROUTINE
  - BR-TASKS-PAUSE-UNTIL-RESUMES
verification:
  - scenario-ref: "SB-TASKS-PAUSE-UNTIL-AUTO-RESUME"
uses-terms:
  - TERM-ROUTINE
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Routine Pause, Notes 8); interview: product owner decision Q-0007 (E-0158)"
  confidence: medium
  recovered-from: interview
---

## Requirement

The product MUST let a person pause an active routine until a given date, and MUST resume that routine automatically on that date (decided: Q-0007). The current model has no pause-until date yet (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs).

## Rationale

Most pauses are for a known period, such as a holiday; the routine should come back without anyone having to remember it.
