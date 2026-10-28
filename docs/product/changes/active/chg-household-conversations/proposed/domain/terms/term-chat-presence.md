---
id: TERM-CHAT-PRESENCE
type: domain-term
title: Chat presence and typing signal
status: draft
defined-in: BC-CHATS
uses-terms:
  - TERM-CHAT-THREAD
---

## Definition

A transient indication that an authorized participant is online or composing in a conversation, subject to participant privacy and availability policy.

## Boundaries

Presence and typing are not messages, are not retained as conversation history and must not reveal private activity to nonparticipants.

## Distinguish From

- Presence indicates availability; typing indicates transient composition activity.
- Neither signal is a delivery, read or participation record.

## Usage

The product applies the accepted heartbeat, expiry and privacy behavior and may suppress either signal per participant preference.
