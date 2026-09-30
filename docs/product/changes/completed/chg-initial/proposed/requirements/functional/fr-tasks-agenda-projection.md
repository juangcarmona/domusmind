---
id: FR-TASKS-AGENDA-PROJECTION
type: functional-requirement
title: Show tasks and routines in the Agenda
status: draft
derived-from:
  - UC-TASKS-SEE-WORK-IN-AGENDA
  - BR-TASKS-ROUTINES-PROJECTED
  - BR-AGENDA-PROJECTION-READ-ONLY
verification:
  - scenario-ref: "SB-TASKS-AGENDA-SHOWS-DUE-TASK"
  - scenario-ref: "SB-TASKS-AGENDA-SHOWS-ROUTINE-OCCURRENCE"
  - scenario-ref: "SB-TASKS-AGENDA-HIDES-PAUSED-ROUTINE"
uses-terms:
  - TERM-TASK
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
  - TERM-AGENDA
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Agenda Projection)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST show each task that has a due date in the Agenda on its due date, and each active routine on every date matching its recurrence. Showing them MUST NOT create or modify tasks or routines (observed: openspec/specs/tasks/spec.md, Requirement Agenda Projection).

## Rationale

The household sees all of its time-relevant work in one place while each area keeps owning its own things.
