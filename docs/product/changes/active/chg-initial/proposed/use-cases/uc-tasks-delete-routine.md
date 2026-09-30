---
id: UC-TASKS-DELETE-ROUTINE
type: use-case
title: Delete a routine
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by: []
uses-terms:
  - TERM-ROUTINE
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands, Domain Events Emitted); openspec/specs/tasks/spec.md (Notes 5); absent from src/backend/DomusMind.Domain/Tasks; interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Goal

Remove a routine that no longer applies (observed: docs/_legacy/04_contexts/tasks.md, Commands, DeleteRoutine; Domain Events Emitted, RoutineDeleted). Deleting a routine is current product (decided: Q-0009).

## Trigger

The household stops doing the recurring work for good.

## Preconditions

The routine exists.

## Main Flow

1. The person deletes the routine.
2. The product removes it; it no longer appears anywhere.

## Alternative Flows

None stated.

## Failure Conditions

None specified (observed: openspec/specs/tasks/spec.md, Notes 5). The current domain model has no deletion for routines yet (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs).

## Postconditions

The routine no longer exists.
