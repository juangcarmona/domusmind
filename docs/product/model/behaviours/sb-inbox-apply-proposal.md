---
id: SB-INBOX-APPLY-PROPOSAL
type: structured-behaviour
title: An approved proposal records its successful result
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - The proposal is executing after explicit approval.
  - The owning area accepts the action under its authorization and validation rules.
when: The owning area completes the approved action.
then:
  - The proposal becomes applied.
  - The result remains linked to the Inbox item.
covers-transition:
  lifecycle: LC-INBOX-PROPOSAL
  transition: APPLY
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Give the member a traceable result once the owning area completes the action.

## Boundaries

The example concerns one proposal. Other proposals from the same note keep their own decisions. The owning area retains its own authorization and validation rules.
