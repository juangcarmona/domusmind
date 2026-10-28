---
id: FR-CHAT-ASK-ASSISTANT
type: functional-requirement
title: Speak with or mention the assistant in Chats and stage supported actions for review
status: draft
derived-from:
  - UC-CHAT-ASK-ASSISTANT
  - BR-CHAT-ASSISTANT-READ-ONLY
  - BR-CHAT-MEMBER-ACCESS
verification:
  - scenario: A member starts a direct assistant conversation and receives an answer grounded in household information they may access.
  - scenario: A member mentions the assistant in a group and receives an answer or focused clarification in that group.
  - scenario: A member requests a supported action; the assistant stages an unreviewed Inbox proposal without changing a household record.
  - scenario: A member posts an ordinary message without invoking the assistant; the assistant does not respond or receive it as provider input.
  - scenario: No authorized query result answers the question; the assistant says it cannot answer from available information.
  - scenario: A private-thread action request is not copied to the household Inbox.
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-ASSISTANT-PARTICIPANT
  - TERM-ASSISTANT-RESPONSE
  - TERM-INBOX-PROPOSAL
  - TERM-ASSISTANT-PROVIDER-TARGET
---

## Requirement

The product MUST provide a deliberate direct assistant conversation and MUST allow a member to explicitly mention the assistant in a group. Each response MUST be grounded in information available to the invoking member. The assistant MUST NOT invoke household domain commands directly. For supported create or update requests, it MAY ask focused clarification questions and stage an unreviewed Inbox proposal; no household record may change until an authorized member explicitly approves that proposal through Inbox and the owning area's normal rules. A private-thread action request MUST NOT be copied to the household Inbox. The response MUST identify the provider target that handled it and whether fallback occurred.

## Rationale

Members should be able to use DomusMind's assistant naturally without learning a magic token or granting it independent authority over household state.
