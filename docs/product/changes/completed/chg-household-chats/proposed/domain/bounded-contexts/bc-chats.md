---
id: BC-CHATS
type: bounded-context
title: Chats
status: draft
---

## Responsibility

Chats owns household-member conversations and the read-only assistant responses explicitly requested within them.

## Language

Chats distinguishes a conversation thread, a member message, and an assistant response. A thread has a household and participants; an assistant response is a message generated only after a member invokes `@dm`.

## Boundaries

- Threads and their messages belong to one household.
- Chats owns conversation history, not the household information discussed in a message.
- The assistant may answer questions using information available to the invoking member; it does not change household records.
- Inbox remains the place where members capture and review proposed household changes.
- Chats does not replace general-purpose messaging services or ingest their messages.

## External Relationships

- Household and Members supplies household membership and identity.
- Other household contexts provide information the assistant may read, subject to the invoking member's access.
- Inbox is the member-controlled path for turning a conversation into a proposed household change.
