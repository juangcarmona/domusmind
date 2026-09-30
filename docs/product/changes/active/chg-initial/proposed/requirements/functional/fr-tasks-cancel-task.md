---
id: FR-TASKS-CANCEL-TASK
type: functional-requirement
title: Cancel an open task
status: draft
derived-from:
  - UC-TASKS-CANCEL-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
verification:
  - scenario-ref: "SB-TASKS-CANCEL-TASK"
  - scenario-ref: "SB-TASKS-CANCEL-COMPLETED-REJECTED"
  - scenario-ref: "SB-TASKS-CANCEL-TWICE-REJECTED"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Task Cancellation); interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person cancel a pending task, keeping it in history but removing it from active work, and MUST reject cancelling a completed or already cancelled task (observed: openspec/specs/tasks/spec.md, Requirement Task Cancellation). The lifecycle has no in-progress state (decided: Q-0005).

## Rationale

Work that no longer matters should stop demanding attention without being erased.
