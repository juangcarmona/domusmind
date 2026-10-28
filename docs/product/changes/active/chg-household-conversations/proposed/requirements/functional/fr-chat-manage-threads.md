---
id: FR-CHAT-MANAGE-THREADS
type: functional-requirement
title: Create and use household private and group conversation threads
status: draft
derived-from:
  - UC-CHAT-USE-CONVERSATIONS
  - BR-CHAT-MEMBER-ACCESS
  - BR-CHAT-GROUP-MEMBERSHIP
verification:
  - scenario: An authorized household member creates a private 1:1 conversation with another household member, posts a message and later reads it from history.
  - scenario: An authorized household member creates or joins a group, posts a message and sees it only through the group's participant boundary.
  - scenario: A member who is not a participant cannot retrieve or post messages to a private conversation or group.
  - scenario: A participant loses access to conversations when their household access or group membership is removed.
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-GROUP-CONVERSATION
---

## Requirement

The product MUST allow a household member to create and use household-scoped private 1:1 and group conversation threads, exchange persistent messages and retrieve authorized history. It MUST limit each thread to its current participants and the members' current household access. Group membership changes MUST apply consistently to thread reads, writes, files, notifications, assistant context and history navigation.

## Rationale

Household coordination needs both focused private conversations and shared groups while keeping conversation history inside its intended participant and household boundary.
