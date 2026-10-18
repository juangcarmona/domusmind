# Inbox Specification

<!-- pdac-scope: cited -->

## Purpose

Inbox stores household notes and assistant action requests and presents optional Plan create/update, Task, Reminder, Timeline Note and List Item add/update proposals for an authorized member's decision. A Reminder adds an offset to an existing Plan; a Timeline Note is a standalone dated family note. The household member remains responsible for every change to shared product state.

<!-- pdac:cite id="BC-INBOX" digest="sha256:bc90937648b6bc5398680e0c192dc7d470e46fba359991982b55bbbeda594cab" -->

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

### Requirement: Interpret notes and household Chat requests as proposals

Inbox SHALL support interpretation of captured notes and supported action requests from household Chats. It MAY suggest Plan create/update, Task, Reminder, Timeline Note and List Item add/update proposals. A private Chat request is not copied into Inbox. A Reminder proposal identifies an existing Plan and a positive offset. A Timeline Note proposal has a date and text. A List Item proposal identifies an active list the member may edit. Inbox distinguishes interpreted fields and unresolved details from the original note. Interpretation must not execute product behavior. The Inbox discloses which hosted or local target receives request text and authorized context, and identifies when fallback occurs.

<!-- pdac:cite id="FR-INBOX-INTERPRET" digest="sha256:f1a488d642604dbe43c9e465683fa2602b7f027d5618c4c4d3ac7e73cc29e8be" -->

<!-- pdac:cite id="QR-INBOX-PRIVATE-HOUSEHOLD-DATA" digest="sha256:671602d6ae7cf237d2cc2c4c93dc385e7e6e6a8940206c5f2526872610c5e851" -->
<!-- pdac:cite id="BR-ASSISTANT-PROVIDER-FALLBACK" digest="sha256:07ffbc80116119eba2b2a1febdff940366df8e1dffb807106a4f2f9d29088b7a" -->
<!-- pdac:cite id="TERM-ASSISTANT-PROVIDER-TARGET" digest="sha256:9518dbd21adfc0e1ee6140643e5889050d90b902ef440118bb0efb19b21e928f" -->

#### Scenario: Interpretation is unavailable

- GIVEN Inbox has stored a member's note
- WHEN the configured interpretation provider is unavailable
- THEN the note remains available
- AND no proposal is executed

### Requirement: Member decides each proposal

Each proposal is reviewed, refined, approved or rejected independently. Approval invokes the owning Calendar, Tasks or Lists behavior and still follows its authorization and validation rules. Rejection or leaving a proposal pending does not change household state.

<!-- pdac:cite id="FR-INBOX-REVIEW" digest="sha256:8bd88c2349b041ed29da4d7d69c7423782605d478d6ad209a3e27f2fb47e33a5" -->

<!-- pdac:cite id="BR-INBOX-REVIEW-AND-EXECUTION" digest="sha256:7c54cc5879964502757faea971dacd03b841305ae6166d64072cde41aea1303f" -->

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
