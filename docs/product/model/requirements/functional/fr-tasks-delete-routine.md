---
id: FR-TASKS-DELETE-ROUTINE
type: functional-requirement
title: Delete a routine
status: active
derived-from:
  - UC-TASKS-DELETE-ROUTINE
verification:
  - scenario-ref: SB-TASKS-DELETE-ROUTINE
uses-terms:
  - TERM-ROUTINE
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands, Domain Events Emitted); openspec/specs/tasks/spec.md (Notes 5); interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST let a person delete a routine, after which it no longer appears anywhere; this is current product (decided: Q-0009; observed: docs/_legacy/04_contexts/tasks.md, Commands DeleteRoutine; Domain Events Emitted RoutineDeleted). No further behaviour is specified (observed: openspec/specs/tasks/spec.md, Notes 5). Routine.Delete and the DeleteRoutine handler now implement removal (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs; src/backend/DomusMind.Application/Features/Tasks/DeleteRoutine/DeleteRoutineCommandHandler.cs).

## Rationale

Some recurring work ends for good.
