---
id: SB-TASKS-COMPLETE-CANCELLED-REJECTED
type: structured-behaviour
title: Cancelled task cannot be completed
status: draft
illustrates:
  - UC-TASKS-COMPLETE-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
given:
  - "A task in cancelled state"
when: A person tries to complete it
then:
  - "The operation is rejected"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: openspec/specs/tasks/spec.md (Task Completion, scenario Cancelled task cannot be completed)
  confidence: high
  recovered-from: documentation
---

## Intent

Cancelled work cannot be recorded as done.

## Boundaries

None.
