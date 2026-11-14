# Chats Specification

<!-- pdac-scope: cited -->

## Purpose

Chats lets household members coordinate in persistent household or participant-scoped private conversations. Members may explicitly address `@dm` to ask questions grounded in information available to them or stage supported Plan and List Item changes as Inbox proposals for explicit review.

<!-- pdac:cite id="BC-CHATS" digest="sha256:8bee3e6eae0eef994be3523b72a983cd4c98e8b3aa1d365da90fd7de7d24283f" -->
<!-- pdac:cite id="TERM-CHAT-THREAD" digest="sha256:361bf9e8d9655b2ea3e16b509ac34e375fad829671f85a32386f43bc77ffe73e" -->
<!-- pdac:cite id="TERM-CHAT-MESSAGE" digest="sha256:1eb172300547a4ce59bd91907c639652eb389cef9602670911bc5c27ddcf08d1" -->
<!-- pdac:cite id="TERM-ASSISTANT-RESPONSE" digest="sha256:0c0085ba615181c04dc5f487b86d3d03013b4180202ebca559b55a4e5cec9bec" -->
<!-- pdac:cite id="TERM-ASSISTANT-PROVIDER-TARGET" digest="sha256:3dae47c4e9e1adb8f0d1ec16d5de18e3261adb0c39874806472f318cda3fee4b" -->

## Requirements

### Requirement: Create and use conversation threads

An authorized household member SHALL be able to create a household conversation or a private group with selected household members. The creator SHALL be included as a participant. Members SHALL be able to list only their own threads, read their messages, and post messages. A member outside a private group SHALL receive no thread metadata or message content.

<!-- pdac:cite id="FR-CHAT-MANAGE-THREADS" digest="sha256:ca38e0324cedc282b3ad87f3f482eb85bf0772b6aeb509468f9b18a4559a7418" -->
<!-- pdac:cite id="BR-CHAT-MEMBER-ACCESS" digest="sha256:4a477d90cec98082be4894fe5c77a009172de5cb9a127d4ab379c795c5c7bce6" -->
<!-- pdac:cite id="UC-CHAT-USE-CONVERSATIONS" digest="sha256:0c31f7ee0c36782714b7ad98cc79e6d4a7a5458e2d5309141c7d10ea3c496f8c" -->
<!-- pdac:cite id="QR-CHAT-PRIVATE-CONTENT" digest="sha256:b58936e309e9fea5898eddefd2c44a8c6eebdb25d268add2f59a3d2c3667baca" -->

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

<!-- pdac:cite id="FR-CHAT-ASK-ASSISTANT" digest="sha256:db3f154f813d074cac902e755f3af966bcf889ac6ddc7356927eeb79eedc3a11" -->
<!-- pdac:cite id="BR-CHAT-ASSISTANT-READ-ONLY" digest="sha256:d8dd787f0b4bb9bbaaac8e897ecd777ad057866d3e8288a1d060bb1c0dc6321f" -->
<!-- pdac:cite id="UC-CHAT-ASK-ASSISTANT" digest="sha256:0d58faa2d48485053d947a4c8b633b6aa4ab261d633894f5e4fa133f4715f9d6" -->
<!-- pdac:cite id="TERM-INBOX-PROPOSAL" digest="sha256:4a818e728ca80288c87447e0c4138ccd67cae35e29ad219e40a0563fa72ee6de" -->

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

<!-- pdac:cite id="FR-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:cbc68a46b1c8cdec569c82e15ae1ae4de01a615795e09a8f543c24e2ad215d99" -->
<!-- pdac:cite id="UC-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:4368dc7f90d60dfc87ce6d3313e2865ec897854ece9e4cbc1f064356c53a4820" -->
<!-- pdac:cite id="BR-ASSISTANT-PROVIDER-FALLBACK" digest="sha256:2c98370c1a0aaaff6a236833daf18682088f617825e15cc6f789cf8718597ba9" -->
<!-- pdac:cite id="QR-ASSISTANT-PROVIDER-PRIVACY" digest="sha256:17c1d2b4e0e56c4edea6f365bf389d232ba0772e52dd327c8b9ae907b7e2d614" -->

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
