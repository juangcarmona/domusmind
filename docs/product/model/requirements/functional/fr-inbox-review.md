---
id: FR-INBOX-REVIEW
type: functional-requirement
title: Review, refine and decide on Inbox proposals
status: active
derived-from:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
verification:
  - scenario-ref: SB-INBOX-APPROVE-PROPOSAL
  - scenario-ref: SB-INBOX-APPLY-PROPOSAL
  - scenario-ref: SB-INBOX-REJECT-PROPOSAL
  - scenario-ref: SB-INBOX-UNAUTHORIZED-PROPOSAL
  - scenario-ref: SB-INBOX-INVALID-PROPOSAL
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Requirement

An authorized household member MUST be able to review proposals, refine a proposal, approve a proposal, or reject a proposal. Approval MUST invoke the owning area behavior for that proposal; rejection MUST leave household state unchanged. The member MUST be able to tell whether a proposal is pending, applied, or rejected.

## Rationale

Members review and decide on each suggested action before it changes household records (issue #17).
