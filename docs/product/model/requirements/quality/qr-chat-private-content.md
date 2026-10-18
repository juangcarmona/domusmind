---
id: QR-CHAT-PRIVATE-CONTENT
type: quality-requirement
title: Protect private conversation content
status: draft
quality-attribute: confidentiality
applies-to:
  - UC-CHAT-USE-CONVERSATIONS
  - UC-CHAT-ASK-ASSISTANT
verification:
  - scenario: An authenticated member outside a private thread cannot retrieve its thread metadata, messages or assistant responses.
  - scenario: The assistant's household queries for a response are limited by the invoking member's existing access.
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-ASSISTANT-RESPONSE
  - TERM-ASSISTANT-PROVIDER-TARGET
---
## Requirement

Private conversation content and assistant responses MUST be available only to authorized thread participants. Assistant queries MUST respect the invoking member's existing household-data permissions. An assistant action request in a private thread MUST NOT copy message text or context into the household Inbox or expose it to nonparticipants.

## Measurement

For every private-thread read and write path, automated authorization scenarios verify that nonparticipants and members without household access receive no conversation content. Assistant-query scenarios verify that provider context includes only information the invoking member may access. Private-thread action requests do not create household Inbox items.
