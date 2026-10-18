---
id: SB-INBOX-CAPTURE-AND-INTERPRET
type: structured-behaviour
title: A member captures a note and receives proposals
status: draft
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
when: A household member captures a note that can be interpreted as a Plan or Task.
given:
  - The member is authorized to access the household Inbox.
then:
  - DomusMind stores the original note.
  - DomusMind may display one or more structured proposals and unresolved details.
  - No Plan or Task is created until the member explicitly approves its proposal.
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Reduce the effort of capturing and structuring household information without moving the decision from the member to the system.

## Boundaries

The member can leave the note unreviewed. A proposal is not an existing household record. The owning area remains responsible for validation and authorization when the member approves it.
