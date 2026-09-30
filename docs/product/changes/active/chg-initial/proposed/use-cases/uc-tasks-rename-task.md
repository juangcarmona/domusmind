---
id: UC-TASKS-RENAME-TASK
type: use-case
title: Rename a task
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-TITLE-REQUIRED
  - BR-TASKS-CLOSED-TASK-FINAL
uses-terms:
  - TERM-TASK
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands); openspec/specs/tasks/spec.md (Notes 4, no feature spec); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: observation
---

## Goal

Correct or clarify what a task is called (observed: docs/_legacy/04_contexts/tasks.md, Commands, RenameTask; src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs, Rename). Renaming a task is current product (decided: Q-0009).

## Trigger

A task's title is wrong or unclear.

## Preconditions

The task is neither completed nor cancelled (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Main Flow

1. The person gives the task a new title.
2. The product updates the title.

## Alternative Flows

None stated.

## Failure Conditions

- The new title is empty, or the task is completed or cancelled: renaming is rejected (observed: code only).

## Postconditions

The task carries the new title.
