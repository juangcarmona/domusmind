# Calendar Specification

<!-- pdac-scope: cited -->

## Purpose

Calendar is the temporal structure of household life. It owns the **Event** aggregate: time-bound commitments that affect one or more household participants.

Calendar answers:
- what is happening in this household
- when it happens
- who is involved

Calendar is the source of truth for time-based planning. It owns event schedules, participants, and reminder definitions. Notification delivery belongs to infrastructure, not to Calendar.

Calendar also manages **external calendar connections** — member-scoped delegated integrations that import read-only entries from third-party providers (Phase 1: Microsoft Outlook) into the household's temporal read surfaces.

Calendar does not own the Agenda or family timeline read models. It is one source for those projections. The Agenda and timeline are assembled from Calendar, Tasks, and Shared Lists.

User interfaces may present Events as **Plans**, but the domain model remains centered on the **Event** aggregate.

Imported external calendar data is read-only integration state. It must never be treated as native household planning state.

<!-- pdac:cite id="BC-CALENDAR" digest="sha256:672274e54e0fbc3961c16ed85b7c8a6db31b7ff828ca0a88304a049be3389c0f" -->

<!-- pdac:cite id="TERM-PLAN" digest="sha256:f06fe7678136c535cf5f55e815f161d73562165b17c565f9496386dbd8395ac3" -->

<!-- pdac:cite id="TERM-AGENDA" digest="sha256:816f1a2f6d4e87da6b928b3c6d68433adab2c5467f4c02667e00fdb9b86387f0" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:2b2ea1de6c0f9a84d96f94d20087467fa7a198f197da80a85748fb145816fe03" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:a3d8d4afcfc23492b745fa37ac5b103ae76a7bc1d214456aeddbe2f57473ceea" -->

---

## Requirements

### Requirement: Event Scheduling

A household SHALL be able to schedule a time-bound event with a title and start time as the minimum required inputs.

Optional inputs: end time (must be after start time if provided), participants (family members, dependents, or pets), and a responsibility domain reference for contextual grouping. A newly created event starts in **scheduled** status and belongs to exactly one family.

<!-- pdac:cite id="FR-CALENDAR-SCHEDULE-PLAN" digest="sha256:bd41645e55c258195ace26b0a0c80f301df66c4f6b9da0688005e72fc34caa97" -->

<!-- pdac:cite id="UC-CALENDAR-SCHEDULE-PLAN" digest="sha256:1d352af5e2b9dbe134c0d2731506114f5656c35d50a7346d7e88cd0eb6a45da8" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START" digest="sha256:4e9f7d1662dd3383ac02f651186e69ba48289e1680c4624abba444f7183346ba" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-END-AFTER-START" digest="sha256:a9c60bc761516a4bcb95d4bc350f2ecb3106b5c82af05db5bd0e96cdb7b399ea" -->

<!-- pdac:cite id="BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD" digest="sha256:e7357786552892d48ee56f18e1141d513be6e603e1eec7f33d090e97f0103535" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-BELONGS-TO-ONE-HOUSEHOLD" digest="sha256:35c8ee69a2de1f83c595f15a51f9fce90205e72ca6c99e3533cb324503685049" -->

<!-- pdac:cite id="TERM-PLAN" digest="sha256:f06fe7678136c535cf5f55e815f161d73562165b17c565f9496386dbd8395ac3" -->

<!-- pdac:cite id="TERM-PLAN-PARTICIPANT" digest="sha256:9aec8030305ed4f94ceefe56c52cdfd679f58bae7b1b97c4c3f9014902074c96" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-PLAN-MINIMUM" digest="sha256:65598ceb793e3dbe3aefe0fc4becd48c750903e6de819a26f10ab969aa35aadb" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-PLAN-END-BEFORE-START" digest="sha256:17b26905f475af3aa2315cba33118428927f3241aa4971f8017749c99ca9befa" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-PLAN-UNKNOWN-PARTICIPANT" digest="sha256:194bb5e8dc7345e587089e2decad2f0e38505c0891c9a87968380863e3ef2185" -->

<!-- pdac-drift ids="BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START, FR-CALENDAR-SCHEDULE-PLAN, SB-CALENDAR-SCHEDULE-ALL-DAY-PLAN" summary="Spec requires a start time; the model (Q-0045) also accepts an all-day plan with a start date alone and multi-day plans" -->

<!-- pdac-drift ids="TERM-PLAN-PARTICIPANT, BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD" summary="Spec names members, dependents and pets as participant kinds; the model (Q-0042) has household members of any role only, pets being members with the Pet role" -->

#### Scenario: Household creates an event with minimum inputs

- GIVEN a family exists
- WHEN a member schedules an event with a title and start time
- THEN an event is created in scheduled status
- AND the event belongs to that family

#### Scenario: Event with end time before start time is rejected

- GIVEN a member provides a start time and an end time
- WHEN the end time is earlier than the start time
- THEN the event is not created
- AND a validation error is returned

#### Scenario: Event with invalid participant reference is rejected

- GIVEN a participant identifier does not belong to the family
- WHEN a member attempts to schedule an event including that participant
- THEN the event is not created
- AND a validation error is returned

---

### Requirement: Event Rescheduling

A household SHALL be able to update the schedule of a scheduled event.

Rescheduling updates the start time and optionally the end time. The event's identity, participants, and reminders remain unchanged. Cancelled events cannot be rescheduled.

