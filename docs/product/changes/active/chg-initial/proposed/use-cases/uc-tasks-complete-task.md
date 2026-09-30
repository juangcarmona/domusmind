---
id: UC-TASKS-COMPLETE-TASK
type: use-case
title: Complete a task
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-CLOSED-TASK-FINAL
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Task Completion); docs/_legacy/04_contexts/tasks.md (Invariants, Lifecycle); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Record that a piece of household work is done (observed: openspec/specs/tasks/spec.md, Task Completion).

## Trigger

Someone finishes the work.

## Preconditions

The task is pending; the lifecycle has no in-progress state (decided: Q-0005).

## Main Flow

1. The person marks the task as completed.
2. The product sets the task's status to completed.

## Alternative Flows

- Completion may record which person completed the task and when (observed: openspec/specs/tasks/spec.md, Task Completion). The current model records neither (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Failure Conditions

- The task is already completed, or is cancelled: completion is rejected (observed: openspec/specs/tasks/spec.md, scenarios Already completed task cannot be completed again; Cancelled task cannot be completed).

## Postconditions

The task is completed and can no longer return to pending.
