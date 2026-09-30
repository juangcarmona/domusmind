---
id: SB-TASKS-RESCHEDULE-PENDING
type: structured-behaviour
title: Pending task rescheduled
status: draft
illustrates:
  - UC-TASKS-RESCHEDULE-TASK
given:
  - "A task exists in pending state"
when: A person gives it a new due date
then:
  - "The task's due date is updated"
  - "The task keeps its state and assignee"
uses-terms:
  - TERM-TASK
provenance:
  source: openspec/specs/tasks/spec.md (Task Rescheduling, scenario Due date of a pending task is updated)
  confidence: high
  recovered-from: documentation
---

## Intent

Rescheduling changes only the due date.

## Boundaries

None.
