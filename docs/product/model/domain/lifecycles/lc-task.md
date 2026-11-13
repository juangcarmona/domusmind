---
id: LC-TASK
type: domain-lifecycle
title: Task lifecycle
status: active
subject: TERM-TASK
states:
  - id: PENDING
    title: Pending
    initial: true
  - id: COMPLETED
    title: Completed
    terminal: true
  - id: CANCELLED
    title: Cancelled
    terminal: true
transitions:
  - id: COMPLETE
    title: Complete the task
    from:
      - PENDING
    to: COMPLETED
    trigger: A household member marks the task as completed
    initiated-by: &a1
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-TASKS-CLOSED-TASK-FINAL
    realized-by:
      - UC-TASKS-COMPLETE-TASK
  - id: CANCEL
    title: Cancel the task
    from:
      - PENDING
    to: CANCELLED
    trigger: A household member cancels the task
    initiated-by: *a1
    governed-by:
      - BR-TASKS-CLOSED-TASK-FINAL
    realized-by:
      - UC-TASKS-CANCEL-TASK
provenance:
  source: TERM-TASK; BR-TASKS-CLOSED-TASK-FINAL; UC-TASKS-COMPLETE-TASK; UC-TASKS-CANCEL-TASK; product owner decision Q-0005; src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs
  confidence: high
  recovered-from: documentation
---

## Purpose

Show whether household work still needs attention or has been closed.

## Invariants

A task belongs to one household. Only a pending task can be completed or cancelled. Completed and cancelled tasks remain in history and cannot return to pending.

## State Semantics

Pending is open work. Completed records work marked as done. Cancelled records work the household no longer intends to do.

## Transition Semantics

Completion and cancellation each close a pending task. Their corresponding domain events record the successful change; an attempted action on a closed task makes no transition.

## Boundaries

Rescheduling and assignment keep the task pending. A Routine defines recurrence separately; it does not create Tasks. There is no in-progress task state in the accepted lifecycle.
