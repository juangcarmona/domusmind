---
id: SB-TASKS-COMPLETE-TASK
type: structured-behaviour
title: Task marked as completed
status: active
illustrates:
  - UC-TASKS-COMPLETE-TASK
given:
  - A task exists in pending state
when: A person marks it as completed
then:
  - The task's status becomes completed
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Task Completion, scenario Task is marked as completed); interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
covers-transition:
  lifecycle: LC-TASK
  transition: COMPLETE
---

## Intent

Open work can be closed as done.

## Boundaries

The task starts pending. Closed tasks are covered by the separate rejection examples.
