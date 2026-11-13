---
id: BC-CHATS
type: bounded-context
title: Chats
status: draft
---
## Responsibility

Chats owns family-member conversations, their participants and history, and explicitly requested assistant responses. It supports private 1:1 conversations and managed group conversations. The assistant can answer authorized questions and stage supported action requests as Inbox proposals; it cannot invoke household domain commands directly.

## Language

Chats distinguishes a conversation thread, a member message, an assistant participant, an assistant response, an attachment, a mention, a quoted reply and a proposed action. A thread belongs to one household and has an explicit participant set; an assistant response is generated only after an explicit direct or mentioned invocation.

## Boundaries

- Threads and their messages belong to one household.
- A private 1:1 thread has its selected household participants; a group thread has managed household membership.
- Chats owns conversation history and access to it, not the household information discussed in a message.
- Rich message capabilities remain inside the conversation's participant boundary.
- The assistant answers only from information available to the invoking member.
- Supported action requests can become unreviewed Inbox proposals; household records change only after explicit Inbox approval.
- An action request in a private thread is not copied into the household Inbox. The member is directed to capture it there.
- Only household members and the DomusMind assistant participate in Chats. Non-family participants and external messaging channels are outside this boundary.

## External Relationships

- Household and Members supplies household membership, identity and authorization.
- Other household contexts provide information the assistant may read, subject to the invoking member's access.
- Inbox owns captured action requests and proposals; it never applies them without the member's explicit decision.
- The configured Assistant Provider Target route supplies language and structured-proposal capabilities and receives only context the invoking member may access.
