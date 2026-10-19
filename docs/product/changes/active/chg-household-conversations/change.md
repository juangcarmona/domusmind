---
id: CHG-HOUSEHOLD-CONVERSATIONS-001
type: product-change
title: Expand Chats into rich family conversations with an AI assistant participant
status: draft
base-revision: 'b4f77ad01ca87d2de33682d24ea2b7eee9e4cb15'
operations:
  add:
    - TERM-CHAT-GROUP-CONVERSATION
    - TERM-CHAT-ASSISTANT-PARTICIPANT
    - TERM-CHAT-MENTION
    - TERM-CHAT-QUOTED-REPLY
    - TERM-CHAT-ATTACHMENT
    - TERM-CHAT-READ-STATE
    - TERM-CHAT-READ-RECEIPT
    - TERM-CHAT-DRAFT
    - TERM-CHAT-PINNED-MESSAGE
    - TERM-CHAT-PRESENCE
    - TERM-CHAT-IMAGE-ALBUM
    - BR-CHAT-GROUP-MEMBERSHIP
    - BR-CHAT-COLLABORATIVE-MESSAGING
    - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
    - FR-CHAT-RICH-CONVERSATIONS
    - QR-CHAT-INTERACTION-USABILITY
  modify:
    - BC-CHATS
    - TERM-CHAT-THREAD
    - TERM-CHAT-MESSAGE
    - TERM-ASSISTANT-RESPONSE
    - BR-CHAT-MEMBER-ACCESS
    - BR-CHAT-ASSISTANT-READ-ONLY
    - UC-CHAT-USE-CONVERSATIONS
    - UC-CHAT-ASK-ASSISTANT
    - FR-CHAT-MANAGE-THREADS
    - FR-CHAT-ASK-ASSISTANT
    - QR-CHAT-PRIVATE-CONTENT
  remove: []
---

## Problem

DomusMind's accepted Chats definition is too small for family coordination. It describes persistent text threads and an explicitly invoked assistant, but it does not define the conversation model needed for a familiar WhatsApp-like experience: durable private 1:1 conversations, named group conversations, membership administration, rich messages, replies, mentions, unread/read state, attachments, notifications and message-history navigation. It also does not define the assistant as a participant that can be addressed directly or from a group while preserving household authorization and Inbox review.

