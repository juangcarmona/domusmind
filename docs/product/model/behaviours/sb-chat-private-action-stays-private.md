---
id: SB-CHAT-PRIVATE-ACTION-STAYS-PRIVATE
type: structured-behaviour
title: A private Chat action stays out of the household Inbox
status: active
illustrates:
  - UC-CHAT-ASK-ASSISTANT
  - BR-CHAT-ASSISTANT-READ-ONLY
given:
  - An authorized member is in a private Chat thread.
when: The member asks @dm for a supported Plan creation action.
then:
  - The private message is not copied into the household Inbox.
  - The assistant directs the member to capture the request in Inbox themselves.
  - No household Plan is created.
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-INBOX-ITEM
  - TERM-PLAN
---

## Intent

Keep private conversation content within its existing access boundary.

## Boundaries

The member chooses what to enter in Inbox. A Chat request does not authorize a household change.
