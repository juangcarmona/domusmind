---
id: UC-TASKS-START-TASK
type: use-case
title: Start a task
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
  source: "docs/_legacy/04_contexts/tasks.md (Commands, Invariants); openspec/specs/tasks/spec.md (Notes 2); absent from src/backend/DomusMind.Domain/Tasks; interview: product owner decision Q-0005 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Goal

Show that work on a task has begun (observed: docs/_legacy/04_contexts/tasks.md, Commands, StartTask; Invariants, Lifecycle).

## Trigger

A person begins the work.

## Preconditions

The task is pending.

## Main Flow

1. The person marks the task as started.
2. The product sets the task's status to in progress.

## Alternative Flows

None stated.

## Failure Conditions

Unspecified: no source defines the rule for entering the in-progress state (observed: openspec/specs/tasks/spec.md, Notes 2), and the current model has no in-progress state (observed: src/backend/DomusMind.Domain/Tasks/Enums/HouseholdTaskStatus.cs).

## Postconditions

The task is in progress and can still be completed or cancelled.

Planned (future), low confidence: the current task lifecycle has no in-progress state (decided: Q-0005).
