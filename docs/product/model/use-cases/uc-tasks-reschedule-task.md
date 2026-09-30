---
id: UC-TASKS-RESCHEDULE-TASK
type: use-case
title: Reschedule a task
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
  source: openspec/specs/tasks/spec.md (Task Rescheduling); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs
  confidence: high
  recovered-from: documentation
---

## Goal

Move a task's due date without losing its identity or assignment (observed: openspec/specs/tasks/spec.md, Task Rescheduling).

## Trigger

Plans change and a task needs a new due date.

## Preconditions

The task exists and is neither completed nor cancelled.

## Main Flow

1. The person gives the task a new due date.
2. The product updates the due date; status and assignee stay as they were.

## Alternative Flows

None stated. (inferred: removing the due date is possible in the current model, which has a no-schedule option; src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskSchedule.cs.)

## Failure Conditions

- The task is completed or cancelled: rescheduling is rejected (observed: openspec/specs/tasks/spec.md, scenarios Completed task cannot be rescheduled; Cancelled task cannot be rescheduled).

## Postconditions

The task shows in the Agenda on its new due date.
