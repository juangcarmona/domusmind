---
id: SB-INBOX-REJECT-PROPOSAL
type: structured-behaviour
title: A member rejects one pending proposal
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - The proposal is pending.
  - The member has permission to review the Inbox item.
when: The member explicitly rejects the proposal.
then:
  - The proposal becomes rejected.
  - No owning area action is invoked.
  - The decision remains visible with the Inbox item.
covers-transition:
  lifecycle: LC-INBOX-PROPOSAL
  transition: REJECT
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Let the member decline one suggestion while retaining its review history.

## Boundaries

The example concerns one proposal. Other proposals from the same note keep their own decisions. The owning area retains its own authorization and validation rules.
