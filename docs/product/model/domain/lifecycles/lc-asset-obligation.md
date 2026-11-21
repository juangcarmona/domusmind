---
id: LC-ASSET-OBLIGATION
type: domain-lifecycle
title: Asset obligation lifecycle
status: active
subject: TERM-ASSET-OBLIGATION
states:
- id: ACTIVE
  title: Active
  initial: true
- id: SUSPENDED
  title: Suspended
- id: CANCELLED
  title: Cancelled
  terminal: true
- id: FINISHED
  title: Finished
  terminal: true
transitions:
- id: SUSPEND
  title: Suspend the requirement
  from:
  - ACTIVE
  to: SUSPENDED
  trigger: An authorized member suspends the obligation
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-MANAGE-OBLIGATION
- id: REACTIVATE
  title: Reactivate the same requirement
  from:
  - SUSPENDED
  to: ACTIVE
  trigger: An authorized member explicitly reactivates the obligation
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-MANAGE-OBLIGATION
- id: CANCEL
  title: Cancel the requirement
  from:
  - ACTIVE
  - SUSPENDED
  to: CANCELLED
  trigger: An authorized member cancels the obligation
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-MANAGE-OBLIGATION
- id: FINISH-COMPLETED
  title: Finish a completed one-time requirement
  from:
  - ACTIVE
  to: FINISHED
  trigger: An authorized member completes the Pending cycle of a one-time obligation
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-COMPLETE-OBLIGATION
- id: FINISH-SKIPPED
  title: Finish a skipped one-time requirement
  from:
  - ACTIVE
  to: FINISHED
  trigger: An authorized member skips the Pending cycle of a one-time obligation
  initiated-by:
  - ACT-HOUSEHOLD-MEMBER
  governed-by:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  realized-by:
  - UC-ASSETS-COMPLETE-OBLIGATION
---

## Purpose

Distinguish an actionable requirement, a temporary suspension, a cancelled requirement and a resolved one-time requirement.

## Invariants

The obligation keeps its Asset, household, identity, recurrence rule and recorded history across its states. An Active obligation has one Pending cycle. Completed, Skipped and Cancelled cycle outcomes are never overwritten by an ordinary lifecycle action. The Asset and the acting member must satisfy the authorization and state guards in BR-ASSET-OBLIGATION-LIFECYCLE.

## State Semantics

Active allows the declared pending-cycle actions. Suspended preserves its pending cycle while refusing completion, skip and rescheduling; only explicit reactivation or cancellation can change its lifecycle. Cancelled and Finished are terminal and retain authorized consultation and exact-operation retry responses. Finished is reached only by resolving a one-time requirement. Due and overdue are derived date conditions of a Pending cycle, not additional obligation states.

## Transition Semantics

Suspend and reactivate retain the pending identity and date. Cancel terminates the requirement and closes its pending cycle without inventing completion or skip. A one-time completion or skip enters Finished. Resolving a recurring cycle leaves the obligation Active and advances its pending cycle under the accepted recurrence rule; it does not enter Finished. Rescheduling changes the Pending cycle without changing obligation state. Actions from undeclared source states are refused without mutation, except exact idempotent retries that return an earlier accepted result and make no new transition.

## Boundaries

Registration establishes Active without a fictitious pre-existence state. The related occurrence lifecycle records cycle outcomes. Archive and restore retain their own Asset lifecycle and do not silently transition obligations. Tasks, Plans, Routines and Lists retain independent ownership. This lifecycle proposes product semantics, not storage, commands, HTTP statuses or implementation completion.
