---
id: FR-ASSETS-MANAGE-OBLIGATIONS
type: functional-requirement
title: Define and project asset obligations
status: draft
derived-from:
  - UC-ASSETS-MANAGE-OBLIGATION
  - BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
  - BR-ASSET-RECURRENCE-DUE-STATE
verification:
  - scenario: "One-time, calendar-based and completion-relative obligations produce one pending due/overdue Agenda projection without creating a Task, Plan or List item; a monthly last-day rule stays on the last valid day."
---
## Requirement

An authorized member can define, review, suspend and cancel a one-time, calendar-based or completion-relative Asset Obligation. First-release due dates are household-local calendar dates without exact times. DomusMind exposes one pending due cycle as a distinguishable read-only Agenda projection; an unresolved cycle stays overdue until explicitly resolved. A monthly last-day rule uses the last valid day of each month, and completion-relative recurrence advances from the actual completion date. Explicit skip records the omission and calculates the next due state; rescheduling changes only the pending cycle, not the rule. It does not create a Task, Routine, Plan or List item automatically.

## Rationale

Persistent obligations reduce forgotten maintenance and renewal while preserving the existing ownership of time, execution and lists.
