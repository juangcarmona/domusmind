---
id: FR-TASKS-DELETE-ROUTINE
type: functional-requirement
title: Delete a routine
status: draft
derived-from:
  - UC-TASKS-DELETE-ROUTINE
verification:
  - scenario: "A person deletes a routine and it no longer appears in the Agenda"
uses-terms:
  - TERM-ROUTINE
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands, Domain Events Emitted); openspec/specs/tasks/spec.md (Notes 5); interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST let a person delete a routine, after which it no longer appears anywhere; this is current product (decided: Q-0009; observed: docs/_legacy/04_contexts/tasks.md, Commands DeleteRoutine; Domain Events Emitted RoutineDeleted). No further behaviour is specified (observed: openspec/specs/tasks/spec.md, Notes 5), and the current domain model has no deletion for routines yet (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs).

## Rationale

Some recurring work ends for good.