<!-- pdac:cite id="FR-CALENDAR-RESCHEDULE-PLAN" digest="sha256:0fd9effe4cc812573e94d4803c563719133e2077ca39d411b8f7332640da1eb9" -->

<!-- pdac:cite id="UC-CALENDAR-RESCHEDULE-PLAN" digest="sha256:b271a7e6b992fa9f9eb2487d1429a5bd2a46d334f1f7064779dfc287075639c4" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-END-AFTER-START" digest="sha256:a9c60bc761516a4bcb95d4bc350f2ecb3106b5c82af05db5bd0e96cdb7b399ea" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:d0ffa51dd1b3cb43b9ccec2077dd08a91fc3e03e0124f8112b361e122abf94f9" -->

<!-- pdac:cite id="SB-CALENDAR-RESCHEDULE-PLAN" digest="sha256:9eeba089f8470428310ab4caeb564d17c37e1923b9c9f470598a6ebb1a8dd4a1" -->

<!-- pdac:cite id="SB-CALENDAR-RESCHEDULE-CANCELLED-PLAN" digest="sha256:52e53dde2c7da7ef00aacec96fffbff27b9bcc3d23068cd7be9589455e32deb5" -->

<!-- pdac:cite id="SB-CALENDAR-RESCHEDULE-INVALID-TIMES" digest="sha256:49dc28f5bd0c9f8aeb81fdc1e0a18b6184404ef6602d24567bd9eeb69c316dbd" -->

#### Scenario: Event is rescheduled

- GIVEN an event exists in scheduled status
- WHEN the household provides a new start time
- THEN the event schedule is updated
- AND participants and other event properties remain unchanged

#### Scenario: Cancelled event cannot be rescheduled

- GIVEN an event in cancelled status
- WHEN the household attempts to reschedule it
- THEN the operation is rejected

#### Scenario: Rescheduling with invalid times is rejected

- GIVEN a new end time that is earlier than the new start time
- WHEN the household attempts to reschedule the event
- THEN the operation is rejected

---

### Requirement: Event Cancellation

A household SHALL be able to cancel a scheduled event.

Cancellation preserves the event in history but removes it from active planning. Cancelled events cannot be rescheduled or receive new participants.

<!-- pdac:cite id="FR-CALENDAR-CANCEL-PLAN" digest="sha256:c915803cb87d6684644fb00805bc2d506fd1f07cae4403263c20666fb23b713f" -->

<!-- pdac:cite id="UC-CALENDAR-CANCEL-PLAN" digest="sha256:112064a4ebddfe5d53a46aad8feb3c9bc6ff1c101f8b7054baa26e7c89a12b3d" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:d0ffa51dd1b3cb43b9ccec2077dd08a91fc3e03e0124f8112b361e122abf94f9" -->

<!-- pdac:cite id="SB-CALENDAR-CANCEL-PLAN" digest="sha256:648f50fc7076e75beea7648a3c459057a7bf5452d9b6db8855f234aaf265ab2f" -->

<!-- pdac:cite id="SB-CALENDAR-CANCEL-CANCELLED-PLAN" digest="sha256:4b76cc35c9b6b2881bfb444f6e212f38a43223d3a4f6250d1b5ad03f52d2a5ab" -->

<!-- pdac-drift ids="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED, UC-CALENDAR-CANCEL-PLAN" summary="Spec only forbids rescheduling and adding participants on a cancelled event; the model (Q-0043) freezes a cancelled plan entirely: no participant removal, no reminder changes, no detail edits" -->

#### Scenario: Event is cancelled

- GIVEN an event exists in scheduled status
- WHEN the household cancels the event
- THEN the event status becomes cancelled
- AND it is no longer considered active

#### Scenario: Already cancelled event cannot be cancelled again

- GIVEN an event in cancelled status
- WHEN the household attempts to cancel it again
- THEN the operation is rejected

---

### Requirement: Event Participant Management

A household SHALL be able to add and remove participants from an event.

Participants may be family members, dependents, or pets. Each participant must be unique within the event. Participants cannot be added to a cancelled event.

<!-- pdac:cite id="FR-CALENDAR-MANAGE-PLAN-PARTICIPANTS" digest="sha256:c49632dc7e408cea8d44ca0643d7662abbcc67c96f0cfa2b2402cec1ef562c26" -->

<!-- pdac:cite id="UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS" digest="sha256:6663583cfeef4838a5801900727e5661db158fc37766ddc2dc55218df166da16" -->

<!-- pdac:cite id="BR-CALENDAR-PARTICIPANT-UNIQUE" digest="sha256:4a280f60bfa7b6d8696527afe55d5c15d93bc959e1bf6e5f89b9a154ed3aba2f" -->

<!-- pdac:cite id="BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD" digest="sha256:e7357786552892d48ee56f18e1141d513be6e603e1eec7f33d090e97f0103535" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:d0ffa51dd1b3cb43b9ccec2077dd08a91fc3e03e0124f8112b361e122abf94f9" -->

