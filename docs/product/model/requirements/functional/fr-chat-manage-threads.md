---
id: FR-CHAT-MANAGE-THREADS
type: functional-requirement
title: Create and use household conversation threads
status: draft
derived-from:
  - UC-CHAT-USE-CONVERSATIONS
  - BR-CHAT-MEMBER-ACCESS
verification:
  - scenario: An authorized household member creates a conversation with household participants, posts a message and later reads the same message from the thread history.
  - scenario: A member who is not a participant cannot retrieve or post messages to a private conversation.
  - scenario: A participant loses access to conversations when their household access is removed.
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
---

## Requirement

The product MUST allow a household member to create household-scoped conversation threads, exchange text messages, and retrieve conversation history. It MUST limit each private thread to its participants and the members' current household access.

## Rationale

Household coordination requires persistent messages while keeping private conversation history inside its intended household and participant boundary.
