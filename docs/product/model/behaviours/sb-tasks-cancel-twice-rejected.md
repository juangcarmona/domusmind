---
id: SB-TASKS-CANCEL-TWICE-REJECTED
type: structured-behaviour
title: Cancelled task cannot be cancelled again
status: draft
illustrates:
  - UC-TASKS-CANCEL-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
given:
  - "A task in cancelled state"
when: A person tries to cancel it again
then:
  - "The operation is rejected"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: openspec/specs/tasks/spec.md (Task Cancellation, scenario Already cancelled task cannot be cancelled again)
  confidence: high
  recovered-from: documentation
---

## Intent

Cancellation happens once.

## Boundaries

None.
