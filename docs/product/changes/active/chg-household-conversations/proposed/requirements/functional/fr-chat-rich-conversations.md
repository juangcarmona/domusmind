---
id: FR-CHAT-RICH-CONVERSATIONS
type: functional-requirement
title: Support rich, navigable and notification-aware household conversations
status: draft
derived-from:
  - UC-CHAT-USE-CONVERSATIONS
  - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
  - BR-CHAT-COLLABORATIVE-MESSAGING
  - BR-CHAT-MEMBER-ACCESS
verification:
  - scenario: A participant mentions another participant or the assistant and the picker contains only eligible current participants.
  - scenario: A participant replies to a message with a quoted preview and can navigate to the referenced message without bypassing access.
  - scenario: A participant composes text and stages one or more permitted attachments, navigates away and returns to the same conversation, and finds the draft isolated to that conversation until it is sent or discarded by the accepted session policy.
  - scenario: A participant sends one or more permitted attachments and authorized participants can see upload progress, retry a failed upload, see image miniatures or an image album where applicable, and preview or download files without source-permission widening.
  - scenario: A participant receives an unread indicator, opens the thread at the applicable unread boundary, sees per-message read confirmation according to the accepted policy and can mark the conversation unread again.
  - scenario: Group membership and conversation activity produce the accepted system notifications and read-state behavior without exposing content to nonparticipants.
  - scenario: A participant can search authorized message text and file names, inspect the conversation file list, and navigate a long conversation through paginated history without requiring the product to disclose unauthorized history.
  - scenario: A participant can use the message action menu to copy, reply, forward, pin, amend or delete a message only when the accepted policy permits the action, and a pinned message can be opened at its source.
  - scenario: A participant sees eligible typing or presence signals and receives mention, message and membership notifications according to participant preferences.

  - scenario: On supported web and mobile layouts, each permitted message action is reachable; opening or dismissing the action surface does not accidentally invoke an action, and the accessible name identifies icon-only controls.
  - scenario: An unread boundary advances only while the conversation is actually visible to the participant; navigating away while the thread remains mounted does not clear its unread state.
  - scenario: When a participant is following the latest messages, a multi-line incoming message settles fully above the composer; when they have scrolled away from the latest message, new arrivals do not steal their reading position and an explicit return-to-latest affordance is available.
  - scenario: Missing or failed image miniatures produce a recoverable, accessible file/image fallback; album layout preserves sender identity and each message's individual actions.
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-MENTION
  - TERM-CHAT-QUOTED-REPLY
  - TERM-CHAT-ATTACHMENT
  - TERM-CHAT-READ-STATE
  - TERM-CHAT-READ-RECEIPT
  - TERM-CHAT-DRAFT
  - TERM-CHAT-PINNED-MESSAGE
  - TERM-CHAT-PRESENCE
  - TERM-CHAT-IMAGE-ALBUM
---

## Requirement

The product MUST support the rich conversation capabilities accepted for this change: participant and `@all` mentions with eligible autocomplete and notifications; quoted replies with jump-to-source; reliable live delivery and retry; per-conversation drafts; permitted single and multi-file attachments with progress, retry, image miniatures and album grouping; file-list, preview, download and referenced-document behavior; member-specific unread boundaries and per-message read receipts; paginated history with position preservation; search over authorized message text and file names; pins; typing and presence; notifications and preferences; forwarding with destination authorization and attachment handling; and message action menus with policy-governed copy, reply, edit, delete, pin and forward actions. Every capability MUST preserve the thread's participant boundary and household authorization. Message editing, deletion, forwarding, attachment retention, notification channels and read-confirmation detail MUST follow explicit product rules rather than client-only conventions.

The conversation interface MUST keep core interactions perceivable and operable on supported layouts: message actions MUST remain reachable and dismiss without unintended execution; unread state MUST advance only while a thread is actually visible; automatic following MUST not displace a participant who is reading older history; incoming content MUST remain visible above the composer; and missing media MUST have an accessible, recoverable fallback. Native and web controls MAY differ when they provide equivalent authorized actions and clear, accessible behavior.

## Rationale

A conversation becomes dependable for household coordination when members can address the right participant, preserve context, share permitted files and return to unread history without losing privacy or traceability.
