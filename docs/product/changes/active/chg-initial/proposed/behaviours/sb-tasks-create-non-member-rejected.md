---
id: SB-TASKS-CREATE-NON-MEMBER-REJECTED
type: structured-behaviour
title: Task with an assignee from outside the household is rejected
status: draft
illustrates:
  - UC-TASKS-CREATE-TASK
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
given:
  - "A person identifier that does not belong to the household"
when: A task is created with that person as assignee
then:
  - "The task is not created"
  - "A validation error is returned"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: openspec/specs/tasks/spec.md (Task Creation, scenario Creating a task with a non-member assignee is rejected)
  confidence: high
  recovered-from: documentation
---

## Intent

Assignees must come from the task's household.

## Boundaries

None.
