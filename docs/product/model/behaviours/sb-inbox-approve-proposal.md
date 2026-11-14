---
id: SB-INBOX-APPROVE-PROPOSAL
type: structured-behaviour
title: An authorized member approves one pending proposal
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - The proposal is pending.
  - The member has permission to act on the Inbox item and its owning area.
when: The member explicitly approves the proposal.
then:
  - The proposal enters execution through its owning area.
  - Approval alone does not assert that the area action has succeeded.
covers-transition:
  lifecycle: LC-INBOX-PROPOSAL
  transition: APPROVE
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Keep the member's approval distinct from the owning area's result.

## Boundaries

The example concerns one proposal. Other proposals from the same note keep their own decisions. The owning area retains its own authorization and validation rules.
