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

<!-- pdac:cite id="FR-INBOX-CAPTURE" digest="sha256:522b9c28e76f8f2f7c113be168f53020be4cbd7abdbfaea52b28577d98a9e2db" -->

#### Scenario: Member captures a note

- GIVEN the member can access the household
- WHEN the member captures a text note
- THEN Inbox stores the original note for later review
- AND no Plan or Task is created by capture alone

### Requirement: Interpret notes and household Chat requests as proposals

Inbox SHALL support interpretation of captured notes and supported action requests from household Chats. It MAY suggest Plan create/update, Task, Reminder, Timeline Note and List Item add/update proposals. A private Chat request is not copied into Inbox. A Reminder proposal identifies an existing Plan and a positive offset. A Timeline Note proposal has a date and text. A List Item proposal identifies an active list the member may edit. Inbox distinguishes interpreted fields and unresolved details from the original note. Interpretation must not execute product behavior. The Inbox discloses which hosted or local target receives request text and authorized context, and identifies when fallback occurs.

<!-- pdac:cite id="FR-INBOX-INTERPRET" digest="sha256:8655bde49845f4278b7ad37baeab11421c4ae263138f278a4a902d6aef1fcee8" -->

<!-- pdac:cite id="QR-INBOX-PRIVATE-HOUSEHOLD-DATA" digest="sha256:90b04c52585e12c8c0083b41322685067f69f32783fd280db0e895f7714debb3" -->
<!-- pdac:cite id="BR-ASSISTANT-PROVIDER-FALLBACK" digest="sha256:2c98370c1a0aaaff6a236833daf18682088f617825e15cc6f789cf8718597ba9" -->
<!-- pdac:cite id="TERM-ASSISTANT-PROVIDER-TARGET" digest="sha256:3dae47c4e9e1adb8f0d1ec16d5de18e3261adb0c39874806472f318cda3fee4b" -->

#### Scenario: Interpretation is unavailable

- GIVEN Inbox has stored a member's note
- WHEN the configured interpretation provider is unavailable
- THEN the note remains available
- AND no proposal is executed

### Requirement: Member decides each proposal

Each proposal is reviewed, refined, approved or rejected independently. Approval invokes the owning Calendar, Tasks or Lists behavior and still follows its authorization and validation rules. Rejection or leaving a proposal pending does not change household state.

If an approved action fails without applying its household change, the proposal SHALL return to Pending with its details retained and its failure reason visible. The member SHALL be able to correct it and explicitly approve it again. The product SHALL NOT retry the action automatically or execute an Applied proposal again.

<!-- pdac:cite id="FR-INBOX-REVIEW" digest="sha256:38d5e9eae4b9ca17bf8f545451fc58bea64d06318c9d65e4ff3dd7d1385e8147" -->
<!-- pdac:cite id="LC-INBOX-PROPOSAL" digest="sha256:6a279f3f478b30f7097d5ad9356fe3cd391905b302d9a7b82e88e0c47e494599" -->
<!-- pdac:cite id="SB-INBOX-RECOVER-UNAPPLIED-FAILURE" digest="sha256:d29471982db39e2242bcfa80ead88d8be75256147350babdd540779a4e25c537" -->
<!-- pdac:cite id="SB-INBOX-REAPPROVE-CORRECTED-PROPOSAL" digest="sha256:006ae048f59dc44279436be42da03ccef19167c1e3172dd8cbde8641f9a50520" -->
<!-- pdac:cite id="SB-INBOX-APPLIED-PROPOSAL-CANNOT-EXECUTE-AGAIN" digest="sha256:1648350c0aae8c7c8419cfe297fea3da7eea2f4197d46a1898d30c593974f704" -->

<!-- pdac:cite id="BR-INBOX-REVIEW-AND-EXECUTION" digest="sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267" -->

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

#### Scenario: Failed unapplied action returns for correction

- GIVEN a proposal is Executing after explicit approval
- AND its owning-area action fails without applying the household change
- WHEN the failed attempt is reported
- THEN the proposal is Pending with its details retained
- AND the member sees the failure reason
- AND no automatic retry occurs

#### Scenario: Member approves a corrected proposal

- GIVEN a failed unapplied proposal is Pending
- AND an authorized member has corrected its details
- WHEN the member explicitly approves the corrected proposal
- THEN the proposal enters execution through its owning area
- AND the owning area checks its current authorization and validation rules

#### Scenario: Applied proposal cannot execute again

- GIVEN a proposal is Applied and linked to its successful result
- WHEN a member attempts to execute it again
- THEN the owning-area action is not invoked again
- AND the proposal remains Applied with its existing result
