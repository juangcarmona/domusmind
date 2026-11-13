---
id: SB-TASKS-CANCEL-TASK
type: structured-behaviour
title: Task cancelled
status: active
illustrates:
  - UC-TASKS-CANCEL-TASK
given:
  - A task exists in pending state
when: A person cancels the task
then:
  - The task's status becomes cancelled
  - It is no longer treated as active work
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Task Cancellation, scenario Task is cancelled); interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
covers-transition:
  lifecycle: LC-TASK
  transition: CANCEL
---

## Intent

Unneeded work leaves active work but stays in history.

## Boundaries

The task starts pending. Closed tasks are covered by the separate rejection examples.
