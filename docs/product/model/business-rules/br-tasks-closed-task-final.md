---
id: BR-TASKS-CLOSED-TASK-FINAL
type: business-rule
title: Completed and cancelled tasks are final
status: draft
applies-to:
  - UC-TASKS-COMPLETE-TASK
  - UC-TASKS-CANCEL-TASK
  - UC-TASKS-RESCHEDULE-TASK
  - UC-TASKS-ASSIGN-TASK
  - UC-TASKS-RENAME-TASK
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Task Completion, Task Cancellation, Task Rescheduling); docs/_legacy/04_contexts/tasks.md (Invariants, Lifecycle); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: observation
---

## Rule

Once a task is completed or cancelled it MUST NOT return to pending, be completed or cancelled again, or be rescheduled; only pending tasks can be completed, cancelled or rescheduled. The lifecycle has no in-progress state (decided: Q-0005).

## Rationale

Completed and cancelled tasks are history; changing them would make the record of household work unreliable (observed: openspec/specs/tasks/spec.md, Task Completion, Task Cancellation, Task Rescheduling; docs/_legacy/04_contexts/tasks.md, Invariants, Lifecycle).

## Examples

- Completing a completed task, completing a cancelled task, cancelling a completed or cancelled task, and rescheduling a completed or cancelled task are all rejected (observed: openspec/specs/tasks/spec.md, scenarios of Task Completion, Task Cancellation, Task Rescheduling).
- The current model also refuses to assign, reassign, rename or recolour a completed or cancelled task (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Exceptions

None.
