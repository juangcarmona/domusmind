---
id: BR-CHAT-MEMBER-ACCESS
type: business-rule
title: Chat threads are limited to their household participants
status: draft
applies-to:
  - UC-CHAT-USE-CONVERSATIONS
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
---

## Rule

A Chat thread belongs to one household and can be read or written only by its participating members while they have access to that household. A member cannot add a person from another household to the thread.

## Rationale

Conversation history may contain private household information and must follow household membership boundaries.

## Examples

- A participating household member can read and post messages in the thread.
- A household member who is not a participant cannot read a private thread or post to it.
- A member whose household access has been removed cannot continue to access that household's threads.

## Exceptions

None.
