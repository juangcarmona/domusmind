---
id: FR-ASSETS-MANAGE-OBLIGATIONS
type: functional-requirement
title: Define and project asset obligations
status: draft
derived-from:
  - UC-ASSETS-MANAGE-OBLIGATION
  - BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
verification:
  - scenario: "One-time, calendar-based and completion-relative obligations produce the agreed due/overdue Agenda projection without creating a Task, Plan or List item."
---
## Requirement

An authorized member can define, review, suspend and cancel a one-time, calendar-based or completion-relative Asset Obligation. DomusMind calculates its next due/overdue state using explicit timezone/calendar rules and exposes that state as a distinguishable read-only Agenda projection. It does not create a Task, Routine, Plan or List item automatically.

## Rationale

Persistent obligations reduce forgotten maintenance and renewal while preserving the existing ownership of time, execution and lists.
