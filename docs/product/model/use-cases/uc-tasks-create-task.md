---
id: UC-TASKS-CREATE-TASK
type: use-case
title: Create a task
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-TITLE-REQUIRED
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
  - BR-TASKS-TASKS-CREATED-EXPLICITLY
  - BR-TASKS-AREA-CATEGORISES-ONLY
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
  - TERM-TASK-STATUS
  - TERM-AREA
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/tasks/spec.md (Task Creation); docs/_legacy/04_contexts/tasks.md (Responsibilities); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0058 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person records a concrete piece of household work so it is not held in anyone's memory (observed: openspec/specs/tasks/spec.md, Task Creation).

## Trigger

A person decides some household work needs doing.

## Preconditions

The household exists and the person belongs to it.

## Main Flow

1. The person gives the task a title.
2. Optionally, the person picks an assignee from the household, a due date (a date, or a date and time), and an area for grouping (observed: openspec/specs/tasks/spec.md, Task Creation; src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskSchedule.cs).
3. The product creates the task in pending state with a manual origin.

## Alternative Flows

- Title only: the task is created with no assignee and no due date (observed: openspec/specs/tasks/spec.md, scenario Household creates a task with title only).
- The person may also give a description and a colour (decided: Q-0058; observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Failure Conditions

- The assignee is not a person of the household: the task is not created and a validation error is shown (observed: openspec/specs/tasks/spec.md, scenario Creating a task with a non-member assignee is rejected).
- The title is empty: the task is not created (observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskTitle.cs).

## Postconditions

A pending task exists; if it has a due date it appears in the Agenda on that date (observed: openspec/specs/tasks/spec.md, Agenda Projection).
