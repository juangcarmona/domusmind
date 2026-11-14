---
id: SB-INBOX-CAPTURE-NOTE
type: structured-behaviour
title: A member records a household note
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
when: An authorized household member captures a text note in Inbox.
given:
  - The member is authorized to access the household Inbox.
then:
  - DomusMind stores the original note as an Inbox item.
  - Capturing the note does not create a Plan, Task or List Item.
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Intent

Let the member save a thought before deciding what action it needs.

## Boundaries

The member can leave the note unreviewed. Interpretation and approval are separate actions.
