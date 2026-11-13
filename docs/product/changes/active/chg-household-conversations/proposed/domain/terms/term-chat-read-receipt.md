---
id: TERM-CHAT-READ-RECEIPT
type: domain-term
title: Chat read receipt
status: draft
defined-in: BC-CHATS
uses-terms:
  - TERM-CHAT-MESSAGE
---

## Definition

A participant-visible indication that a specific message has been read by an authorized participant, subject to the conversation's read-receipt policy.

## Boundaries

A read receipt is distinct from a participant's unread boundary and does not disclose message content to anyone who could not already read the message.

## Distinguish From

- An unread boundary records where a participant resumes reading; a read receipt describes a specific message's observed read state.
- A delivery acknowledgement does not prove that a participant has read the message.

## Usage

Read receipts are shown only to authorized participants and follow the conversation's accepted visibility and privacy policy.