<!-- pdac:cite id="TERM-PLAN-PARTICIPANT" digest="sha256:9aec8030305ed4f94ceefe56c52cdfd679f58bae7b1b97c4c3f9014902074c96" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-PARTICIPANT" digest="sha256:89545defaceaba81ab32a06d5ebfe9f6eb362c7561941cb000d1190565383bae" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-DUPLICATE-PARTICIPANT" digest="sha256:854f901835d0d81a1472c3707d3a54ceb817fdccfe1b2a8ca963dcd4cc1f9335" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-PARTICIPANT-CANCELLED-PLAN" digest="sha256:eb9905ca369e8f24089b47c726473b6eccab937189fd2be00e32da5349857434" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-PARTICIPANT" digest="sha256:dddce8bf657b5b06a6ee4cb2aea6046d2152de5201f919596c2db5f59dbb317f" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-NON-PARTICIPANT" digest="sha256:bfe7e70e43315a1e0bb378f36f3b2e08b01ec630f5348db59ee58154df818000" -->

<!-- pdac-drift ids="TERM-PLAN-PARTICIPANT, SB-CALENDAR-ADD-PARTICIPANT, FR-CALENDAR-MANAGE-PLAN-PARTICIPANTS" summary="Spec allows family members, dependents or pets as participants; the model (Q-0042) allows household members of any role only, pets being members with the Pet role" -->

#### Scenario: Participant is added to an event

- GIVEN an event exists in scheduled status
- AND a family entity (member, dependent, or pet) is not already a participant
- WHEN the household adds that entity as a participant
- THEN the entity is added to the event's participant set

#### Scenario: Duplicate participant is rejected

- GIVEN an entity is already a participant in an event
- WHEN the household attempts to add the same entity again
- THEN the operation is rejected

#### Scenario: Participant cannot be added to a cancelled event

- GIVEN an event in cancelled status
- WHEN the household attempts to add a participant
- THEN the operation is rejected

#### Scenario: Participant is removed from an event

- GIVEN an entity is a participant in a scheduled event
- WHEN the household removes that participant
- THEN the entity is no longer in the event's participant set
- AND the event and other participants remain unchanged

#### Scenario: Removing a non-participant is rejected

- GIVEN an entity is not a participant in an event
- WHEN the household attempts to remove that entity
- THEN the operation is rejected

---

### Requirement: Event Reminders

A household SHALL be able to add and remove time-based reminders on an event.

Reminders are defined as offsets relative to the event's start time (e.g. 30 minutes before, 24 hours before). Each offset must be unique per event. Calendar defines reminder schedules; notification delivery belongs to infrastructure.

<!-- pdac:cite id="FR-CALENDAR-MANAGE-PLAN-REMINDERS" digest="sha256:f7fa0b9b501dd3b7e2895e798a0e5e493b0bdd32a5094951912b82610198a089" -->

<!-- pdac:cite id="UC-CALENDAR-MANAGE-PLAN-REMINDERS" digest="sha256:5b0dcdccd49f86bdfe459f343b99b8df689ec954929610c012de3c29c56cf03b" -->

<!-- pdac:cite id="BR-CALENDAR-REMINDER-OFFSET-UNIQUE" digest="sha256:7ee30d83e566648e630729d570868c47d687bca4be7ae6add761b5e0f9b2baf5" -->

<!-- pdac:cite id="BR-CALENDAR-REMINDER-BEFORE-START" digest="sha256:cc417232fbde7497405080d5df75d87cfbdf0350f8f1e2cd21e829389b5e3de9" -->

<!-- pdac:cite id="TERM-REMINDER" digest="sha256:96357bca9767a603e335970f75ed643c20e92492d07fb4efb2aa8c63732cd134" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-REMINDER" digest="sha256:352b4ecc30dc56951dca4feedbd0c0522b478d90083e2cf3ed8a434175d8a1bd" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-DUPLICATE-REMINDER" digest="sha256:8528c1af4c510a68c82ca313b4100fe01b2355c3bb740f956bfd59b1dfc183f0" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-REMINDER" digest="sha256:9626e48b3b21dbaf419f82511a2b9feec112c0ad5731152183aab607e21fe278" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-MISSING-REMINDER" digest="sha256:f7912153591dacba77d84c20799ae3fb93545a726b43f9b0d85b1798ea192b85" -->

<!-- pdac-drift ids="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED, FR-CALENDAR-MANAGE-PLAN-REMINDERS, UC-CALENDAR-MANAGE-PLAN-REMINDERS" summary="Spec lets reminders be added or removed on any existing event; the model (Q-0043) rejects adding or removing reminders on a cancelled plan" -->

#### Scenario: Reminder is added to an event

- GIVEN an event exists
- AND no reminder with the same offset already exists on that event
- WHEN the household adds a reminder with an offset
- THEN the reminder is added to the event's reminder set

#### Scenario: Duplicate reminder offset is rejected

- GIVEN a reminder with a specific offset already exists on an event
- WHEN the household adds a reminder with the same offset
- THEN the operation is rejected

#### Scenario: Reminder is removed from an event

- GIVEN a reminder with a specific offset exists on an event
- WHEN the household removes that reminder
- THEN the reminder is no longer part of the event's reminder set
- AND the event and other reminders remain unchanged

#### Scenario: Removing a non-existent reminder is rejected

- GIVEN no reminder with a specific offset exists on an event
- WHEN the household attempts to remove a reminder with that offset
- THEN the operation is rejected

---

### Requirement: Outlook Account Connection

A member SHALL be able to connect a Microsoft Outlook account to their DomusMind identity for read-only calendar ingestion.

