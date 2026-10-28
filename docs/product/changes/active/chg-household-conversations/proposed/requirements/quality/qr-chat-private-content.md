---
id: QR-CHAT-PRIVATE-CONTENT
type: quality-requirement
title: Protect private conversation content
status: draft
quality-attribute: confidentiality
applies-to:
  - UC-CHAT-USE-CONVERSATIONS
  - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
  - UC-CHAT-ASK-ASSISTANT
  - BC-CHATS
verification:
  - scenario: An authenticated member outside a private thread or group cannot retrieve its metadata, messages, attachments, read state or assistant responses.
  - scenario: Removing household access or group membership prevents subsequent access without requiring a new login.
  - scenario: The assistant's household queries and provider context are limited by the invoking member's existing access.
  - scenario: A quoted reply, mention, notification or attachment reference never reveals content to a nonparticipant.
  - scenario: An assistant action request in a private thread does not copy message text or context into the household Inbox or expose it to nonparticipants.
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-ATTACHMENT
  - TERM-CHAT-READ-STATE
  - TERM-ASSISTANT-RESPONSE
  - TERM-ASSISTANT-PROVIDER-TARGET
---
## Requirement

Private conversation and group content MUST be available only to authorized current participants. This includes metadata, messages, quoted previews, mentions, attachments, history navigation, unread/read state, notifications and assistant responses. Assistant queries MUST respect the invoking member's existing household-data permissions. Assistant action requests in private threads MUST NOT copy message text or context into the household Inbox or expose it to nonparticipants.

## Measurement

For every private-thread and group read/write path, automated authorization scenarios verify that nonparticipants and members without household access receive no conversation content. Assistant-query scenarios verify that provider context includes only information the invoking member may access. Private-thread action requests do not create household Inbox items.
