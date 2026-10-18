---
id: BR-CHAT-ASSISTANT-READ-ONLY
type: business-rule
title: The Chats assistant responds only to explicit read-only questions
status: draft
applies-to:
  - UC-CHAT-ASK-ASSISTANT
uses-terms:
  - TERM-ASSISTANT-RESPONSE
---

## Rule

The assistant responds only when a member explicitly invokes `@dm`. It answers from DomusMind information the invoking member may access, does not modify household state, and must not present unsupported information as fact. If a member asks it to make a change, it directs the member to Inbox for review.

## Rationale

Explicit invocation keeps ordinary conversation separate from assistant use. Read-only behavior preserves existing authorization and the Inbox decision boundary for household changes.

## Examples

- A member asks `@dm` what is scheduled next Friday; the assistant answers using the member's authorized household information.
- A member asks `@dm` to add a Plan; the assistant does not create one and directs the member to Inbox.
- A message without `@dm` receives no assistant response.

## Exceptions

None.
