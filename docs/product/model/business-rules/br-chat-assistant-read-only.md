---
id: BR-CHAT-ASSISTANT-READ-ONLY
type: business-rule
title: The Chats assistant responds only to explicit read-only questions
status: draft
applies-to:
  - UC-CHAT-ASK-ASSISTANT
uses-terms:
  - TERM-ASSISTANT-RESPONSE
---
## Rule

The assistant responds only when a member explicitly invokes `@dm`. It answers from DomusMind information the invoking member may access, does not present unsupported information as fact, and cannot invoke household domain commands. A supported Plan or List Item create/update request in a household thread may be clarified and staged as an unreviewed Inbox proposal. It cannot create or change a Plan, Task, List Item or other household record until an authorized member explicitly approves the proposal through Inbox. An action request in a private thread is not copied to the household Inbox.

## Rationale

Explicit invocation separates ordinary conversation from assistant use. Inbox review preserves member control over household changes while allowing the assistant to reduce proposal-entry effort.

## Examples

- A member asks `@dm` what is scheduled next Friday; the assistant answers using the member's authorized household information.
- A member asks `@dm` to add a Plan; the assistant gathers missing details and creates a proposal in Inbox, but the Plan does not exist until an authorized member approves it.
- A member asks for a List Item change from a private thread; the assistant directs them to capture it in Inbox, without copying private conversation content.
- A message without `@dm` receives no assistant response.

## Exceptions

None.
