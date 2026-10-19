---
id: TERM-CHAT-QUOTED-REPLY
type: domain-term
title: Chat quoted reply
status: draft
defined-in: BC-CHATS
synonyms:
  - Reply
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-THREAD
---

## Definition

A Chat message that references an earlier message in the same thread and presents a compact preview with navigation back to the referenced history.

## Distinguish From

- Quoting is not copying the earlier text into a new message.
- Forwarding, if accepted later, sends content into another conversation and is a separate operation.

## Usage

A quoted reply never permits access to a referenced message that the viewer could not otherwise read.
