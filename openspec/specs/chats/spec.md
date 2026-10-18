# Chats Specification

<!-- pdac-scope: cited -->

## Purpose

Chats lets household members coordinate in persistent household or participant-scoped private conversations. Members may explicitly address `@dm` to ask read-only questions grounded in information available to them.

<!-- pdac:cite id="BC-CHATS" digest="sha256:02ba26543d23ee6f89430f2766bcd7c86f3e4cc4e3fcad3307a75d94a475310e" -->
<!-- pdac:cite id="TERM-CHAT-THREAD" digest="sha256:f48b5ba12484eb725d5a7056b60ab1183f77785e03e92a7b4eabc3f924e32328" -->
<!-- pdac:cite id="TERM-CHAT-MESSAGE" digest="sha256:ee2d12eb38b2ed9da1ed30db28ea96bf555d997859191fedf7491c67a3d0aeaa" -->
<!-- pdac:cite id="TERM-ASSISTANT-RESPONSE" digest="sha256:c6df065b29e270063218b26d33811849a967fe374b09f5425d27a2767828faa2" -->

## Requirements

### Requirement: Create and use conversation threads

An authorized household member SHALL be able to create a household conversation or a private group with selected household members. The creator SHALL be included as a participant. Members SHALL be able to list only their own threads, read their messages, and post messages. A member outside a private group SHALL receive no thread metadata or message content.

<!-- pdac:cite id="FR-CHAT-MANAGE-THREADS" digest="sha256:471f9fa8993fa17314d158ee599bfbe772b0b10251665aabcedb4f99ec16d486" -->
<!-- pdac:cite id="BR-CHAT-MEMBER-ACCESS" digest="sha256:fa5a7ef2fc77a8b8a3c55da99567c801445606f9ecc263bafff5a0261c0bb15a" -->
<!-- pdac:cite id="UC-CHAT-USE-CONVERSATIONS" digest="sha256:68367b27fc35e2ce66d9d3162f8ec02de68914b6b6c142a419370aa70b534aaf" -->
<!-- pdac:cite id="QR-CHAT-PRIVATE-CONTENT" digest="sha256:2a21faf96ceea5d0842fdc28b3308c404d49f9862d15a6f646f9132f45adfea3" -->

#### Scenario: A member creates a household conversation

- GIVEN an authenticated member can access a household
- WHEN the member creates a household conversation
- THEN the creator and household members can find the conversation
- AND participants can exchange persistent messages

#### Scenario: A nonparticipant requests a private conversation

- GIVEN a private group contains the current member and selected household members
- WHEN another household member requests its messages
- THEN the API denies access without returning conversation content

### Requirement: Invoke the read-only assistant explicitly

Chats SHALL invoke the assistant only when a member message begins with an explicit `@dm` mention. The assistant SHALL answer using authorized household data, SHALL not modify household records, and SHALL direct change requests to Inbox. If a configured external provider receives the question and context, Chats SHALL disclose that to the member.

<!-- pdac:cite id="FR-CHAT-ASK-ASSISTANT" digest="sha256:49e94c18fbcf9ee6722eb9230cae07edb9661d284455c26c6522e160f8574543" -->
<!-- pdac:cite id="BR-CHAT-ASSISTANT-READ-ONLY" digest="sha256:0d3703a9b0b3d2e5db5baf8b8c8658a40739273f42c973e17943aec214b4d6c8" -->
<!-- pdac:cite id="UC-CHAT-ASK-ASSISTANT" digest="sha256:20081973d9ffcde2bd50b4d6c5baadc2d7cfe83fa2dc0ad683632cb1b0a2b5d6" -->

#### Scenario: A member posts an ordinary message

- GIVEN a member is in a conversation
- WHEN the member posts a message without a leading `@dm`
- THEN the message is stored
- AND the assistant does not respond

#### Scenario: A member asks a household question

- GIVEN a member is in a conversation and the configured assistant is available
- WHEN the member posts a question beginning with `@dm`
- THEN Chats stores the member message and an assistant response in the same thread
- AND the response refers only to authorized household information

#### Scenario: A member asks for a household change

- GIVEN a member addresses `@dm` with a request to create or change a household record
- WHEN the assistant responds
- THEN no household record changes
- AND the assistant directs the member to Inbox for review

#### Scenario: The configured assistant provider is external

- GIVEN the selected assistant provider is outside the deployment
- WHEN the member opens Chats
- THEN the surface identifies that assistant questions and their household context are sent to that provider

