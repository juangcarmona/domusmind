---
id: FR-TASKS-COMPLETE-TASK
type: functional-requirement
title: Complete an open task
status: draft
derived-from:
  - UC-TASKS-COMPLETE-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
verification:
  - scenario-ref: "SB-TASKS-COMPLETE-TASK"
  - scenario-ref: "SB-TASKS-COMPLETE-TWICE-REJECTED"
  - scenario-ref: "SB-TASKS-COMPLETE-CANCELLED-REJECTED"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Task Completion); interview: product owner decision Q-0005 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person mark a pending task as completed, and MUST reject completing a task that is already completed or cancelled. Completion MAY record which person completed it and when (observed: openspec/specs/tasks/spec.md, Requirement Task Completion). The lifecycle has no in-progress state (decided: Q-0005). The current model does not record who or when (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs).

## Rationale

Knowing what is done is half of knowing what still needs attention.
