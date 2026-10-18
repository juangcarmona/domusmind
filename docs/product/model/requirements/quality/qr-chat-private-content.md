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
---

## Requirement

Private conversation content and assistant responses MUST be available only to authorized thread participants. Assistant queries MUST respect the invoking member's existing household-data permissions.

## Measurement

For every private-thread read and write path, automated authorization scenarios verify that non-participants and members without household access receive no conversation content. Assistant-query scenarios verify that the results used are limited to information the invoking member may access.
