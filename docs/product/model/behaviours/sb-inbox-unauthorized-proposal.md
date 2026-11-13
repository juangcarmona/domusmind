---
id: SB-INBOX-UNAUTHORIZED-PROPOSAL
type: structured-behaviour
title: A person without household access cannot approve a proposal
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - A pending proposal belongs to a household the person cannot access.
when: The person attempts to approve the proposal.
then:
  - The request is refused.
  - No area action is authorized by the attempt.
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

A person without household access cannot approve a proposal.

## Boundaries

The example concerns one proposal. Other proposals from the same note keep their own decisions. The owning area retains its own authorization and validation rules.