The existing assistant issue (#204) correctly identifies the need for a deliberate assistant conversation, but its scope does not cover the broader conversation substrate.

## Intended Product Outcome

DomusMind defines Chats as family-only conversations with two ordinary forms: private 1:1 threads between selected household members and group threads with managed household membership. Members can exchange persistent messages and use a complete family-chat experience: reliable real-time delivery with retry, per-conversation drafts, mentions including `@all`, quoted replies to old messages, copy and forwarding, attachments with progress/retry, image miniatures and albums, file lists with preview/download, unread boundaries, per-message read confirmations, notifications, typing/presence, pinning, search, paginated history and traceable message amendments where the product rules permit them.

The DomusMind assistant is a recognizable participant in a direct assistant conversation and can be explicitly addressed from a group. It answers using only information the invoking member may access. A supported request may be clarified and staged as an Inbox proposal; the assistant does not bypass household authorization or create household records directly. No non-family participant or external messaging channel is included in this change.

## Rationale

Prior implementation and usability evidence shows group conversations require a coherent access boundary, membership lifecycle, messaging substrate, unread/read model, files, navigation, and notification behavior. A review of usability screenshots, defect reports, regression tests, and device checks also surfaced transferable interaction lessons: keep one household list that distinguishes 1:1 and group threads; keep message actions reachable and safely dismissible; advance read state only while a conversation is visible; preserve the reader's position when new content arrives; provide accessible media fallbacks; and present typing as transient activity near the composer.

Evidence strength varied. Some findings had screenshots or targeted automated/device checks, while other paths lacked a runtime session, manual web verification, or live cross-device delivery. These lessons inform the proposed quality bar; DomusMind must verify its own supported surfaces rather than assume those results transfer unchanged.

This draft is tracked by [Epic #213: Family conversations with groups and the @dm assistant](https://github.com/juangcarmona/domusmind/issues/213). The Epic owns the ordered child-feature backlog and the intended sequence of stacked implementation pull requests.

DomusMind already has the right household authorization and review-first assistant boundaries. Extending Chats through a Product Change lets the richer experience be defined without weakening those boundaries or silently turning DomusMind into a general-purpose messenger. It also gives #204 a coherent home instead of delivering an assistant-only slice against an underspecified conversation model.

## Affected Product Areas

- Chats conversation forms, family participants and history.
- Household member access and group membership administration.
- Message composition, mentions, quoted replies, attachments, unread/read state, notifications and history navigation.
- Message drafts, image miniatures and albums, file lists, search, pinning, forwarding, typing/presence and message action menus.
- Assistant invocation, assistant identity, provider attribution and Inbox proposal behavior.
- Private-content protection and data-sharing boundaries for family and assistant conversations.
- Responsive web and mobile product parity where the conversation capability is exposed.
- Cross-cutting usability: visible read boundaries, stable history scrolling, reachable contextual actions, accessible attachment fallbacks, and clear composer states.

## Open Questions

- May the assistant ever execute an allow-listed household action directly after explicit confirmation, or must every action continue through an Inbox proposal? Recommended default: preserve Inbox approval for every household record change.
- Who may create groups, manage membership and administer a group? Recommended default: any household member may start a group; group administrators manage membership; household access remains the outer boundary.
- What history does a newly added or re-added group member receive, and how are removed members' prior messages and read state represented?
- Which attachments are supported, how long are they retained, and may a conversation reference existing household documents without widening their permissions?
- Which message amendments are allowed, who can perform them, and what trace is visible to other household members?
- Which notification channels are required for the first delivery slice, and what are the member-level mute and preference rules?
- Is the assistant available in every group by default, only when added, or through a dedicated assistant participant that a member can start explicitly?
- Are drafts session-local and discarded on sign-out/app restart, as in CHEW ME!, or should DomusMind synchronize them across sessions or devices?
- Does each conversation have one shared pinned message, and may any participant pin or unpin it?
- Should family chat show typing indicators and online presence to all participants, and what privacy controls apply?
- What is the exact image policy: accepted image types, thumbnail generation, album grouping window, and behavior for older images without miniatures?
- Does “all files” mean all file types with download-only behavior for unknown or unsafe types, and what preview/download policy applies?
- Does forwarding copy attachments into the destination while re-evaluating mentions and destination permissions, and which family conversations may be destinations?
- Should conversation search cover message text and file names but never file contents, and should it be scoped to one conversation or all family conversations?
- What message-action menu is available on each platform, including copy, reply, forward, pin, edit, delete and read information?
- Which composer keyboard conventions should apply on desktop and native mobile (for example, Enter-to-send versus Return-to-newline), and how should members discover the send action?
- Which interaction patterns should be identical across web and mobile, and where may the control presentation differ while preserving equivalent actions and accessibility?

## Product Acceptance

A human can inspect the proposed model and confirm that:

- 1:1 and group conversations are distinct, family-only forms with explicit participant access.
- Group membership and administration are defined without allowing cross-household access.
- The conversation model covers the agreed rich-message, history and notification capabilities without inventing unresolved policy decisions.
- The model explicitly covers drafts, attachment progress/retry, image miniatures/albums, file preview/download, search, pinning, presence/typing, forwarding, action menus, pagination and read receipts.
- The assistant can be used directly or explicitly addressed in a group, is visibly distinct from a household member, receives only authorized context, and cannot silently mutate household state.
- Existing Inbox approval, provider privacy and private-thread protections remain intact.
- Non-family participants and external messaging integrations are explicitly outside this change.
- Core interactions remain usable on supported layouts: permitted actions are reachable and dismiss safely, unread state advances only while the thread is actually visible, incoming content is not obscured by the composer, and failed/missing media has an accessible fallback.

## Out of Scope

This Product Change does not approve, apply or merge itself; create implementation branches; select a framework, storage design or deployment shape; or start an FF loop. It does not authorize autonomous background agents, direct household mutations outside the approved product boundary, end-to-end encryption, a public social network, non-family participants or external messaging integrations. CHEW ME!'s DS3D/client out-of-hours auto-reply is deliberately not carried into family-only Chats.
