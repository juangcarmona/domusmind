---
id: BR-CHAT-ASSISTANT-READ-ONLY
type: business-rule
title: The Chats assistant responds only to explicit authorized invocations
status: draft
applies-to:
  - UC-CHAT-ASK-ASSISTANT
  - BC-CHATS
uses-terms:
  - TERM-ASSISTANT-RESPONSE
  - TERM-CHAT-ASSISTANT-PARTICIPANT
  - TERM-CHAT-MENTION
---

## Rule

The assistant responds only when a member explicitly starts a direct assistant conversation or addresses the assistant in a group. It answers from DomusMind information the invoking member may access, does not present unsupported information as fact and cannot invoke household domain commands. A supported action request may be clarified and staged as an unreviewed Inbox proposal; no household record changes until an authorized member explicitly approves the proposal through Inbox and the owning area's normal rules. An action request in a private 1:1 thread is not copied to the household Inbox.

## Rationale

Making the assistant a recognizable participant improves discoverability without granting it independent authority. Explicit invocation and Inbox review preserve ordinary conversation, privacy and member control.

## Examples

- A member opens a direct assistant conversation and asks what is scheduled next Friday; the assistant answers using authorized information.
- A member mentions the assistant in a group and asks for a supported household action; the assistant clarifies missing details and stages a proposal, but does not execute it.
- A message without an assistant invocation receives no assistant response.
- A provider cannot answer from authorized information; the assistant says so rather than inventing a household fact.

## Exceptions

None.
