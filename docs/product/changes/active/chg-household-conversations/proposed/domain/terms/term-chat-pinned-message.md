---
id: TERM-CHAT-PINNED-MESSAGE
type: domain-term
title: Chat pinned message
status: draft
defined-in: BC-CHATS
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-THREAD
---

## Definition

A conversation message selected for persistent prominence so authorized participants can inspect its current excerpt and jump to its source.

## Boundaries

Pinning does not copy the message outside the conversation. A withdrawn message cannot remain an active pin, and an amended message updates the pinned excerpt according to the accepted traceability policy.

## Distinguish From

- A pin is a navigation aid, not a duplicate message or a new conversation.
- A pinned message remains subject to the source message's access, amendment and withdrawal rules.

## Usage

The product presents the accepted pinned-message banner and jump behavior only to authorized conversation participants.
