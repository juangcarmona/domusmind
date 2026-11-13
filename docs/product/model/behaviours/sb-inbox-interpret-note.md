---
id: SB-INBOX-INTERPRET-NOTE
type: structured-behaviour
title: Interpreting a note presents a reviewable suggestion
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - A member has captured a note in the household Inbox.
  - Interpretation is available and the configured provider can suggest a supported action from the note.
when: The member requests interpretation of the note.
then:
  - The original note remains available.
  - The suggested action is shown as an unapplied proposal with any unresolved details.
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Show the successful interpretation path while keeping the original note and the member's review decision.

## Boundaries

Interpretation is an optional capability under FR-INBOX-INTERPRET. This example does not require every note to produce a proposal.
