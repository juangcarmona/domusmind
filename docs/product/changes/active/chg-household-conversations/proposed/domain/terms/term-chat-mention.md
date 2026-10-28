---
id: TERM-CHAT-MENTION
type: domain-term
title: Chat mention
status: draft
defined-in: BC-CHATS
synonyms: []
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
---

## Definition

A structured reference from one Chat message to a current participant or to the Chat assistant, rendered as an addressable mention in the conversation.

## Distinguish From

- A plain occurrence of the `@` character is not necessarily a mention.
- Mentioning the assistant is an explicit invocation; mentioning a household member does not invoke the assistant.

## Usage

The product offers only participants visible and eligible in the current thread, and it does not use a mention to widen access to the thread or its history.
