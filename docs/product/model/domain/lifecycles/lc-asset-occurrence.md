---
id: LC-ASSET-OCCURRENCE
type: domain-lifecycle
title: Asset occurrence lifecycle
status: active
subject: TERM-ASSET-OCCURRENCE
states:
- id: PENDING
  title: Pending
  initial: true
- id: COMPLETED
  title: Completed
  terminal: true
- id: SKIPPED
  title: Skipped
  terminal: true
- id: CANCELLED
  title: Cancelled
  terminal: true
transitions:
- id: COMPLETE
  title: Record completion
  from:
  - PENDING
  to: COMPLETED
  trigger: An authorized member completes the pending cycle
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-COMPLETE-OBLIGATION
- id: SKIP
  title: Record an explicit skip
  from:
  - PENDING
  to: SKIPPED
  trigger: An authorized member explicitly skips the pending cycle
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-COMPLETE-OBLIGATION
- id: RESCHEDULE
  title: Move the same pending cycle
  from:
  - PENDING
  to: PENDING
  trigger: An authorized member explicitly changes the pending cycle date
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-COMPLETE-OBLIGATION
- id: CANCEL
  title: Close the cancelled requirement cycle
  from:
  - PENDING
  to: CANCELLED
  trigger: An authorized member cancels the obligation
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-MANAGE-OBLIGATION
---

## Purpose

Preserve one explicit outcome for each resolved due cycle while retaining a single bounded Pending cycle.

## Invariants

A cycle retains its identity and obligation. Exactly one terminal outcome can win for a cycle. An unresolved past date is still Pending and overdue; time alone creates no terminal outcome and no additional Pending cycles. Rescheduling preserves the same cycle and original recurrence rule. Completed, Skipped and Cancelled cycles are retained and are not ordinarily editable.

## State Semantics

Pending can be resolved or rescheduled only while its obligation and Asset are Active. A Suspended obligation keeps its Pending cycle without allowing those actions. Completed, Skipped and Cancelled are terminal historical outcomes. Cancelled means the requirement was explicitly cancelled; it is not evidence of completed work or a skipped recurrence. Due and overdue derive from the pending household-local date.

## Transition Semantics

Completion records the actual completion date once. Skip records the member's explicit omission once. Reschedule retains an auditable date change while the same cycle stays Pending. Cancellation closes the cycle when the obligation is cancelled. Exact-operation retries return the accepted outcome without a new transition; conflicting actions against a resolved or replaced cycle are refused. A recurring completion or skip gives its obligation one next Pending cycle under the accepted recurrence rule; no future sequence is materialized.

## Boundaries

Suspension and Asset archive do not silently resolve a cycle. Corrections preserve the original outcome and remain separate Asset Record product work. The lifecycle does not require a specific persistence representation, duplicate visible occurrence/record rows, or work in any other bounded context.
