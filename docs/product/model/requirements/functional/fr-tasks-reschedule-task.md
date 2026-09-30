---
id: FR-TASKS-RESCHEDULE-TASK
type: functional-requirement
title: Reschedule an open task
status: draft
derived-from:
  - UC-TASKS-RESCHEDULE-TASK
  - BR-TASKS-CLOSED-TASK-FINAL
verification:
  - scenario-ref: "SB-TASKS-RESCHEDULE-PENDING"
  - scenario-ref: "SB-TASKS-RESCHEDULE-COMPLETED-REJECTED"
  - scenario-ref: "SB-TASKS-RESCHEDULE-CANCELLED-REJECTED"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Task Rescheduling)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person change the due date of a task that is not completed or cancelled, without changing its identity, state or assignee, and MUST reject rescheduling a completed or cancelled task (observed: openspec/specs/tasks/spec.md, Requirement Task Rescheduling).

## Rationale

Households re-plan constantly; closed work is history.
