---
id: TERM-TASK-STATUS
type: domain-term
title: Task Status
status: draft
defined-in: BC-TASKS
synonyms:
  - TaskStatus
uses-terms:
  - TERM-TASK
provenance:
  source: "openspec/specs/tasks/spec.md (Task Completion, Task Cancellation, Notes 2); docs/_legacy/04_contexts/tasks.md (Invariants, Lifecycle); src/backend/DomusMind.Domain/Tasks/Enums/HouseholdTaskStatus.cs; interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

The lifecycle state of a task: a task is pending, then completed or cancelled (decided: Q-0005; observed: src/backend/DomusMind.Domain/Tasks/Enums/HouseholdTaskStatus.cs). Completed and cancelled are terminal: a completed task cannot return to pending, a cancelled task cannot be completed, and neither can be cancelled or completed again (observed: openspec/specs/tasks/spec.md, Task Completion, Task Cancellation).

## Distinguish From

- Routine status (TERM-ROUTINE): routines are active or paused, not pending or completed (observed: openspec/specs/tasks/spec.md, Routine Pause).
- In progress: not a task state; the lifecycle has no in-progress step (decided: Q-0005). The legacy sources name it (observed: docs/_legacy/04_contexts/tasks.md, Invariants, Lifecycle; openspec/specs/tasks/spec.md, Notes 2); starting a task stays a low-confidence future item.

## Usage

Cancelled tasks stay in history but are no longer treated as active work (observed: openspec/specs/tasks/spec.md, Task Cancellation). Task overviews group tasks by status (observed: docs/_legacy/04_contexts/tasks.md, Read Models, Task Board).
