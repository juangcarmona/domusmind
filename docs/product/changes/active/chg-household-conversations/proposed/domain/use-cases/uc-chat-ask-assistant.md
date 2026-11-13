---
id: UC-CHAT-ASK-ASSISTANT
type: use-case
title: A household member speaks with or mentions the Chats assistant
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-CHATS
governed-by:
  - BR-CHAT-MEMBER-ACCESS
  - BR-CHAT-ASSISTANT-READ-ONLY
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-CHAT-ASSISTANT-PARTICIPANT
  - TERM-ASSISTANT-RESPONSE
  - TERM-INBOX-PROPOSAL
  - TERM-ASSISTANT-PROVIDER-TARGET
---

## Goal

Ask the DomusMind assistant a household question or prepare a supported household action for review from a familiar direct or group conversation.

## Trigger

A member explicitly starts a direct assistant conversation or mentions the assistant in a group message.

## Preconditions

- The member is authorized to use the conversation.
- The assistant can access relevant DomusMind query results under the member's existing permissions.
- A configured provider target is eligible for the requested operation, or the product can explain that no eligible target is available.

## Main Flow

1. The member opens the assistant conversation or composes a group message that explicitly addresses the assistant.
2. DomusMind determines whether the member asked a question or requested a supported action.
3. For a question, DomusMind retrieves relevant information the member may access and answers in the same conversation.
4. For a supported action request, the assistant asks focused clarification questions as needed and stages an unreviewed Inbox proposal when the request is complete.
5. The member reviews, refines, approves or rejects the proposal in Inbox.
6. The owning area applies an approved proposal through its normal authorization and validation rules.

## Alternative Flows

- If the retrieved information does not answer a question, the assistant says it cannot answer from available household information.
- If a request is unsupported, the assistant explains that and directs the member to the owning product surface.
- If an action request comes from a private 1:1 thread, the assistant directs the member to capture it in Inbox; no private message is copied.
- If the provider route falls back, the response identifies the effective target and fallback outcome under the existing provider privacy rule.

## Failure Conditions

- If the assistant cannot answer reliably from authorized results, it states that it cannot answer rather than inventing household facts.
- If the configured provider is unavailable or incapable, the member's original message remains in the conversation and no domain record changes.
- If the member is not authorized for a referenced household record, neither the assistant nor its provider context discloses it.

## Postconditions

- An assistant response is stored only for an explicit direct or group invocation.
- An action proposal remains unapplied until an authorized member approves it in Inbox.
- No assistant response directly changes a household record.
