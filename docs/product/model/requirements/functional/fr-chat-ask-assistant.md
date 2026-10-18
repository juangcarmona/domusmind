---
id: FR-CHAT-ASK-ASSISTANT
type: functional-requirement
title: Answer questions and stage supported actions in Chats
status: draft
derived-from:
  - UC-CHAT-ASK-ASSISTANT
  - BR-CHAT-ASSISTANT-READ-ONLY
verification:
  - scenario: A member invokes `@dm` with a question answerable from household information they may access; the assistant replies in the same thread with an answer supported by that information.
  - scenario: A member requests a supported Plan or List Item create/update in a household thread; the assistant clarifies missing details and stages an unreviewed proposal without changing a household record.
  - scenario: A member requests an action in a private thread; no private message is copied to Inbox.
  - scenario: A member posts a message without invoking `@dm`; the assistant does not respond.
  - scenario: No authorized query result answers the question; the assistant says it cannot answer from available information.
uses-terms:
  - TERM-CHAT-MESSAGE
  - TERM-ASSISTANT-RESPONSE
  - TERM-INBOX-PROPOSAL
  - TERM-ASSISTANT-PROVIDER-TARGET
---
## Requirement

The product MUST respond only to messages that explicitly invoke `@dm`. Each answer MUST be grounded in DomusMind information available to the invoking member. The assistant MUST NOT invoke household domain commands. For a supported create or update request in a household thread, it MAY ask focused clarifying questions and stage an unreviewed Inbox proposal. No household record may change until an authorized member explicitly approves that proposal through Inbox and the owning area's normal rules. A private-thread action request MUST NOT be copied to the household Inbox. The response MUST identify the provider target that handled it and whether fallback occurred.

## Rationale

Members can ask contextual questions and prepare a change for review without granting the assistant independent authority or bypassing existing household permissions.
