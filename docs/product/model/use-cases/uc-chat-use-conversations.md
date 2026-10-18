---
id: UC-CHAT-USE-CONVERSATIONS
type: use-case
title: Household members use conversations
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-CHATS
governed-by:
  - BR-CHAT-MEMBER-ACCESS
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
---

## Goal

Coordinate with household members through a persistent conversation.

## Trigger

A household member opens Chats or starts a conversation with household members.

## Preconditions

- The member is authenticated and has access to the household.
- A conversation's participants belong to that household.

## Main Flow

1. The member opens a conversation they participate in or creates a conversation with household members.
2. The member reads the conversation history available to them.
3. The member posts a text message.
4. The message appears in the conversation for its participants.

## Alternative Flows

- A member may start a private conversation with selected household members instead of a household group conversation.
- A member may address `@dm` in a message to ask a question; the assistant behavior is described by UC-CHAT-ASK-ASSISTANT.

## Failure Conditions

- A member who is not a participant cannot read or post to a private conversation.
- A member without access to the household cannot read or post in its conversations.
- The product rejects a conversation that includes a person from another household.

## Postconditions

- A posted message is retained in its conversation history.
- Only authorized participants can access a private conversation and its messages.
