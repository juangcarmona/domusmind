---
id: FR-TASKS-PAUSE-ROUTINE
type: functional-requirement
title: Pause an active routine
status: draft
derived-from:
  - UC-TASKS-PAUSE-ROUTINE
  - BR-TASKS-PAUSE-RESUME
verification:
  - scenario-ref: "SB-TASKS-PAUSE-ROUTINE"
  - scenario-ref: "SB-TASKS-PAUSE-PAUSED-REJECTED"
uses-terms:
  - TERM-ROUTINE
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Routine Pause); interview: product owner decision Q-0007 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person pause an active routine so it stops appearing in the Agenda while keeping its identity and definition, and MUST reject pausing a paused routine. A pause MAY name a date until which it lasts, and the routine then resumes automatically on that date (decided: Q-0007; FR-TASKS-PAUSE-ROUTINE-UNTIL).

## Rationale

Households have seasons; routines should step aside without being lost.
