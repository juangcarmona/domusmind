---
id: UC-TASKS-UNASSIGN-TASK
type: use-case
title: Remove a task's assignee
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands); openspec/specs/tasks/spec.md (Notes 3); absent from src/backend/DomusMind.Domain/Tasks; interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Goal

Leave a task without an assignee again (observed: docs/_legacy/04_contexts/tasks.md, Commands, UnassignTask). Removing a task's assignee is current product (decided: Q-0009).

## Trigger

The assignee can no longer do the task.

## Preconditions

The task has an assignee.

## Main Flow

1. The person removes the assignee.
2. The product records the task as unassigned.

## Alternative Flows

None stated.

## Failure Conditions

None specified (observed: openspec/specs/tasks/spec.md, Notes 3). The current domain model has no way to remove an assignee yet (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Postconditions

The task has no assignee.
