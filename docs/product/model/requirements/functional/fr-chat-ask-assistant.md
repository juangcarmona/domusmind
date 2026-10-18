---
id: FR-CHAT-ASK-ASSISTANT
type: functional-requirement
title: Answer explicit household questions in Chats
status: draft
derived-from:
  - UC-CHAT-ASK-ASSISTANT
  - BR-CHAT-ASSISTANT-READ-ONLY
verification:
  - scenario: A member invokes `@dm` with a question answerable from household information they may access; the assistant replies in the same thread with an answer supported by that information.
  - scenario: A member asks the assistant to change household information; no record changes and the assistant directs the member to Inbox.
  - scenario: A member posts a message without invoking `@dm`; the assistant does not respond.
  - scenario: No authorized query result answers the question; the assistant says it cannot answer from available information.
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-ASSISTANT-RESPONSE
---

## Requirement

The product MUST respond only to messages that explicitly invoke `@dm`. Each response MUST be grounded in DomusMind information available to the invoking member. The assistant MUST NOT create or change household records and MUST direct change requests to Inbox.

## Rationale

Members can ask contextual questions without granting the assistant independent authority or bypassing existing household permissions and review flows.
