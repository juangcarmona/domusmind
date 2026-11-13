---
id: BR-CHAT-COLLABORATIVE-MESSAGING
type: business-rule
title: Conversation collaboration stays inside the authorized thread
status: draft
applies-to:
  - UC-CHAT-USE-CONVERSATIONS
  - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
  - BC-CHATS
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-MENTION
  - TERM-CHAT-QUOTED-REPLY
  - TERM-CHAT-ATTACHMENT
  - TERM-CHAT-READ-STATE
  - TERM-CHAT-READ-RECEIPT
  - TERM-CHAT-DRAFT
  - TERM-CHAT-PINNED-MESSAGE
  - TERM-CHAT-PRESENCE
  - TERM-CHAT-IMAGE-ALBUM
---

## Rule

Messages, mentions, quoted replies, attachments, drafts, image albums, unread/read markers, read receipts, presence, typing, pins and notifications are evaluated within the conversation and the viewer's current authorization. These capabilities may improve coordination but must not widen the set of people who can read the referenced content. Message amendments, forwarding and attachment references require an explicit product rule and leave no silent access bypass.

## Rationale

The familiar conversation features are useful only when they preserve the privacy and traceability of household coordination.

## Examples

- A mention picker contains only eligible current participants and the assistant where applicable.
- A quoted reply can navigate only to a message the viewer is allowed to read.
- An unread counter includes only messages inside the viewer's authorized history.
- A referenced household document keeps the source document's permissions.
- A draft and its staged attachments remain isolated to the conversation where they were composed.
- A pinned message, read receipt, typing signal or presence signal is visible only according to the conversation's participant and privacy rules.
- Consecutive authorized image attachments may be presented as an album without changing their individual access or download behavior.

## Exceptions

Forwarding, message amendment visibility, attachment retention and notification channels remain subject to the open decisions in CHG-HOUSEHOLD-CONVERSATIONS-001.
