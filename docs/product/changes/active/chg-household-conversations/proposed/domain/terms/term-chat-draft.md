---
id: TERM-CHAT-DRAFT
type: domain-term
title: Chat draft
status: draft
defined-in: BC-CHATS
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
---

## Definition

Unsent message text, reply context and staged attachments retained for a specific conversation according to the accepted session and device policy.

## Boundaries

A draft is not a sent message, is isolated per conversation and is not visible to other participants until sent.

## Distinguish From

- A draft is not part of shared conversation history.
- Staged attachments belong to the draft until a successful send establishes their message relationship.

## Usage

Draft retention, synchronization and discard behavior follow the accepted session and device policy.
