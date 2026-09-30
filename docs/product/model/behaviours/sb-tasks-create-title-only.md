---
id: SB-TASKS-CREATE-TITLE-ONLY
type: structured-behaviour
title: Task created with a title only
status: draft
illustrates:
  - UC-TASKS-CREATE-TASK
  - BR-TASKS-TITLE-REQUIRED
given:
  - "A household exists"
when: A person creates a task with only a title
then:
  - "A task is created in pending state"
  - "The task has no assignee and no due date"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: openspec/specs/tasks/spec.md (Task Creation, scenario Household creates a task with title only)
  confidence: high
  recovered-from: documentation
---

## Intent

A title is the only thing needed to capture work.

## Boundaries

Does not assert anything about where an undated task is shown.
