---
id: SB-TASKS-RESCHEDULE-CANCELLED-REJECTED
type: structured-behaviour
title: Cancelled task cannot be rescheduled
status: draft
illustrates:
  - UC-TASKS-RESCHEDULE-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
given:
  - "A task in cancelled state"
when: A person tries to reschedule it
then:
  - "The operation is rejected"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: openspec/specs/tasks/spec.md (Task Rescheduling, scenario Cancelled task cannot be rescheduled)
  confidence: high
  recovered-from: documentation
---

## Intent

Cancelled tasks are final.

## Boundaries

None.
