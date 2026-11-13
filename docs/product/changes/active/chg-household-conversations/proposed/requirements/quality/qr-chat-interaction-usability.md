---
id: QR-CHAT-INTERACTION-USABILITY
type: quality-requirement
title: Keep household conversation interactions perceivable and operable
status: draft
quality-attribute: usability
applies-to:
  - UC-CHAT-USE-CONVERSATIONS
  - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
  - UC-CHAT-ASK-ASSISTANT
verification:
  - scenario: On each supported web and mobile layout, all actions permitted to the current participant are reachable, have accessible names, and can be dismissed without triggering an action.
  - scenario: Read state advances only while the relevant conversation is actually visible; a mounted but off-screen conversation does not consume unread messages.
  - scenario: When following the latest message, incoming content including multi-line text is fully visible above the composer after layout settles; while reading older history, new arrivals do not move the reading position and a clear return-to-latest affordance is available.
  - scenario: Missing, delayed, or failed image miniatures provide an accessible and recoverable fallback; grouped media preserves sender identity and individual message actions.
  - scenario: A typing indicator is presented as transient activity near the composer, clears when the participant sends, and expires when activity stops; the label uses neutral product language.
uses-terms:
  - TERM-CHAT-READ-STATE
  - TERM-CHAT-IMAGE-ALBUM
  - TERM-CHAT-ATTACHMENT
  - TERM-CHAT-PRESENCE
provenance:
  source: Prior implementation evidence reviewed 2026-10-19 (issue discussions, change reviews, usability screenshots, regression tests, and device checks)
  confidence: medium
  recovered-from: documentation
---

## Requirement

The conversation interface MUST keep core interactions perceivable and operable on supported web and mobile layouts. Every action available to a participant MUST be reachable and have an accessible name; opening or dismissing an action surface MUST NOT execute an action. Read state MUST advance only while the conversation is actually visible. Automatic following MUST keep incoming content visible above the composer after layout settles and MUST NOT move a participant who is reading older history; a clear return-to-latest affordance MUST be available. Missing or delayed media MUST have an accessible, recoverable fallback that preserves the sender and message context. Typing indicators MUST be presented as transient activity near the composer, clear when a message is sent, and expire when activity stops; their wording MUST use the product's own neutral language.

## Measurement

For each supported layout and input mode in the conversation test matrix, verify that 100% of actions authorized for the test participant can be reached and dismissed without activation, and that icon-only controls expose accessible names. Automated navigation tests MUST prove that off-screen mounted conversations do not advance read state. Automated scroll tests MUST prove that a multi-line arrival is fully visible above the composer when following latest, while an older-history position remains stable on arrival. Media tests MUST cover missing/failed thumbnails, fallback accessibility, and album preservation of sender identity and per-message actions. Typing-indicator tests MUST verify placement, clearing on send, expiry, and neutral wording; live cross-device delivery evidence MUST be reported separately when not verified. Record screenshots or equivalent runtime evidence for representative desktop and mobile flows; distinguish runtime evidence from unit/CI-only results.
