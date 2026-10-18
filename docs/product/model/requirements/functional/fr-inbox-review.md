---
id: FR-INBOX-REVIEW
type: functional-requirement
title: Review, refine and decide on Inbox proposals
status: draft
derived-from:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
verification:
  - scenario-ref: SB-INBOX-REVIEW-PROPOSAL
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Requirement

An authorized household member MUST be able to review proposals, refine a proposal, approve a proposal, or reject a proposal. Approval MUST invoke the owning area behavior for that proposal; rejection MUST leave household state unchanged. The member MUST be able to tell whether a proposal is pending, applied, or rejected.

## Rationale

Review makes AI assistance a tool for household members rather than an authority over the household (issue #17).
