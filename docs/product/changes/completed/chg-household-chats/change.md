---
id: CHG-HOUSEHOLD-CHATS
type: product-change
title: Add household conversations with a read-only assistant
status: applied
base-revision: 'da227098600afa4f88033a58187691ac8479d9f9'
operations:
  add:
    - BC-CHATS
    - BR-CHAT-MEMBER-ACCESS
    - BR-CHAT-ASSISTANT-READ-ONLY
    - FR-CHAT-MANAGE-THREADS
    - FR-CHAT-ASK-ASSISTANT
    - QR-CHAT-PRIVATE-CONTENT
    - UC-CHAT-USE-CONVERSATIONS
    - UC-CHAT-ASK-ASSISTANT
    - TERM-CHAT-THREAD
    - TERM-CHAT-MESSAGE
    - TERM-ASSISTANT-RESPONSE
  modify:
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-NON-GOALS
    - CON-PRODUCT-PHASED-ROADMAP
    - CON-PRODUCT-NO-MAGIC-AUTOMATION
  remove: []
---

## Problem

The accepted Product Definition does not include conversations between household members and explicitly excludes a family chat replacement. It also does not define how a member may invoke a read-only DomusMind assistant from a conversation.

## Intended Product Outcome

DomusMind includes household-scoped conversations for family coordination. Members can create conversations with household members, exchange persistent messages, and access only conversations in which they participate. A member may explicitly invoke `@dm` to ask a question about household information available to that member. The assistant answers from authorized DomusMind data, makes no household changes, and directs action requests to Inbox.

This is a focused household coordination surface, not a general-purpose messaging product or an autonomous assistant.

## Rationale

Issue [#18](https://github.com/juangcarmona/domusmind/issues/18) defines Chats as a place for family coordination and grounded, read-only assistance. The selected product scope adds household and private conversations while keeping assistant invocation explicit and preserving the existing Inbox review boundary for changes.

## Affected Product Areas

- Household coordination and member-to-member conversations.
- Household membership and participant access to private conversations.
- Read-only assistant questions grounded in existing household information.
- The current product boundary, roadmap, and rule against hidden household changes.

## Open Questions

None. V1 includes household-member conversations, participant-scoped private or group threads, persistent messages, explicit `@dm` invocation, and read-only answers. Provider administration, external messaging integration, and assistant-initiated changes remain outside this change.

## Product Acceptance

- The Product Definition includes a Chats context, its terms, member conversations, and assistant questions.
- Conversations and messages remain within one household, and private thread history is available only to its participants.
- The assistant responds only to an explicit `@dm` invocation and grounds answers in information the invoking member may access.
- An assistant response cannot create or change a household record; action requests point to Inbox for review.
- The non-goals distinguish household coordination from replacing general-purpose messaging, and external messaging integrations remain future scope.

## Out of Scope

- Replacing general-purpose messaging services or integrating with external messaging platforms.
- Assistant-initiated or automatic changes to Plans, Tasks, Lists, or other household records.
- Provider administration, ordered provider fallbacks, and actions requiring model tool calling.
- Voice, reactions, notifications, message editing or deletion, and topic subthreads.
- API, persistence, deployment, and UI implementation details.