Connecting establishes a delegated external calendar connection. Connection requires a successful delegated authorization including `Calendars.Read` and `offline_access` scopes. A member may not have two active connections to the same Outlook account. Connecting does not create native Event aggregates. The initial data import occurs through a subsequent sync operation. On connection creation, default sync settings are applied: a sync horizon of now − 1 day to now + 90 days forward, scheduled refresh enabled with a default interval of 60 minutes.

<!-- pdac:cite id="FR-CALENDAR-CONNECT-OUTLOOK" digest="sha256:9622bceda9cb6c52db6ecd691fdc235a9a677d16eee23180a31f6d02380bf388" -->

<!-- pdac:cite id="UC-CALENDAR-CONNECT-OUTLOOK" digest="sha256:f8c4aff4fa188eacc0a4014f41ab98b85f94446cca0daab97e36a8a67c4f1a5f" -->

<!-- pdac:cite id="JRN-CALENDAR-BRING-OUTLOOK-INTO-AGENDA" digest="sha256:c244eeaca0222ecd280c8170250bfa0d7e02d4703befbbffddedc04f5198d464" -->

<!-- pdac:cite id="ACT-MICROSOFT-OUTLOOK" digest="sha256:5f796b186efb4d164fd71462cce88157e4857e7e602f35acffee972881c33f54" -->

<!-- pdac:cite id="BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS" digest="sha256:930b71665ccb52c4a57896655dcefdbe469480c4ad1973c179c57197e1534645" -->

<!-- pdac:cite id="BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT" digest="sha256:64e4f60f3587511b2d16522e52c46318d37a336718636b251a4b4d4c05c884c5" -->

<!-- pdac:cite id="BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS" digest="sha256:92737f241a94bb8c8663837766c0035e4f8c64b12cc5409bef30289fb4dab848" -->

<!-- pdac:cite id="BR-CALENDAR-DEFAULT-REFRESH-HOURLY" digest="sha256:ddcfe57b8ec824f9206307c352a548d55614d3f681f690a10064ea0c05158e2f" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:2b2ea1de6c0f9a84d96f94d20087467fa7a198f197da80a85748fb145816fe03" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-CONNECTION" digest="sha256:e748e63deba44f835b829a1dba1e6a0a90592ca927ee34922701e5fbda5758b7" -->

<!-- pdac:cite id="TERM-SYNC-HORIZON" digest="sha256:fe2a373405ffcd5295b2bd2bd0a7ee58c8479f6af8096652834c72ec9e211756" -->

<!-- pdac:cite id="SB-CALENDAR-CONNECT-OUTLOOK" digest="sha256:0b741369ff7411f5f401fc7a05f21c48f56b0843e061f4153b495997d51c595d" -->

<!-- pdac:cite id="SB-CALENDAR-CONNECT-DUPLICATE-ACCOUNT" digest="sha256:142e4cf37ed79f6c3e5e7d29c5d4a5d1460501ec843288d8b20e8c549fdc473a" -->

<!-- pdac:cite id="SB-CALENDAR-CONNECT-MISSING-ACCESS" digest="sha256:b6c25c223884113f84aa6709f676222c4617e3ae77256169a5e02bbc9ed00e62" -->

#### Scenario: Member connects an Outlook account

- GIVEN a family member exists
- AND a successful delegated authorization for a Microsoft account is available
- WHEN the member connects the Outlook account
- THEN an external calendar connection is created associated with that member
- AND the connection status is pending initial sync
- AND no native Event aggregates are created

#### Scenario: Duplicate connection for the same account is rejected

- GIVEN a member already has an active connection to a specific Outlook account
- WHEN the member attempts to connect the same account again
- THEN the operation is rejected

#### Scenario: Connection with missing required scopes is rejected

- GIVEN a delegated authorization does not include the required scopes
- WHEN a member attempts to connect the account
- THEN the connection is rejected

---

### Requirement: External Calendar Configuration

A member SHALL be able to configure which provider calendars are selected for ingestion and which sync horizon applies to a connection.

Supported sync horizons: 30, 90, 180, or 365 days forward. Changing the selected calendars or the sync horizon triggers rehydration of affected feeds. Removed feeds stop projecting entries and their sync state is cleared. A configuration change must not create native Event aggregates.

<!-- pdac:cite id="FR-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR" digest="sha256:e3a1dcb7128c90af4e87da38aae9641e754c6a6ea81f2ed9e064c1164d2b6adc" -->

<!-- pdac:cite id="UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR" digest="sha256:2cfd9b4b0f71c7e62dcd49d1fda91cdbee19a21a49023c4c8eab24d6dbb9c3a0" -->

<!-- pdac:cite id="BR-CALENDAR-SUPPORTED-SYNC-HORIZONS" digest="sha256:108b36696099fec2a33f0a8a4ff5c86c7c4f6994300afc57f40d53fdcfca36af" -->

<!-- pdac:cite id="BR-CALENDAR-HORIZON-CHANGE-RELOADS" digest="sha256:29df7eb132b538b7223f5d4c728829260a754ba43aeb20fc3acbde6f475d126e" -->

<!-- pdac:cite id="BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT" digest="sha256:2203af1f822a02a6785362b0180282d26d90ac808ee9a319379b6ad7a89eacb8" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:2b2ea1de6c0f9a84d96f94d20087467fa7a198f197da80a85748fb145816fe03" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-FEED" digest="sha256:e7fa4ebe35ec7082c2703c35ec8dd11d9189ef5c7235f2fac15256ab4f4fbee5" -->

