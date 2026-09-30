---
id: SB-TASKS-CANCEL-COMPLETED-REJECTED
type: structured-behaviour
title: Completed task cannot be cancelled
status: draft
illustrates:
  - UC-TASKS-CANCEL-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
given:
  - "A task in completed state"
when: A person tries to cancel it
then:
  - "The operation is rejected"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: openspec/specs/tasks/spec.md (Task Cancellation, scenario Completed task cannot be cancelled)
  confidence: high
  recovered-from: documentation
---

## Intent

Done work cannot be un-done by cancellation.

## Boundaries

None.
