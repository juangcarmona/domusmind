---
id: SB-TASKS-COMPLETE-TASK
type: structured-behaviour
title: Task marked as completed
status: draft
illustrates:
  - UC-TASKS-COMPLETE-TASK
given:
  - "A task exists in pending state"
when: A person marks it as completed
then:
  - "The task's status becomes completed"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Task Completion, scenario Task is marked as completed); interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Open work can be closed as done.

## Boundaries

The legacy source also allows completion from in progress; the lifecycle has no in-progress state (decided: Q-0005).