<!-- pdac:cite id="TERM-SYNC-HORIZON" digest="sha256:fe2a373405ffcd5295b2bd2bd0a7ee58c8479f6af8096652834c72ec9e211756" -->

<!-- pdac:cite id="SB-CALENDAR-CONFIGURE-SELECTION-AND-HORIZON" digest="sha256:2410482369386b7ab24c1213bfe9dcd25ee5189575dd0768653296b756f7b3ad" -->

<!-- pdac:cite id="SB-CALENDAR-DESELECT-FEED" digest="sha256:06f38c34100b4fc666124588593905e234502d6218768828df8dc82186278b41" -->

<!-- pdac:cite id="SB-CALENDAR-HORIZON-CHANGE-RELOADS" digest="sha256:4c7d4ae8971e2313dcc513e5551d3e70e3aea9a5efe43ba097b5d2b4ab1b2be9" -->

<!-- pdac:cite id="SB-CALENDAR-UNSUPPORTED-HORIZON" digest="sha256:2ecb510b3dcf2a52c9ce68632aef1c23bd347b68913304137121b6a18538ace2" -->

#### Scenario: Member selects calendars and sets a sync horizon

- GIVEN an active external calendar connection exists
- WHEN the member selects one or more provider calendars and sets a supported horizon value
- THEN the connection updates its selected feed set and horizon configuration

#### Scenario: Deselected feed stops projecting

- GIVEN a feed is currently selected and projecting entries
- WHEN the member deselects that feed
- THEN stored entries from that feed stop appearing in projections
- AND that feed's delta state is cleared

#### Scenario: Horizon change triggers rehydration

- GIVEN an active connection with a stored sync state
- WHEN the member changes the sync horizon to a different supported value
- THEN existing delta cursors for affected feeds are discarded
- AND the next sync performs a fresh bounded load for the new window

#### Scenario: Unsupported horizon value is rejected

- GIVEN a sync horizon value is not one of the supported options (30, 90, 180, 365 days)
- WHEN the member attempts to save the configuration
- THEN the operation is rejected

---

### Requirement: External Calendar Disconnection

A member SHALL be able to disconnect an external calendar connection.

Disconnection removes the connection and all its imported read-only entries from DomusMind. Disconnection does not modify any native Event aggregates. Imported entries owned by the connection stop appearing in Agenda projections immediately after disconnection. Disconnection removes only DomusMind's local integration state and locally held delegated access material — no operations are performed on the provider account or provider-side calendars.

<!-- pdac:cite id="FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR" digest="sha256:e00320683fbfcb03c01ce44e12874cc062b28d3acb054b2a68c69c3865e70f68" -->

<!-- pdac:cite id="UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR" digest="sha256:2e005d84005460d3bb48da21ccbd69a57c65964dc32c774bae93df627c8bb21b" -->

<!-- pdac:cite id="BR-CALENDAR-DISCONNECT-IS-LOCAL" digest="sha256:3592d1e7dca48282606e0a91d63e5b16fa6d6db6e0f1c48284daf03495e6e236" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:2b2ea1de6c0f9a84d96f94d20087467fa7a198f197da80a85748fb145816fe03" -->

<!-- pdac:cite id="SB-CALENDAR-DISCONNECT" digest="sha256:0e273000ade31e47012f03b1877137aa9ae718a0f0eb8f9a85238a610160705e" -->

<!-- pdac:cite id="SB-CALENDAR-DISCONNECT-UNKNOWN" digest="sha256:9517f9d00124173f5bfa4a3b44da1a7fc771133b6fbd3c8a6ddf72681c037598" -->

#### Scenario: Member disconnects a connection

- GIVEN an active external calendar connection exists
- WHEN the member disconnects it
- THEN the connection is removed
- AND all imported entries from that connection are cleared or tombstoned
- AND those entries no longer appear in Agenda projections
- AND no native Event aggregates are modified

#### Scenario: Disconnecting a non-existent connection is rejected

- GIVEN a connection ID that does not exist
- WHEN a disconnection is attempted
- THEN the operation is rejected

---

### Requirement: External Calendar Synchronization

A member SHALL be able to manually trigger synchronization of an external calendar connection.

Synchronization imports new, updated, and deleted occurrences from each selected feed within the active sync horizon. The sync operates incrementally using a stored delta cursor when one is available; otherwise it performs a fresh bounded load. Two synchronizations of the same connection must not run concurrently. Imported entries remain read-only.

<!-- pdac:cite id="FR-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:2e4013f14b0b0d3092f017991253bd2441931459bd1db5b24d0bc5a6c00dc423" -->

<!-- pdac:cite id="UC-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:bbecb1046bb1b9c284c6f69ed6a02879f41210f364542e9538d7bb63cd99045e" -->

<!-- pdac:cite id="BR-CALENDAR-NO-CONCURRENT-SYNC" digest="sha256:85649c4ba718266f5043e7a39f97fef18f3cdaec34b67e91bdb207cedb87974b" -->

<!-- pdac:cite id="BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT" digest="sha256:2203af1f822a02a6785362b0180282d26d90ac808ee9a319379b6ad7a89eacb8" -->

