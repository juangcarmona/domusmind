---
id: UC-CHAT-USE-CONVERSATIONS
type: use-case
title: Household members use private and group conversations
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-CHATS
governed-by:
  - BR-CHAT-MEMBER-ACCESS
  - BR-CHAT-COLLABORATIVE-MESSAGING
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-GROUP-CONVERSATION
  - TERM-CHAT-READ-STATE
---

## Goal

Coordinate with household members through persistent private 1:1 and group conversations that remain easy to understand, reliable to use and bounded by household membership.

## Trigger

A household member opens Chats, starts a private conversation with one household member, or creates or opens a group conversation.

## Preconditions

- The member is authenticated and has access to the household.
- A private conversation's participants belong to that household.
- A group conversation's membership is managed within that household.

## Main Flow

1. The member opens a conversation they participate in or starts a private 1:1 conversation with another household member.
2. The member creates or opens a group conversation when group coordination is appropriate.
3. The member reads the authorized history at the member's unread boundary or chosen position.
4. The member posts a text message, optionally mentioning a participant, replying to an earlier message or attaching permitted content.
5. The message appears in the conversation for its authorized participants and updates their unread/read and notification state according to their preferences.

## Alternative Flows

- A member may start a direct assistant conversation or address the assistant in a group; the assistant behavior is described by UC-CHAT-ASK-ASSISTANT.
- A member may mark a conversation unread, inspect a quoted message or navigate older history.
- A member may manage group membership or profile according to the accepted group administration rules.

## Failure Conditions

- A member who is not a participant cannot read or post to a private conversation or group.
- A member without access to the household cannot read or post in its conversations.
- The product rejects a conversation participant or group member from another household.
- A referenced message or attachment that is not visible to the viewer is not disclosed through a reply, search, notification or preview.

## Postconditions

- A posted message and its permitted references are retained in the conversation history.
- Only authorized participants can access a private conversation, group and their content.
- The viewer's read state and notification behavior reflect the accepted conversation rules.
