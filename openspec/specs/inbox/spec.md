# Inbox Specification

<!-- pdac-scope: cited -->

## Purpose

Inbox stores household notes and presents optional Plan, Task, Reminder and Timeline Note proposals for an authorized member's decision. A Reminder adds an offset to an existing Plan; a Timeline Note is a standalone dated family note. The household member remains responsible for every change to shared product state.

<!-- pdac:cite id="BC-INBOX" digest="sha256:5d1c27e26d86f896dd7be4e3ab7507f69768119974525ed9817ee843f9d4ad4d" -->

<!-- pdac:cite id="TERM-INBOX-ITEM" digest="sha256:282309fac7b6161f54cbb3c3247faccd8da1506796ab0319fd1b4b89aefd45a7" -->

<!-- pdac:cite id="TERM-INBOX-PROPOSAL" digest="sha256:338813e09047ef84c451b2e8afa55879912772536afe9b80d69bc994035a37ce" -->

---

## Requirements

### Requirement: Capture household text

An authorized household member can capture a text note and retrieve the original text and its review status. Raw text is retained as household data and is visible only to authorized members of that household.

<!-- pdac:cite id="FR-INBOX-CAPTURE" digest="sha256:4bdadc809f324b1df68faf4b47365ec5de20697436f0e83e80a33416fd97258b" -->

#### Scenario: Member captures a note

- GIVEN the member can access the household
- WHEN the member captures a text note
- THEN Inbox stores the original note for later review
- AND no Plan or Task is created by capture alone

### Requirement: Interpret notes as proposals

Inbox may interpret a note using the deployment-selected provider and suggest Plan, Task, Reminder and Timeline Note proposals. A Reminder proposal identifies an existing Plan and a positive offset. A Timeline Note proposal has a date and text. Inbox distinguishes interpreted fields and unresolved details from the original note. Interpretation must not execute product behavior. The Inbox discloses when note text is sent outside the deployment.

<!-- pdac:cite id="FR-INBOX-INTERPRET" digest="sha256:b03ca8cf8beb1233c86f7433dc594c9cf4dfad6c6f47bdb413a084b58e679765" -->

<!-- pdac:cite id="QR-INBOX-PRIVATE-HOUSEHOLD-DATA" digest="sha256:8d7ea231e78c1835edef714637cd4f3620aa6319892b341156110022b66e96cb" -->

#### Scenario: Interpretation is unavailable

- GIVEN Inbox has stored a member's note
- WHEN the configured interpretation provider is unavailable
- THEN the note remains available
- AND no proposal is executed

### Requirement: Member decides each proposal

Each proposal is reviewed, refined, approved or rejected independently. Approval invokes the owning area behavior and still follows its authorization and validation rules. Rejection or leaving a proposal pending does not change Calendar or Tasks state.

<!-- pdac:cite id="FR-INBOX-REVIEW" digest="sha256:8bd88c2349b041ed29da4d7d69c7423782605d478d6ad209a3e27f2fb47e33a5" -->

<!-- pdac:cite id="BR-INBOX-REVIEW-AND-EXECUTION" digest="sha256:a0b5a7a703274aa651f59d2b20d2c200d249892473aa0bb06a9d40995073e1bd" -->

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