<!-- pdac:cite id="BR-CALENDAR-HORIZON-CHANGE-RELOADS" digest="sha256:29df7eb132b538b7223f5d4c728829260a754ba43aeb20fc3acbde6f475d126e" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:a3d8d4afcfc23492b745fa37ac5b103ae76a7bc1d214456aeddbe2f57473ceea" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-SYNC" digest="sha256:93a620fa8f638ab7d2b0e0d2daab578f1b020486617c96934cdf0f75db7f0f9d" -->

<!-- pdac:cite id="SB-CALENDAR-MANUAL-SYNC" digest="sha256:0d276534d141e1a084f552a5697c921114d24bc76cae01f6dbcc02a42262e310" -->

<!-- pdac:cite id="SB-CALENDAR-SYNC-INVALID-STATE" digest="sha256:b9ba219536f2c5f5d3d2273f88dd32553184e04a33827a6265886eca45e57de9" -->

<!-- pdac:cite id="SB-CALENDAR-SYNC-CONCURRENT" digest="sha256:cc832522ebc204a5fe2beef7c82586d4db6a4bea3c8fc54baeface829d2ae46b" -->

<!-- pdac:cite id="SB-CALENDAR-SYNC-NO-FEEDS" digest="sha256:0482cf39c885902cf7dae212ec1b4f694eb8c6a6da06b4f804e2ab31e09767de" -->

#### Scenario: Manual sync imports entries from selected feeds

- GIVEN an active connection with at least one selected feed
- WHEN the member triggers a sync
- THEN entries from all selected feeds within the horizon are imported or updated
- AND the sync cursor for each feed is updated

#### Scenario: Sync with invalid delta cursor falls back to fresh load

- GIVEN a stored delta cursor is invalid or stale
- WHEN a sync is triggered
- THEN local entries for the affected feed are cleared
- AND a fresh bounded load is performed
- AND a new cursor is stored

#### Scenario: Concurrent sync for the same connection is rejected

- GIVEN a sync is already running for a connection
- WHEN another sync for the same connection is triggered
- THEN the second request is rejected or deferred

#### Scenario: Sync with no selected feeds is a no-op

- GIVEN a connection has no selected feeds
- WHEN a sync is triggered
- THEN no entries are imported and the sync completes without error

---

### Requirement: Background Feed Refresh

The system SHALL automatically refresh stale external calendar connections in the background so that the Agenda stays current without requiring manual sync.

The default refresh interval is 60 minutes. Catch-up triggers also fire on user login and when Agenda is opened in Member scope with a stale connection. A connection is never synced concurrently with itself. A batch failure for one connection must not prevent other connections from refreshing. Background refresh must not create native Event aggregates.

<!-- pdac:cite id="FR-CALENDAR-BACKGROUND-REFRESH" digest="sha256:4a820cb4807164b225e417fbb59c0b4cba8a5b188bde26f1821e62a258b2aeb4" -->

<!-- pdac:cite id="UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS" digest="sha256:c85cb91e3305edcf423a102db0233adb71663d994849bf455f241e7d41e0e6b7" -->

<!-- pdac:cite id="ACT-CALENDAR-SYNC-SCHEDULER" digest="sha256:af7e16c4a05a0b76bf10ec69cc19e11e7b47c9dbf5c40adf437a3e842f5f1ef8" -->

<!-- pdac:cite id="BR-CALENDAR-DEFAULT-REFRESH-HOURLY" digest="sha256:ddcfe57b8ec824f9206307c352a548d55614d3f681f690a10064ea0c05158e2f" -->

<!-- pdac:cite id="BR-CALENDAR-NO-CONCURRENT-SYNC" digest="sha256:85649c4ba718266f5043e7a39f97fef18f3cdaec34b67e91bdb207cedb87974b" -->

<!-- pdac:cite id="BR-CALENDAR-REFRESH-FAILURES-ISOLATED" digest="sha256:f41e92fb7d9d444e72344a6a887650fa22f486659b5c9200bfae89edec108211" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:2b2ea1de6c0f9a84d96f94d20087467fa7a198f197da80a85748fb145816fe03" -->

<!-- pdac:cite id="SB-CALENDAR-REFRESH-STALE" digest="sha256:a9e578775f7a5f39b82e8bc7f4fc76f3422857a0eb25dbb0993d4c6568bcd153" -->

<!-- pdac:cite id="SB-CALENDAR-REFRESH-SKIP-FRESH" digest="sha256:167b540f5a1e0826914dbddd1b86654178eef99aca4acbdef3018d604535e55b" -->

<!-- pdac:cite id="SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN" digest="sha256:f1bf4c3d0285eab97812840a44d4b364fdaebc78bf5c443413ae144bd7d5f449" -->

#### Scenario: Stale connection is refreshed automatically

- GIVEN a connection's last successful sync is older than the configured threshold
- WHEN the background worker evaluates connections
- THEN that connection is synchronized
- AND the sync timestamp is updated

#### Scenario: Already-fresh connection is skipped

- GIVEN a connection was recently synchronized
- WHEN the background worker evaluates connections
- THEN that connection is skipped

#### Scenario: Catch-up sync fires when Agenda opens with stale state

- GIVEN a member opens Agenda in Member scope
- AND one of their connections has stale state
- WHEN the Agenda loads
- THEN a catch-up sync is triggered for the stale connection

---

