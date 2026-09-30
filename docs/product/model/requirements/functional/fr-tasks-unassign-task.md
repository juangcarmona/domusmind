---
id: FR-TASKS-UNASSIGN-TASK
type: functional-requirement
title: Remove a task's assignee
status: draft
derived-from:
  - UC-TASKS-UNASSIGN-TASK
verification:
  - scenario: "A person removes the assignee from an assigned task and the task becomes unassigned"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands); openspec/specs/tasks/spec.md (Notes 3); interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST let a person remove a task's assignee, leaving the task unassigned; this is current product (decided: Q-0009; observed: docs/_legacy/04_contexts/tasks.md, Commands, UnassignTask). No further behaviour is specified (observed: openspec/specs/tasks/spec.md, Notes 3), and the current domain model has no way to remove an assignee yet (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Rationale

A task may lose its owner and need to go back to the shared pool.
