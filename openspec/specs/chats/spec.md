# Chats Specification

<!-- pdac-scope: cited -->

## Purpose

Chats lets household members coordinate in persistent household or participant-scoped private conversations. Members may explicitly address `@dm` to ask questions grounded in information available to them or stage supported Plan and List Item changes as Inbox proposals for explicit review.

<!-- pdac:cite id="BC-CHATS" digest="sha256:11b1dbbc66eb13111c05c9123eaa642b4049468bd2d40b04fa86800e8d706da5" -->
<!-- pdac:cite id="TERM-CHAT-THREAD" digest="sha256:f48b5ba12484eb725d5a7056b60ab1183f77785e03e92a7b4eabc3f924e32328" -->
<!-- pdac:cite id="TERM-CHAT-MESSAGE" digest="sha256:ee2d12eb38b2ed9da1ed30db28ea96bf555d997859191fedf7491c67a3d0aeaa" -->
<!-- pdac:cite id="TERM-ASSISTANT-RESPONSE" digest="sha256:c6df065b29e270063218b26d33811849a967fe374b09f5425d27a2767828faa2" -->
<!-- pdac:cite id="TERM-ASSISTANT-PROVIDER-TARGET" digest="sha256:9518dbd21adfc0e1ee6140643e5889050d90b902ef440118bb0efb19b21e928f" -->

## Requirements

### Requirement: Create and use conversation threads

An authorized household member SHALL be able to create a household conversation or a private group with selected household members. The creator SHALL be included as a participant. Members SHALL be able to list only their own threads, read their messages, and post messages. A member outside a private group SHALL receive no thread metadata or message content.

<!-- pdac:cite id="FR-CHAT-MANAGE-THREADS" digest="sha256:471f9fa8993fa17314d158ee599bfbe772b0b10251665aabcedb4f99ec16d486" -->
<!-- pdac:cite id="BR-CHAT-MEMBER-ACCESS" digest="sha256:fa5a7ef2fc77a8b8a3c55da99567c801445606f9ecc263bafff5a0261c0bb15a" -->
<!-- pdac:cite id="UC-CHAT-USE-CONVERSATIONS" digest="sha256:68367b27fc35e2ce66d9d3162f8ec02de68914b6b6c142a419370aa70b534aaf" -->
<!-- pdac:cite id="QR-CHAT-PRIVATE-CONTENT" digest="sha256:8ad3fa078189d181c554234ff721be2110dc75a813373ea153d16178e983e5da" -->

#### Scenario: A member creates a household conversation

- GIVEN an authenticated member can access a household
- WHEN the member creates a household conversation
- THEN the creator and household members can find the conversation
- AND participants can exchange persistent messages

#### Scenario: A nonparticipant requests a private conversation

- GIVEN a private group contains the current member and selected household members
- WHEN another household member requests its messages
- THEN the API denies access without returning conversation content

### Requirement: Invoke the assistant explicitly

Chats SHALL invoke the assistant only when a member message begins with an explicit `@dm` mention and answer from information available to the invoking member. A supported Plan or List Item create/update request in a household thread MAY produce an unreviewed Inbox proposal after required details are known. The assistant SHALL NOT invoke domain commands; each proposal needs explicit Inbox approval. Private-thread requests are not copied into Inbox. Responses identify the provider target and any fallback.

<!-- pdac:cite id="FR-CHAT-ASK-ASSISTANT" digest="sha256:af9ab2889fe4bb5b9168f43d0725874432354e723000b96ae0d732fd8ffc597b" -->
<!-- pdac:cite id="BR-CHAT-ASSISTANT-READ-ONLY" digest="sha256:f1f7e93a9607dc324ecada95aa4bf2f012e7e0a96304c5b4a6d07e8ea5d4d710" -->
<!-- pdac:cite id="UC-CHAT-ASK-ASSISTANT" digest="sha256:d347c37e28fdcd15102837a5dce022727dd669fe446f853aaec45cd7d3e3deca" -->
<!-- pdac:cite id="TERM-INBOX-PROPOSAL" digest="sha256:338813e09047ef84c451b2e8afa55879912772536afe9b80d69bc994035a37ce" -->

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

#### Scenario: A member asks for a supported household change

- GIVEN a member addresses `@dm` in a household thread with a request to create or update a Plan or List Item
- WHEN the required details are available
- THEN Chats creates an unreviewed Inbox proposal
- AND no Plan or List Item changes until an authorized member approves it in Inbox

#### Scenario: The assistant clarifies an incomplete request

- GIVEN a member requests a Plan or List Item change with required details missing
- WHEN the member addresses `@dm` in a household thread
- THEN the assistant asks a focused clarification
- AND no Inbox proposal is created until required details are known

#### Scenario: A member asks for a private-thread change

- GIVEN a member addresses `@dm` in a private thread with a request to change household data
- WHEN the assistant responds
- THEN the assistant directs the member to capture the request in Inbox
- AND Chats does not copy private thread content into Inbox

#### Scenario: The configured assistant provider is external

- GIVEN the selected assistant provider is outside the deployment
- WHEN the member opens Chats
- THEN the surface identifies that assistant questions and their household context are sent to that provider



### Requirement: Configure provider targets and disclose routing

The household manager SHALL be able to configure and order one to three hosted or local assistant targets. Credentials SHALL remain secret. The product SHALL disclose what text and authorized context each target receives and identify which target handled a request and whether fallback occurred. Fallback SHALL be limited to retryable provider availability or capability failures.

<!-- pdac:cite id="FR-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:6f46b897ec17ba34fa6027cdfa95a01a41d176d06edb4e69daced913c924bdc8" -->
<!-- pdac:cite id="UC-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:b86e8a21e83677a4cbe232704a35ea0fa5f07f572d563b18f3c4fbcc87cfb39f" -->
<!-- pdac:cite id="BR-ASSISTANT-PROVIDER-FALLBACK" digest="sha256:07ffbc80116119eba2b2a1febdff940366df8e1dffb807106a4f2f9d29088b7a" -->
<!-- pdac:cite id="QR-ASSISTANT-PROVIDER-PRIVACY" digest="sha256:769c170bfea0d620610f7d331581c0c5c7685f962bcaba4060442dba5a2eb4b2" -->

#### Scenario: The primary provider is unavailable

- GIVEN the household has an ordered primary and secondary provider target
- WHEN the primary target times out for an assistant request
- THEN the next eligible target may handle the request
- AND the response identifies the target used and that fallback occurred

#### Scenario: The assistant returns a valid clarification

- GIVEN a request is missing required details
- WHEN the assistant asks a clarification question
- THEN the request is not sent to a fallback target

#### Scenario: A member inspects provider data use

- GIVEN the household has hosted or local provider targets configured
- WHEN a member views the assistant's provider information
- THEN the member can see what request text and household context those targets receive
- AND saved credentials are not exposed
