---
id: SB-TASKS-ASSIGN-NON-MEMBER-REJECTED
type: structured-behaviour
title: Assigning a task to someone outside the household is rejected
status: draft
illustrates:
  - UC-TASKS-ASSIGN-TASK
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
given:
  - "A task exists"
when: The task is assigned to a person identifier that does not belong to the household
then:
  - "The assignment is rejected"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: openspec/specs/tasks/spec.md (Task Assignment, scenario Assigning a task to a non-member is rejected)
  confidence: high
  recovered-from: documentation
---

## Intent

Assignment never crosses the household boundary.

## Boundaries

None.
