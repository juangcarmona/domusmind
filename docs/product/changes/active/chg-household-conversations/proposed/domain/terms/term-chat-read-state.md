---
id: TERM-CHAT-READ-STATE
type: domain-term
title: Chat read state
status: draft
defined-in: BC-CHATS
synonyms:
  - Unread state
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
---

## Definition

The member-specific record used to show unread messages, the position at which a member resumes a thread and, where accepted, which participants have read a message.

## Distinguish From

- A notification is an alert about activity; read state is part of conversation history behavior.
- Opening a thread and marking a conversation as unread are distinct member actions.

## Usage

Read state never grants access to a message. It is calculated only over the messages available to that member under the thread's participant and membership rules.
