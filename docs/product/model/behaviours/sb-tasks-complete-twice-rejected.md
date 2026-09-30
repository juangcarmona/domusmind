---
id: SB-TASKS-COMPLETE-TWICE-REJECTED
type: structured-behaviour
title: Completed task cannot be completed again
status: draft
illustrates:
  - UC-TASKS-COMPLETE-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
given:
  - "A task in completed state"
when: A person tries to complete it again
then:
  - "The operation is rejected"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: openspec/specs/tasks/spec.md (Task Completion, scenario Already completed task cannot be completed again)
  confidence: high
  recovered-from: documentation
---

## Intent

Completion happens once.

## Boundaries

None.
