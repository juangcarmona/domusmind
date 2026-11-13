---
id: SB-CHAT-STAGE-PLAN-PROPOSAL
type: structured-behaviour
title: A household Chat stages a Plan proposal for review
status: active
illustrates:
  - UC-CHAT-ASK-ASSISTANT
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-CHAT-ASSISTANT-READ-ONLY
  - BR-INBOX-REVIEW-AND-EXECUTION
when: The member explicitly asks @dm to prepare that Plan.
given:
  - An authorized member is in a household Chat thread.
  - The assistant has the required details for a supported Plan creation request.
then:
  - An unreviewed Plan proposal is staged in Inbox.
  - No Plan is created by the Chat response.
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
  - TERM-PLAN
---

## Intent

Save proposal-entry effort while leaving review and Calendar validation with their existing owners.

## Boundaries

The member reviews the proposal in Inbox. Calendar applies it only after explicit approval and its own authorization and validation.