### Requirement: Household Timeline Projection

The system SHALL produce a unified household-scope temporal read model for a requested date window.

The projection assembles entries from four sources: Calendar Events (as Plans), Tasks (due within the window), Routines (projected occurrences), and temporal Shared List Items (items with a due date, reminder, or repeat rule producing an occurrence within the window). External calendar entries are excluded from the household timeline — they appear in Member scope only. The projection is read-only; it must not create or modify any aggregate. A temporal list item linked to a plan projects independently of the linked event — they appear as separate entries in the timeline.

Projected entries follow this priority order within a day: overdue → tasks due today → list items with importance → plans (by start time) → routines → list items without importance → completed/checked.

<!-- pdac:cite id="FR-AGENDA-HOUSEHOLD-TIMELINE" digest="sha256:216ada3dc5c56e3e605c57b715ad660765aa2215be26da1ec2f5b91518a002a2" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-HOUSEHOLD-AGENDA" digest="sha256:3aa05500b8b3f5f223447760dc3b2d73fded5269ea3925ae117d4c0d93be4318" -->

<!-- pdac:cite id="TERM-HOUSEHOLD-TIMELINE" digest="sha256:af4c661643850c49998b6046ecfbf3ad3ec73b46dc61bc0d031acace8ed5b932" -->

<!-- pdac:cite id="TERM-AGENDA" digest="sha256:816f1a2f6d4e87da6b928b3c6d68433adab2c5467f4c02667e00fdb9b86387f0" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="BR-AGENDA-ENTRY-PRIORITY-ORDER" digest="sha256:99c2693fee545eedd4a3feb29da8d89e78edb05d8c68bba4afffc5073078cac6" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:b1dcb8fae830badbc584fe2a2e8b3ec117053470d9123391b2fa0d69f6bc9e69" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:07f2a39c4da3143f7ba1dc285acb840d8a121020a2c68adf87783c23b1e92af9" -->

<!-- pdac:cite id="BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT" digest="sha256:4c9723864e62308e6b5a0068872b52fcafd549eebd8837c69d9eef568c10ea42" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-ALL-SOURCES" digest="sha256:9a8969ad177e8f99c015d0e040f5086c439bb77cb2b3232296bf21b5695f2a23" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-LIST-ITEMS" digest="sha256:1963f96a8d4688cc0300947ff74f4515a07fb285df8abb622fc6a9da1dd3a466" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL" digest="sha256:314f199dbf9e633b3422db2e43359e1936f4c0fd61fd8c4ba4a18e76c2d3b2ea" -->

<!-- pdac-drift ids="BR-MEALS-AGENDA-PROJECTION, FR-MEALS-AGENDA-PROJECTION" summary="Spec assembles the household timeline from four sources; the model also projects meal slots into the Agenda, Meal Planning being current product (Q-0001)" -->

#### Scenario: Household timeline includes events, tasks, routines, and list items

- GIVEN a family has scheduled events, pending tasks, active routines, and list items with due dates
- WHEN the household timeline is requested for a date
- THEN all four entry types appear for that date
- AND external calendar entries do not appear

#### Scenario: Temporal list items project into the household timeline

- GIVEN a list item has a due date within the requested window
- WHEN the household timeline is requested
- THEN the list item appears with a list-item type discriminator
- AND it is distinguishable from tasks and plans

#### Scenario: External entries are excluded from the household timeline

- GIVEN a member has imported external calendar entries
- WHEN the household timeline is requested
- THEN those external entries do not appear

---

### Requirement: Member Agenda Projection

The system SHALL produce a member-scoped temporal read model for a requested date window.

The projection assembles entries from five sources: Calendar Events where the member participates or is a household plan, Tasks assigned to the member, Routines for the member or household scope, External Calendar Entries from the member's active connections, and temporal Shared List Items. External entries are scoped to the member whose connection owns them. External entries are read-only and carry a source label (e.g. "Outlook"). An external entry is included only when the connection is active, the feed is selected, the entry falls within the active sync horizon, and the entry has not been tombstoned as deleted. The projection is read-only; it must not create or modify any aggregate.

<!-- pdac:cite id="FR-AGENDA-MEMBER-AGENDA" digest="sha256:32782594f1e61544552dde7f833856d6bed5363a8be703366fc5636fb3d7b3b5" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-MEMBER-AGENDA" digest="sha256:2c870adbc231b6b1b0c661ab8062530786d0ef7d40c29ca48d6099cd46500fbc" -->

<!-- pdac:cite id="TERM-AGENDA-SCOPE" digest="sha256:7253db64d83dac0a1595a1f2c629b59e27d467f8677afcfa330f9ebf3cdaafc9" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-ENTRY" digest="sha256:9ef123050d8fc32bd7755be77ace1bab03fe0009c4583a0bdced3aff9aba6272" -->

<!-- pdac:cite id="BR-AGENDA-MEMBER-SCOPE-PLANS" digest="sha256:687c533e32506e30c54751e420d32bc5d6ca0683d04cb534977881f133c0e45b" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY" digest="sha256:c0b22c61a193c5202c7877f77b3b86a5f0251afabebafa9367623f63d3a4cf42" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:b1dcb8fae830badbc584fe2a2e8b3ec117053470d9123391b2fa0d69f6bc9e69" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:a3d8d4afcfc23492b745fa37ac5b103ae76a7bc1d214456aeddbe2f57473ceea" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-INCLUDES-IMPORTED" digest="sha256:b36d86ff2957c14ce9476a747fe05e0a8cb48b3c83c913066bf01400c78789cd" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL" digest="sha256:314f199dbf9e633b3422db2e43359e1936f4c0fd61fd8c4ba4a18e76c2d3b2ea" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY" digest="sha256:aca5be88f2c84c717d71bed6e7b84dc226ebd267d0bfc2a2e7f5c05cf78f54c6" -->

