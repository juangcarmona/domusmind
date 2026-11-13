# Inbox Specification

<!-- pdac-scope: cited -->

## Purpose

Inbox stores household notes and assistant action requests and presents optional Plan create/update, Task, Reminder, Timeline Note and List Item add/update proposals for an authorized member's decision. A Reminder adds an offset to an existing Plan; a Timeline Note is a standalone dated family note. The household member remains responsible for every change to shared product state.

<!-- pdac:cite id="BC-INBOX" digest="sha256:dc000d44ca9852b67481248905e09f5116b84637d4cc725137ab80dc4524498e" -->

<!-- pdac:cite id="TERM-INBOX-ITEM" digest="sha256:fe44f0370ce6c30eef712fd3c925705bef6e5c8d792b849ff0f6465e879d2fd3" -->

<!-- pdac:cite id="TERM-INBOX-PROPOSAL" digest="sha256:4a818e728ca80288c87447e0c4138ccd67cae35e29ad219e40a0563fa72ee6de" -->

---

## Requirements

### Requirement: Capture household text

An authorized household member can capture a text note and retrieve the original text and its review status. Raw text is retained as household data and is visible only to authorized members of that household.

<!-- pdac:cite id="FR-INBOX-CAPTURE" digest="sha256:e6f9cc97410c50a3a387dedc4f588004d54ede6d881a673d851faa801f4d3d46" -->

#### Scenario: Member captures a note

- GIVEN the member can access the household
- WHEN the member captures a text note
- THEN Inbox stores the original note for later review
- AND no Plan or Task is created by capture alone

### Requirement: Interpret notes and household Chat requests as proposals

Inbox SHALL support interpretation of captured notes and supported action requests from household Chats. It MAY suggest Plan create/update, Task, Reminder, Timeline Note and List Item add/update proposals. A private Chat request is not copied into Inbox. A Reminder proposal identifies an existing Plan and a positive offset. A Timeline Note proposal has a date and text. A List Item proposal identifies an active list the member may edit. Inbox distinguishes interpreted fields and unresolved details from the original note. Interpretation must not execute product behavior. The Inbox discloses which hosted or local target receives request text and authorized context, and identifies when fallback occurs.

<!-- pdac:cite id="FR-INBOX-INTERPRET" digest="sha256:8f2efe33de44deac70caac674756cd003907c878b4bdfb69128b6f5515bb6f4f" -->

<!-- pdac:cite id="QR-INBOX-PRIVATE-HOUSEHOLD-DATA" digest="sha256:e844ae46daacc6cc3506484172f829b59bc485ffa5d9c9058649885ddbcdd7d0" -->
<!-- pdac:cite id="BR-ASSISTANT-PROVIDER-FALLBACK" digest="sha256:2c98370c1a0aaaff6a236833daf18682088f617825e15cc6f789cf8718597ba9" -->
<!-- pdac:cite id="TERM-ASSISTANT-PROVIDER-TARGET" digest="sha256:3dae47c4e9e1adb8f0d1ec16d5de18e3261adb0c39874806472f318cda3fee4b" -->

#### Scenario: Interpretation is unavailable

- GIVEN Inbox has stored a member's note
- WHEN the configured interpretation provider is unavailable
- THEN the note remains available
- AND no proposal is executed

### Requirement: Member decides each proposal

Each proposal is reviewed, refined, approved or rejected independently. Approval invokes the owning Calendar, Tasks or Lists behavior and still follows its authorization and validation rules. Rejection or leaving a proposal pending does not change household state.

<!-- pdac:cite id="FR-INBOX-REVIEW" digest="sha256:6839e058607fdba5b20c2e3a539803734bcc2f8e277af64793751b8bf7677dac" -->

<!-- pdac:cite id="BR-INBOX-REVIEW-AND-EXECUTION" digest="sha256:92d34d343ebc239e46c834106ac81dc978c2f014b9ae1e5574786cc82da7821e" -->

#### Scenario: Member rejects a proposal

- GIVEN an Inbox item has a pending proposal
- WHEN the member rejects that proposal
- THEN the proposal remains visible as rejected
- AND the owning area is not invoked

#### Scenario: Member approves one proposal

- GIVEN an Inbox item has multiple pending proposals
- WHEN the member approves one valid proposal
- THEN the owning area applies only that proposal through its normal behavior
- AND other proposals remain pending until separately decided

#### Scenario: Member approves a Reminder proposal

- GIVEN a pending Reminder proposal identifies an existing Plan and a positive offset
- WHEN the member approves the proposal
- THEN Calendar adds that reminder offset to the selected Plan using its normal rules

#### Scenario: Member approves a Timeline Note proposal

- GIVEN a pending Timeline Note proposal has text and a date
- WHEN the member approves the proposal
- THEN Calendar stores a standalone dated family note
- AND the note appears in that date's household timeline


#### Scenario: Member approves a List Item addition

- GIVEN an Inbox item has a valid List Item addition proposal for an active list the member may edit
- WHEN the member approves the proposal
- THEN Lists adds the item through its normal behavior

#### Scenario: Member approves a List Item update

- GIVEN an Inbox item has a valid update proposal for an existing List Item the member may edit
- WHEN the member approves the proposal
- THEN Lists updates only the proposed fields through its normal behavior
