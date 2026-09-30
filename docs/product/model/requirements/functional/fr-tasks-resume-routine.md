---
id: FR-TASKS-RESUME-ROUTINE
type: functional-requirement
title: Resume a paused routine
status: draft
derived-from:
  - UC-TASKS-RESUME-ROUTINE
  - BR-TASKS-PAUSE-RESUME
verification:
  - scenario-ref: "SB-TASKS-RESUME-ROUTINE"
  - scenario-ref: "SB-TASKS-RESUME-ACTIVE-REJECTED"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Routine Resume)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person resume a paused routine so it becomes active and appears in the Agenda again, without retroactively adding occurrences from the paused period, and MUST reject resuming a routine that is not paused (observed: openspec/specs/tasks/spec.md, Requirement Routine Resume).

## Rationale

Coming back from a pause should not flood the Agenda with stale work.
