---
id: UC-TASKS-ASSIGN-TASK
type: use-case
title: Assign a task to a person
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
  - BR-TASKS-EXPLICIT-ASSIGNMENT
  - BR-TASKS-CLOSED-TASK-FINAL
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
  - TERM-MEMBER
provenance:
  source: openspec/specs/tasks/spec.md (Task Assignment); docs/_legacy/04_contexts/tasks.md (Invariants, Assignment); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs
  confidence: high
  recovered-from: documentation
---

## Goal

Make clear who is responsible for doing a task (observed: openspec/specs/tasks/spec.md, Task Assignment).

## Trigger

A person chooses who should do an existing task.

## Preconditions

The task exists and is neither completed nor cancelled (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs, Assign).

## Main Flow

1. The person picks a person of the household as the task's assignee.
2. The product records that person as the assignee.

## Alternative Flows

- Reassignment: the task already has an assignee; the new person replaces them (observed: openspec/specs/tasks/spec.md, Task Assignment, "Assignment may be changed after creation"; src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs, Reassign).

## Failure Conditions

- The chosen person is not in the household: the assignment is rejected (observed: openspec/specs/tasks/spec.md, scenario Assigning a task to a non-member is rejected).
- The task is completed or cancelled: the assignment is rejected (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Postconditions

The task has exactly one assignee from the household.