<!-- pdac-drift ids="BR-AGENDA-MEMBER-SCOPE-PLANS, FR-AGENDA-MEMBER-AGENDA, SB-AGENDA-MEMBER-HOUSEHOLD-PLAN-WITHOUT-PARTICIPANTS" summary="Spec includes every household plan in a member agenda; the model (Q-0041) includes the plans the person takes part in plus only household plans without participants" -->

#### Scenario: Member agenda includes personal events and imported entries

- GIVEN a member participates in an event and has an active Outlook connection with entries
- WHEN the member agenda is requested for that date
- THEN both the native event and the external entries appear
- AND external entries carry a source label and are read-only

#### Scenario: External entries do not appear in household scope

- GIVEN a member has imported external calendar entries
- WHEN the household-scope timeline (not member agenda) is requested
- THEN those external entries are not included

#### Scenario: Member agenda projected list items are not editable in-view

- GIVEN a list item with a due date appears in a member's agenda
- WHEN the member views the agenda
- THEN the list item is distinguishable from tasks and plans
- AND it is not editable from Agenda — editing navigates to the Lists surface

---

## Notes

1. **Recurring events** — `docs/_legacy/04_contexts/calendar.md` includes `RecurrenceRule` as a value object and mentions recurring events (e.g. "football practice every Tuesday"), but no feature spec covers creating or modifying recurring events beyond the basic schedule invariant (recurring events must define a recurrence rule). This spec captures the invariant. Full recurring event management (skip occurrence, move occurrence) is listed as future scope in the context document.

<!-- pdac-drift ids="TERM-PLAN" summary="Note says the spec captures a recurrence-rule invariant; the model (Q-0044) has no current recurrence behaviour and plans recurring-plan management for V2" -->

2. **Event completion** — `CompleteEvent` is listed as a future command in the context document. No feature spec exists. The context document notes "completed events cannot change schedule" as an invariant, which implies the state exists but its behavioral lifecycle is unspecified. This spec does not include a Completion requirement; the invariant is noted here only.

3. **RenameEvent** — `RenameEvent` is listed as an active command in the context document but has no feature spec. Behavior is unspecified beyond identity stability.

<!-- pdac-drift ids="FR-CALENDAR-EDIT-PLAN-DETAILS, UC-CALENDAR-EDIT-PLAN-DETAILS" summary="Note says renaming is unspecified; the model defines editing a plan's title, description and colour as current product" -->

4. **Reminder scheduling vs. delivery** — Reminders are defined as time offsets on the event. Delivery (notification infrastructure) is explicitly out of Calendar's scope. Whether reminders auto-remove when an event is cancelled is not specified in any source.

5. **Participant removal from cancelled events** — `cancel-event.md` says cancelled events cannot add participants, but neither it nor `remove-event-participant.md` explicitly states whether removing a participant from a cancelled event is permitted. Not specified.

<!-- pdac-drift ids="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED, TERM-REMINDER" summary="Notes 4-5 leave reminders and participant removal on cancelled events unspecified; the model (Q-0043) keeps reminders as they are and rejects any participant or reminder change on a cancelled plan" -->

6. **Phase 1 sync: pull only** — External calendar synchronization is pull-based only. Webhook subscriptions are explicitly out of scope for Phase 1.

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:7c5c21e2aa317fb9e77b5e4feafe491cfd68b7506203b47bae37b46392d185cb" -->

7. **`pausedUntil` for connections** — No equivalent time-bounded pause exists in the Calendar external connection model (unlike Routine Pause in Tasks). Connections are either active or disconnected.

8. **Background refresh triggers** — Catch-up triggers fire on login and when Agenda opens in Member scope. The exact staleness threshold (distinct from the 60-minute scheduled interval) is not specified in the source documents.

---

## Source References

- `docs/_legacy/04_contexts/calendar.md` — primary context document: aggregates, invariants, commands, events, boundary rules, external calendar model
- `specs/features/calendar/schedule-event.md`
- `specs/features/calendar/reschedule-event.md`
- `specs/features/calendar/cancel-event.md`
- `specs/features/calendar/add-event-participant.md`
- `specs/features/calendar/remove-event-participant.md`
- `specs/features/calendar/add-reminder.md`
- `specs/features/calendar/remove-reminder.md`
- `specs/features/calendar/connect-outlook-account.md`
- `specs/features/calendar/configure-external-calendar-connection.md`
- `specs/features/calendar/disconnect-external-calendar-connection.md`
- `specs/features/calendar/sync-external-calendar-connection.md`
- `specs/features/calendar/refresh-external-calendar-feeds.md`
- `specs/features/calendar/view-family-timeline.md`
- `specs/features/calendar/view-member-agenda.md`
- `docs/_legacy/03_domain/ubiquitous-language.md` — Plan vs Event terminology, Agenda architectural invariant
