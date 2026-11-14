---
id: SB-INBOX-INVALID-PROPOSAL
type: structured-behaviour
title: The owning area refuses an invalid approved action
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - A proposal has been explicitly approved.
  - The proposed action fails the owning area validation rules.
when: The owning area validates the proposed action.
then:
  - The action is not applied.
  - The member receives the correction needed.
  - The proposal is Pending and retains its details for correction and explicit reapproval.
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Preserve the owning area's validation rules when an action comes through Inbox.

## Boundaries

The example concerns one proposal. Other proposals from the same note keep their own decisions. The owner confirmed that failed, unapplied proposals return to Pending with their failure reason. Reapproval is explicit and the owning area checks the corrected action again.
