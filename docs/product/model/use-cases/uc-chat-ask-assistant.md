---
id: UC-CHAT-ASK-ASSISTANT
type: use-case
title: A household member asks the Chats assistant
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-CHATS
governed-by:
  - BR-CHAT-MEMBER-ACCESS
  - BR-CHAT-ASSISTANT-READ-ONLY
uses-terms:
  - TERM-CHAT-THREAD
  - TERM-CHAT-MESSAGE
  - TERM-ASSISTANT-RESPONSE
  - TERM-INBOX-PROPOSAL
  - TERM-ASSISTANT-PROVIDER-TARGET
---
## Goal

Get a concise household answer or prepare a supported change for review using information already available in DomusMind.

## Trigger

A member explicitly addresses `@dm` with a question or action request in a Chat thread.

## Preconditions

- The member is authorized to use the thread.
- The assistant can access relevant DomusMind query results under the member's existing permissions.

## Main Flow

1. The member posts a message that explicitly invokes `@dm`.
2. DomusMind determines whether the member asked a question or requested a supported action.
3. For a question, DomusMind retrieves relevant information the member may access and answers in the same thread.
4. For a supported Plan or List Item create/update request in a household thread, the assistant asks for missing required details and creates an unreviewed Inbox proposal when the request is complete.
5. The member opens Inbox, reviews or refines the proposal, and explicitly approves or rejects it.
6. The owning area applies an approved proposal through its normal authorization and validation rules.

## Alternative Flows

- If the retrieved information does not answer a question, the assistant says it cannot answer from available household information.
- If a request is unsupported, the assistant explains that and directs the member to the owning product surface.
- If an action request comes from a private Chat thread, the assistant directs the member to capture it in Inbox; no private message is copied.

## Failure Conditions

- If the assistant cannot answer reliably from authorized results, it states that it cannot answer rather than inventing household facts.
- If the configured provider is unavailable, the member's original message remains in the conversation and no domain record changes.

## Postconditions

- An assistant response is stored only for an explicit `@dm` invocation.
- An action proposal remains unapplied until an authorized member approves it in Inbox.
- No assistant response directly changes a household record.
