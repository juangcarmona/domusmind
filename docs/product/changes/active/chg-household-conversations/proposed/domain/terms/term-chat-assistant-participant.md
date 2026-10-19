---
id: TERM-CHAT-ASSISTANT-PARTICIPANT
type: domain-term
title: Chat assistant participant
status: draft
defined-in: BC-CHATS
synonyms:
  - DomusMind assistant
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-ASSISTANT-RESPONSE
---

## Definition

The clearly identified DomusMind assistant presence that a member can enter as a direct conversation or address explicitly inside a group. It is a product participant, not a household member and not an independently authorized actor.

## Distinguish From

- A household member has household identity and permissions; the assistant borrows the invoking member's authorization for each request.
- An Assistant Provider Target is the configured endpoint that processes a request; it is not a conversation participant.

## Usage

The assistant participant responds only to explicit invocations and never receives more household context than the invoking member may access.
