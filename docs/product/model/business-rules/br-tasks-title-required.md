---
id: BR-TASKS-TITLE-REQUIRED
type: business-rule
title: A task needs only a title
status: draft
applies-to:
  - UC-TASKS-CREATE-TASK
  - UC-TASKS-RENAME-TASK
uses-terms:
  - TERM-TASK
provenance:
  source: openspec/specs/tasks/spec.md (Task Creation); src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskTitle.cs
  confidence: high
  recovered-from: observation
---

## Rule

A task MUST have a non-empty title; every other input (assignee, due date, area) is optional.

## Rationale

Capturing work should take less effort than remembering it; a title alone is enough to create a task (observed: openspec/specs/tasks/spec.md, Task Creation).

## Examples

- "Buy groceries" with no assignee and no due date is a valid task (observed: openspec/specs/tasks/spec.md, scenario Household creates a task with title only).
- In the current model the title is trimmed and limited to 200 characters (observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskTitle.cs).

## Exceptions

None.
