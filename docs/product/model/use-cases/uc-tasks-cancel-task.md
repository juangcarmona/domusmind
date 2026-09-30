---
id: UC-TASKS-CANCEL-TASK
type: use-case
title: Cancel a task
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
  source: "openspec/specs/tasks/spec.md (Task Cancellation); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Stop a task that should no longer be done while keeping it in history (observed: openspec/specs/tasks/spec.md, Task Cancellation).

## Trigger

The work is no longer needed.

## Preconditions

The task is pending; the lifecycle has no in-progress state (decided: Q-0005).

## Main Flow

1. The person cancels the task.
2. The product sets the task's status to cancelled and no longer treats it as active work.

## Alternative Flows

None.

## Failure Conditions

- The task is completed or already cancelled: cancellation is rejected (observed: openspec/specs/tasks/spec.md, scenarios Completed task cannot be cancelled; Already cancelled task cannot be cancelled again).

## Postconditions

The task is cancelled, kept in history, and cannot be completed or return to pending.
