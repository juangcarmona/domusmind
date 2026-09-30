---
id: SB-TASKS-CREATE-WITH-ASSIGNEE-DUE
type: structured-behaviour
title: Task created with an assignee and a due date
status: draft
illustrates:
  - UC-TASKS-CREATE-TASK
given:
  - "A person of the household exists"
when: A task is created with a title, that person as assignee and a due date
then:
  - "A task is created in pending state"
  - "It is assigned to that person and due on the given date"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: openspec/specs/tasks/spec.md (Task Creation, scenario Household creates a task with an assignee and due date)
  confidence: high
  recovered-from: documentation
---

## Intent

Assignee and due date can be set at creation.

## Boundaries

None.
