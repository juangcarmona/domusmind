---
id: BC-CHATS
type: bounded-context
title: Chats
status: draft
---
## Responsibility

Chats owns household-member conversations and explicitly requested assistant responses. The assistant can answer authorized questions and stage supported action requests as Inbox proposals; it cannot invoke household domain commands.

## Language

Chats distinguishes a conversation thread, a member message, an assistant response and a proposed action. A thread has a household and participants; an assistant response is generated only after a member invokes `@dm`.

## Boundaries

- Threads and their messages belong to one household.
- Chats owns conversation history, not the household information discussed in a message.
- The assistant answers only from information available to the invoking member.
- Supported action requests can become unreviewed Inbox proposals; household records change only after explicit Inbox approval.
- An action request in a private thread is not copied into the household Inbox. The member is directed to capture it there.
- Chats does not replace general-purpose messaging services or ingest their messages.

## External Relationships

- Household and Members supplies household membership and identity.
- Other household contexts provide information the assistant may read, subject to the invoking member's access.
- Inbox owns captured action requests and proposals; it never applies them without the member's explicit decision.
- The configured Assistant Provider Target route supplies language and structured-proposal capabilities and receives only context the invoking member may access.
