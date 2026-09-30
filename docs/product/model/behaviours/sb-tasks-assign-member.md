---
id: SB-TASKS-ASSIGN-MEMBER
type: structured-behaviour
title: Task assigned to a person
status: draft
illustrates:
  - UC-TASKS-ASSIGN-TASK
given:
  - "A task exists"
  - "A person of the household exists"
when: The task is assigned to that person
then:
  - "The task records that person as its assignee"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: openspec/specs/tasks/spec.md (Task Assignment, scenario Task is assigned to a member)
  confidence: high
  recovered-from: documentation
---

## Intent

Assignment after creation is supported.

## Boundaries

Does not cover completed or cancelled tasks.
