---
id: BR-CHAT-MEMBER-ACCESS
type: business-rule
title: Chat threads are limited to their household participants
status: draft
applies-to:
  - UC-CHAT-USE-CONVERSATIONS
  - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
  - BC-CHATS
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-GROUP-CONVERSATION
---

## Rule

A Chat thread belongs to one household and can be read or written only by its current participating household members while they have access to that household. A member cannot add a person from another household or expose a thread to a nonparticipant. Group membership is enforced on every read, write, attachment, notification and history path.

## Rationale

Conversation history may contain private household information and must follow both household membership and thread participation boundaries.

## Examples

- A participating household member can read and post in the thread.
- A household member who is not a participant cannot read or post to a private thread or group.
- A member whose household access is removed cannot continue to access that household's threads.
- A quoted message, attachment reference, notification or assistant context never bypasses the same authorization check.

## Exceptions

The DomusMind assistant is not a household member. It may process only the invoking member's authorized context after explicit invocation, under BR-CHAT-ASSISTANT-READ-ONLY.
