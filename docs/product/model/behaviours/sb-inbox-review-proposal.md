---
id: SB-INBOX-REVIEW-PROPOSAL
type: structured-behaviour
title: A member approves or rejects an Inbox proposal
status: draft
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
when: A member makes an explicit decision on a pending proposal.
given:
  - The member has permission to act on the Inbox item and its owning area.
then:
  - An approved proposal is passed to the normal behavior of its owning area.
  - A rejected proposal does not change household state.
  - The decision and resulting status remain visible with the Inbox item.
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Keep members in control of every household change suggested from an Inbox note.

## Boundaries

Approval is explicit and applies to one proposal. It does not authorize unrelated proposals from the same note or bypass the owning area's rules.
