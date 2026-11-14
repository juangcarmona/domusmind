---
id: LC-INBOX-PROPOSAL
type: domain-lifecycle
title: Inbox proposal lifecycle
status: active
subject: TERM-INBOX-PROPOSAL
states:
  - id: PENDING
    title: Pending
    initial: true
  - id: EXECUTING
    title: Executing
  - id: APPLIED
    title: Applied
    terminal: true
  - id: REJECTED
    title: Rejected
    terminal: true
transitions:
  - id: APPROVE
    title: Approve one proposal
    from:
      - PENDING
    to: EXECUTING
    trigger: An authorized member explicitly approves the proposal
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-INBOX-REVIEW-AND-EXECUTION
    realized-by:
      - UC-INBOX-CAPTURE-AND-REVIEW
  - id: APPLY
    title: Record the completed area action
    from:
      - EXECUTING
    to: APPLIED
    trigger: The owning area successfully performs the approved action
    governed-by:
      - BR-INBOX-REVIEW-AND-EXECUTION
    realized-by:
      - UC-INBOX-CAPTURE-AND-REVIEW
  - id: REJECT
    title: Reject one proposal
    from:
      - PENDING
    to: REJECTED
    trigger: An authorized member explicitly rejects the proposal
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-INBOX-REVIEW-AND-EXECUTION
    realized-by:
      - UC-INBOX-CAPTURE-AND-REVIEW
  - id: RECOVER-UNAPPLIED-FAILURE
    title: Return an unapplied failed action for review
    from:
      - EXECUTING
    to: PENDING
    trigger: The approved area action fails without applying its household change
    governed-by:
      - BR-INBOX-REVIEW-AND-EXECUTION
    realized-by:
      - UC-INBOX-CAPTURE-AND-REVIEW
provenance:
  source: 'BR-INBOX-REVIEW-AND-EXECUTION; UC-INBOX-CAPTURE-AND-REVIEW; src/backend/DomusMind.Domain/Inbox/InboxProposal.cs; src/backend/DomusMind.Domain/Inbox/InboxItem.cs; src/backend/DomusMind.Application/Features/Inbox/Execute/ExecuteApprovedInboxProposalHandler.cs; product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: documentation
---

## Purpose

Keep the member's decision separate from the result of the approved action.

## Invariants

Each proposal represents one action. Capturing or interpreting a note leaves that action unapplied. Approval requires household access, and execution must pass the owning area's authorization and validation rules.

## State Semantics

Pending proposals can be refined, approved or rejected. Executing means approval has started the area action; it does not claim that a household record exists. Applied means the area action succeeded and its result is linked to the proposal. Rejected proposals invoke no area action. A failed, unapplied attempt returns to Pending with its failure reason so the member can correct the proposal and approve it again.

## Transition Semantics

Approval raises InboxProposalApproved and starts execution. Successful execution is recorded through InboxProposalApplied. Rejection records InboxProposalChanged with the rejected status. A failed attempt that has not applied its change returns the proposal to Pending. The member can correct it and explicitly approve it again; the product never retries it automatically. Applied proposals cannot execute again. Refinement changes proposal details while it remains pending.

## Boundaries

The owning area retains authority over its records. These states describe the proposal, not the Plan, Task or List Item it refers to. The owner confirmed recovery to Pending for failed, unapplied actions (CHG-STATE-MODEL-COMPLETION). If an action is already Applied, it remains Applied and cannot execute again. This defines member-visible behavior without prescribing a transaction or retry implementation. It does not add a new UI visibility requirement for Executing.
