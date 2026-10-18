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
---

## Goal

Get a concise answer to a household question using information already available in DomusMind.

## Trigger

A member explicitly addresses `@dm` with a question in a Chat thread.

## Preconditions

- The member is authorized to use the thread.
- The assistant can access the relevant DomusMind query results under the member's existing permissions.

## Main Flow

1. The member posts a message that explicitly invokes `@dm` and asks a question.
2. DomusMind interprets the question and retrieves relevant household information the member may access.
3. The assistant responds in the same thread with an answer grounded in those results.
4. The response becomes part of the thread history.

## Alternative Flows

- If the retrieved information does not answer the question, the assistant says it cannot answer from available household information.
- If the member requests a household change, the assistant directs the member to Inbox and does not perform the change.

## Failure Conditions

- If the assistant cannot answer reliably from authorized results, it states that it cannot answer rather than inventing household facts.
- If the configured assistant is unavailable, the member's original message remains in the conversation and no assistant response is stored.

## Postconditions

- An assistant response is stored only for an explicit `@dm` invocation.
- No household record changes as a result of the assistant response.
