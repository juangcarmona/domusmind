---
id: CHG-CONVERSATIONAL-ASSISTANT
type: product-change
title: Add a conversational assistant with household-configured providers
status: applied
base-revision: 'e0a979c'
operations:
  add:
    - TERM-ASSISTANT-PROVIDER-TARGET
    - FR-ASSISTANT-CONFIGURE-PROVIDERS
    - UC-ASSISTANT-CONFIGURE-PROVIDERS
    - BR-ASSISTANT-PROVIDER-FALLBACK
    - QR-ASSISTANT-PROVIDER-PRIVACY
  modify:
    - BC-CHATS
    - BC-INBOX
    - BC-LISTS
    - BR-CHAT-ASSISTANT-READ-ONLY
    - BR-INBOX-REVIEW-AND-EXECUTION
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-NO-MAGIC-AUTOMATION
    - CON-PRODUCT-PHASED-ROADMAP
    - FR-CHAT-ASK-ASSISTANT
    - FR-INBOX-INTERPRET
    - QR-CHAT-PRIVATE-CONTENT
    - QR-INBOX-PRIVATE-HOUSEHOLD-DATA
    - UC-CHAT-ASK-ASSISTANT
    - UC-INBOX-CAPTURE-AND-REVIEW
    - SB-INBOX-CAPTURE-AND-INTERPRET
  remove: []
---

## Problem

DomusMind supports read-only `@dm` questions in Chats and review-first text interpretation in Inbox, but it does not define how an assistant may clarify and stage requested household changes, how households control provider routing, or when a configured provider may receive household data. Issue #143 asks for conversational queries and actions plus up to three ordered hosted or local providers.

## Intended Product Outcome

Household members can ask `@dm` questions in Chats and request supported changes in plain text. The assistant answers questions using only information the member may access. For an action request, it asks for missing required details and creates an unreviewed Inbox proposal when the request is clear. The first action proposals create or update Calendar Plans and add or update List Items. Inbox keeps its existing Task, Plan Reminder, and Timeline Note proposals. Every proposed change remains unapplied until an authorized member reviews and explicitly approves it through Inbox and the owning area's normal rules.

The household manager configures an ordered route of one to three provider targets. Each target identifies its endpoint, model, protocol and any required credential. Targets may be hosted or local. The manager can replace a saved credential but cannot retrieve its value; ordinary members cannot manage or view credentials. A target is eligible only when it supports the assistant operation's required text and structured-proposal capabilities.

DomusMind may try the next configured target only after a retryable availability failure, rate limit, timeout, or missing required capability. It does not retry a valid answer, refusal, authorization denial, ambiguous request, or domain validation failure against another provider. The member can see which target handled the request and whether fallback occurred. Before use and when routing changes, the product explains what text and authorized household context each hosted or local target receives.

Chats remains a household conversation surface, not a general-purpose autonomous agent. It does not invoke Calendar, Lists or other domain commands directly. An action request made in a private chat is not copied into the household Inbox; the member is directed to capture it there, preserving the private thread boundary.

## Rationale

The product owner selected text-only assistance with explicit review for household changes, and approved a read-only assistant in Chats under CHG-HOUSEHOLD-CHATS. This change expands the assistant to stage proposals while preserving the rule that a person decides whether a household record is created or changed. Making provider routing visible and configurable gives each household control over where its text and context are processed.

## Affected Product Areas

- Chats questions and action requests.
- Inbox interpretation and explicit proposal review.
- Calendar Plan and Lists item creation or updates through existing area rules.
- Household manager provider settings, provider fallback, and household-data privacy.

## Open Questions

None. The initial action types are Plan creation or update and List Item addition or update; action requests in a private chat must be recaptured through Inbox. All household changes require explicit Inbox approval. The household manager configures one to three ordered targets, and fallback is limited to retryable provider availability or capability failures.

## Product Acceptance

- Chats answers only from information available to the invoking member; action requests produce no domain change and are staged for explicit Inbox review.
- A complete request produces a reviewable Plan or List Item proposal; missing required information prompts a focused clarification before proposal creation.
- Inbox proposals remain visible as proposals until an authorized member approves them, and approved changes pass the owning context's normal authorization and validation.
- A private chat action request is not copied to the household Inbox.
- A household manager can configure and order up to three hosted or local targets without exposing saved credentials to ordinary members.
- Provider selection, capability requirements, retryable failures, fallback order and member-visible fallback disclosure are explicit.
- Members are told what data is sent to each selected target; only information the invoking member may access is included.
- The ProductShape overlay validates against the current model with no errors.

## Out of Scope

- Direct model-issued domain commands, automatic execution, or changes without explicit Inbox approval.
- Plan deletion or cancellation; List Item deletion, checking, scheduling or other list operations.
- Assistant actions from private Chats threads.
- Voice, external messaging-platform integrations, and providers that cannot satisfy the required text and structured-proposal capabilities.
- Provider account registration, billing, provider-specific product behavior, implementation architecture, API, persistence, deployment and UI design.
