# Calendar Specification

<!-- pdac-scope: cited -->

## Purpose

Calendar is the temporal structure of household life. It owns **Plans**: time-bound commitments that affect one or more household members.

Calendar answers:

- what is happening in this household
- when it happens
- who is involved

Calendar is the source of truth for time-based planning. It owns event schedules, participants, and reminder definitions. Notification delivery belongs to infrastructure, not to Calendar.

Calendar also manages **external calendar connections** — member-scoped delegated integrations that import read-only entries from third-party providers (Phase 1: Microsoft Outlook) into the household's temporal read surfaces.

Calendar also serves private, revocable ICS subscriptions for outbound read-only sharing of DomusMind Plans. This is separate from importing external calendars. It owns standalone dated family timeline notes, which do not change a Plan or another household record.

Calendar does not own the Agenda or household timeline read models. It is one source for those projections, alongside Tasks, Lists, Meal Planning, and external calendar connections in Member scope.

The household-facing term is **Plan**; `Event` is a domain-code synonym for the same concept.

Imported external calendar data is read-only integration state. It must never be treated as native household planning state.

<!-- pdac:cite id="BC-CALENDAR" digest="sha256:a365941e30ed9ca7e5a897db9bfbf057484fb0f87caa0c7a2dc4fdb1275c771f" -->

<!-- pdac:cite id="TERM-PLAN" digest="sha256:a1ff0f3de21aac20325b367ed1898b3eb3688f97d6c3e0b60b798c724025b07b" -->

<!-- pdac:cite id="TERM-CALENDAR-SUBSCRIPTION" digest="sha256:cedc0b1c2b89b20e16f3332e222c5db3aedd6c2ad530cb30a74e665f0ef72c83" -->

<!-- pdac:cite id="TERM-AGENDA" digest="sha256:a83d53e6c8e54bf187b9038ee34722a44ddd5c98211e1c69089db40b96b26014" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:f538589cc3d3dd54bfc8539b59e38f13f87b49547cf25e651e8ce2906810583e" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:0a46d8c63e8a5ca97a1757e7ed3e0528c2c10779a63621783793131b2bf55522" -->

## Requirements

### Requirement: Standalone dated family timeline notes

An authorized household member can create a family timeline note with a date and note text. A note is a standalone Calendar record and appears in the household timeline on its date without changing a Plan or another area record.

<!-- pdac:cite id="BC-CALENDAR" digest="sha256:a365941e30ed9ca7e5a897db9bfbf057484fb0f87caa0c7a2dc4fdb1275c771f" -->

#### Scenario: Member records a dated family note

- GIVEN the member can access the family
- WHEN the member records a note with a date and text
- THEN Calendar stores a standalone dated family note
- AND the note appears in the household timeline on that date

---

### Requirement: Plan Scheduling

A household SHALL be able to schedule a Plan with a title and a start date as the minimum required inputs. The start may include a time of day, or may be date-only for an all-day or multi-day Plan.

Optional inputs include an end after the start, household member participants (of any role), reminders, an optional description and colour, and an Area reference for context. A new Plan starts scheduled and belongs to exactly one household.

<!-- pdac:cite id="FR-CALENDAR-SCHEDULE-PLAN" digest="sha256:5002367b45c348e6429316409c63fb3c5c20dff05fc5e15f2cd76bf6b91ef7ca" -->

<!-- pdac:cite id="UC-CALENDAR-SCHEDULE-PLAN" digest="sha256:be241830302906c9a08258b12b322a738c73ca574a0f755e65063fe77defa743" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START" digest="sha256:285002bc7e1031cb3d8357f5d6c9a63ebf2a3d7aae3518884b46eecd846ab03f" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-END-AFTER-START" digest="sha256:6d0419337b4972f996926be897f47ef476618d4c0625aeb2b329502d5b6371ad" -->

<!-- pdac:cite id="BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD" digest="sha256:f623ece46da14fa7a170c70b043953ba411e57fa85f5c4a040b1fff91cdcd645" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-BELONGS-TO-ONE-HOUSEHOLD" digest="sha256:49a257891acf96132d3b06683a506f5d8bc0967daddbbbcb8e992dd9049d000f" -->

<!-- pdac:cite id="TERM-PLAN" digest="sha256:a1ff0f3de21aac20325b367ed1898b3eb3688f97d6c3e0b60b798c724025b07b" -->

<!-- pdac:cite id="TERM-PLAN-PARTICIPANT" digest="sha256:d26d0159e743bf71f303e354e9c99b328c0601388d7378c6e2fb1ea03d6126b4" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-PLAN-MINIMUM" digest="sha256:6eca8e70d013bcaacfa9b6c4c7193fb1ce6c9edc78655c0fe5a856f7306c0a45" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-PLAN-END-BEFORE-START" digest="sha256:aa28c4c106a13ac18df98b31ffbad0634055660ff1d8a7820b22db1a2fef8b20" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-PLAN-UNKNOWN-PARTICIPANT" digest="sha256:d0285426afc652cf64e1784ea223f40cf3a2e9144e4e0c1b05bbfba892d8fe0f" -->

<!-- pdac:cite id="SB-CALENDAR-SCHEDULE-ALL-DAY-PLAN" digest="sha256:1cee6b7ebccd3f4ed7cc6470f0b8a33913bb19d62f0d0e2db427be66331d46b2" -->

#### Scenario: Household creates a Plan with minimum inputs

- GIVEN a household exists
- WHEN a member schedules a Plan with a title and start date
- THEN a Plan is created in scheduled status
- AND the Plan belongs to that household

#### Scenario: Household creates an all-day Plan

- GIVEN a household exists
- WHEN a member schedules a Plan with a title and a date without a time of day
- THEN a scheduled all-day Plan exists for that date

#### Scenario: Household creates a multi-day Plan

- GIVEN a household exists
- WHEN a member schedules a Plan spanning multiple dates without a time of day
- THEN a scheduled multi-day Plan exists across those dates

#### Scenario: Plan with end before start is rejected

- GIVEN a member provides a start time and an end time
- WHEN the end time is earlier than the start time
- THEN the Plan is not created
- AND a validation error is returned

#### Scenario: Plan with an invalid participant reference is rejected

- GIVEN a participant identifier does not belong to the family
- WHEN a member attempts to schedule an event including that participant
- THEN the Plan is not created
- AND a validation error is returned

---

### Requirement: Event Rescheduling

A household SHALL be able to update the schedule of a scheduled Plan.

Rescheduling updates the start and optionally the end. A Plan's identity, participants, and reminders remain unchanged. A cancelled Plan cannot be changed.

<!-- pdac:cite id="FR-CALENDAR-RESCHEDULE-PLAN" digest="sha256:a8b70f8c982285b88c71dd71da704fdc7ff3509b8a991eacc675e5730cb15a16" -->

<!-- pdac:cite id="UC-CALENDAR-RESCHEDULE-PLAN" digest="sha256:614a41bed3c42e2c3c77f34c2b7dea394d3cf0714d2e326cdb84405f4039217f" -->

<!-- pdac:cite id="BR-CALENDAR-PLAN-END-AFTER-START" digest="sha256:6d0419337b4972f996926be897f47ef476618d4c0625aeb2b329502d5b6371ad" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:599c500bb2d7c2fe55ab003998aad87f2aa58f667553ba880045ed02a70120a9" -->

<!-- pdac:cite id="SB-CALENDAR-RESCHEDULE-PLAN" digest="sha256:dcf03361cf3257f87fe790a12a4fe120da8dcf9f1a0f4e3ae9db2f9ad124cedf" -->

<!-- pdac:cite id="SB-CALENDAR-RESCHEDULE-CANCELLED-PLAN" digest="sha256:c4da368bf019f8373db923f9f98330546b5dc28614732e33172f4b06113e7b77" -->

<!-- pdac:cite id="SB-CALENDAR-RESCHEDULE-INVALID-TIMES" digest="sha256:ae47b032ef0620b656f3e32d49454eddc80e607650c017c97aee51fe1e5106c9" -->

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

A household SHALL be able to cancel a scheduled Plan.

Cancellation preserves the Plan in history but removes it from active planning. Once cancelled, a Plan cannot be changed: it cannot be rescheduled or cancelled again; participants cannot be added or removed; reminders cannot be added or removed; and title, description, or colour cannot be edited.

<!-- pdac:cite id="FR-CALENDAR-CANCEL-PLAN" digest="sha256:53b73f69861626f442cc19410a78860736db1ec7aafcb119b1e7e7d3520cc598" -->

<!-- pdac:cite id="UC-CALENDAR-CANCEL-PLAN" digest="sha256:16f41091b62c6a5c17afce8708424bb7f768ef8ec54c028dd47c0b10ca7c1a56" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:599c500bb2d7c2fe55ab003998aad87f2aa58f667553ba880045ed02a70120a9" -->

<!-- pdac:cite id="SB-CALENDAR-CANCEL-PLAN" digest="sha256:0f3fcb8c9fb6afd6778ae7e62177eb35fe720a75e558228cfc880f6fd76a2bf3" -->

<!-- pdac:cite id="SB-CALENDAR-CANCEL-CANCELLED-PLAN" digest="sha256:cfff3ff5d8a8c573a7aac5c289fb4862225f35f4c6fafa2c848502b3102d843d" -->

<!-- pdac:cite id="SB-CALENDAR-CHANGE-REMINDER-CANCELLED-PLAN" digest="sha256:3b9d7c2f3a373f14dd6da506e39a66eaec8d42465de25c38d8cfd13b0c7ea15d" -->

#### Scenario: Event is cancelled

- GIVEN an event exists in scheduled status
- WHEN the household cancels the event
- THEN the event status becomes cancelled
- AND it is no longer considered active

#### Scenario: Already cancelled event cannot be cancelled again

- GIVEN an event in cancelled status
- WHEN the household attempts to cancel it again
- THEN the operation is rejected

#### Scenario: A cancelled Plan cannot be changed

- GIVEN a Plan is cancelled
- WHEN a household member tries to change its schedule, participants, reminders, title, description, or colour
- THEN the operation is rejected
- AND the Plan remains unchanged

---

### Requirement: Plan Details Editing

A household member SHOULD be able to change a scheduled Plan's title, description, and colour without changing its identity or schedule. A cancelled Plan cannot be edited.

<!-- pdac:cite id="FR-CALENDAR-EDIT-PLAN-DETAILS" digest="sha256:05fb5aad0a232d2e82327ad16fd64f738035088cf8647b9d6da5b796d3f8d764" -->

<!-- pdac:cite id="UC-CALENDAR-EDIT-PLAN-DETAILS" digest="sha256:ae9fa81ee1600c3d1ff76dc2462d6592571cc167c140fb1745dcb6e7e92757fd" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:599c500bb2d7c2fe55ab003998aad87f2aa58f667553ba880045ed02a70120a9" -->

#### Scenario: Member edits a scheduled Plan's details

- GIVEN a scheduled Plan exists
- WHEN a household member changes its title, description, or colour
- THEN those details are updated
- AND the Plan's identity, schedule, participants, and reminders remain unchanged

#### Scenario: Member cannot edit a cancelled Plan

- GIVEN a Plan is cancelled
- WHEN a household member tries to edit its title, description, or colour
- THEN the operation is rejected
- AND the Plan remains unchanged

---

### Requirement: Event Participant Management

A household SHALL be able to add and remove participants from an event.

Each participant must be a household member and may appear only once on a Plan. Participants cannot be added to or removed from a cancelled Plan.

<!-- pdac:cite id="FR-CALENDAR-MANAGE-PLAN-PARTICIPANTS" digest="sha256:1e28b7ae34e7fea44abc0475780fbf89bbc3a19c192fdca5272ed07db498b66c" -->

<!-- pdac:cite id="UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS" digest="sha256:accc944b47bbe87b04554585e32df9a9bdf952538173877b7332752a8ee3e8bc" -->

<!-- pdac:cite id="BR-CALENDAR-PARTICIPANT-UNIQUE" digest="sha256:b690a242a9d3d0ef4a1db0a8bebc1aef35a991b1a0a27e10f84eebc3dca78282" -->

<!-- pdac:cite id="BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD" digest="sha256:f623ece46da14fa7a170c70b043953ba411e57fa85f5c4a040b1fff91cdcd645" -->

<!-- pdac:cite id="BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED" digest="sha256:599c500bb2d7c2fe55ab003998aad87f2aa58f667553ba880045ed02a70120a9" -->

<!-- pdac:cite id="TERM-PLAN-PARTICIPANT" digest="sha256:d26d0159e743bf71f303e354e9c99b328c0601388d7378c6e2fb1ea03d6126b4" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-PARTICIPANT" digest="sha256:a668d057a0cb15861bdc29a2db948b80df094f9f7bbc8010400f3915b0c345c8" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-DUPLICATE-PARTICIPANT" digest="sha256:8ad0a40cc4e8ad521329be5954b5696702164334e147e779cd5752fd763853b8" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-PARTICIPANT-CANCELLED-PLAN" digest="sha256:4e66bc1d19ad6bac18675f3040358a5ec3f40f98b2ea8e6722c8d577246fd668" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-PARTICIPANT" digest="sha256:7a3168aefbee3875be165bdbb01625cc71bd2fd55366fe0053186eb9b50ebdfd" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-NON-PARTICIPANT" digest="sha256:da3e7d4e873e5e51abbdbfc4453a8a702c33826b63c3ed4502644796d5c9de55" -->

#### Scenario: Participant is added to an event

- GIVEN an event exists in scheduled status
- AND a household member is not already a participant
- WHEN the household adds that member as a participant
- THEN the member is added to the Plan's participant set

#### Scenario: Duplicate participant is rejected

- GIVEN an entity is already a participant in an event
- WHEN the household attempts to add the same entity again
- THEN the operation is rejected

#### Scenario: Participant cannot be added to a cancelled event

- GIVEN an event in cancelled status
- WHEN the household attempts to add a participant
- THEN the operation is rejected

#### Scenario: Participant is removed from a Plan

- GIVEN an entity is a participant in a scheduled event
- WHEN the household removes that participant
- THEN the entity is no longer in the event's participant set
- AND the event and other participants remain unchanged

#### Scenario: Participant cannot be removed from a cancelled Plan

- GIVEN a cancelled Plan has a participant
- WHEN the household attempts to remove that participant
- THEN the operation is rejected
- AND the participant remains on the Plan

#### Scenario: Removing a non-participant is rejected

- GIVEN an entity is not a participant in an event
- WHEN the household attempts to remove that entity
- THEN the operation is rejected

---

### Requirement: Event Reminders

A household SHALL be able to add and remove time-based reminders on a scheduled Plan.

Reminders are defined as offsets relative to the event's start time (e.g. 30 minutes before, 24 hours before). Each offset must be unique per event. Calendar defines reminder schedules; notification delivery belongs to infrastructure.

<!-- pdac:cite id="FR-CALENDAR-MANAGE-PLAN-REMINDERS" digest="sha256:134cc135b54a2d2972ffd16d3bc76930495e33a836558fd605d96849dd8b560c" -->

<!-- pdac:cite id="UC-CALENDAR-MANAGE-PLAN-REMINDERS" digest="sha256:4d20b53488a2c974a97cdd0125d646c0f2b4d49a32ff4ac3cc010e8bb8affb9d" -->

<!-- pdac:cite id="BR-CALENDAR-REMINDER-OFFSET-UNIQUE" digest="sha256:c01cb2c91642c63975eb58fc956c40913bfad385ffea1c0b69b97f8ddd118a1e" -->

<!-- pdac:cite id="BR-CALENDAR-REMINDER-BEFORE-START" digest="sha256:f2d7b2ea5566604e52fc9639e3d7a31efe73cfe682fd21926e8fe19c4e0eed4b" -->

<!-- pdac:cite id="TERM-REMINDER" digest="sha256:a5ca233e79dc46068bff49c2b45fc8468b5b0f64e1b6ff962e12f628734a4e3e" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-REMINDER" digest="sha256:7187bfa5569469aa3d110d79fe757c79208bf6e767fd32ebf87f451e0049acec" -->

<!-- pdac:cite id="SB-CALENDAR-ADD-DUPLICATE-REMINDER" digest="sha256:1e83384c59bdbda6e89b7fb867f28adddd29c565ccb56a86c5a19cac60dca966" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-REMINDER" digest="sha256:b0d2e277e6afe0aba8d16fe3fa1a3f1fd663735984ba36053d2c0e7f122d0bf1" -->

<!-- pdac:cite id="SB-CALENDAR-REMOVE-MISSING-REMINDER" digest="sha256:a4861167aedb5073221e926ebf2b5460b002219d1f7cec4e44192724e3e5a056" -->

#### Scenario: Reminder is added to an event

- GIVEN a scheduled Plan exists
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

#### Scenario: Reminders cannot be changed on a cancelled Plan

- GIVEN a cancelled Plan has a reminder
- WHEN the household attempts to add or remove a reminder
- THEN the operation is rejected
- AND the Plan's reminders remain unchanged

#### Scenario: Removing a non-existent reminder is rejected

- GIVEN no reminder with a specific offset exists on an event
- WHEN the household attempts to remove a reminder with that offset
- THEN the operation is rejected

---

### Requirement: Outlook Account Connection

A member SHALL be able to connect a Microsoft Outlook account to their DomusMind identity for read-only calendar ingestion.

Connecting establishes a delegated external calendar connection. Connection requires a successful delegated authorization including `Calendars.Read` and `offline_access` scopes. A member may not have two active connections to the same Outlook account. Connecting does not create native Event aggregates. The initial data import occurs through a subsequent sync operation. On connection creation, default sync settings are applied: a sync horizon of now − 1 day to now + 90 days forward, scheduled refresh enabled with a default interval of 60 minutes.

<!-- pdac:cite id="FR-CALENDAR-CONNECT-OUTLOOK" digest="sha256:50cfc7aa037888b885e20f91a1a152ec089c55f4f6fa051fcb2d31a8a227a768" -->

<!-- pdac:cite id="UC-CALENDAR-CONNECT-OUTLOOK" digest="sha256:2b5979801bddd1324f3c23b0958b000afe550e98e17a11ce792c2ea54235c034" -->

<!-- pdac:cite id="JRN-CALENDAR-BRING-OUTLOOK-INTO-AGENDA" digest="sha256:525b0a9415a662490181aa092811e0cad1f0c48fffd408f3d2505c8e70a4dc05" -->

<!-- pdac:cite id="ACT-MICROSOFT-OUTLOOK" digest="sha256:a274a18ef29171d475391275699318abf8c93ee7fa3daf494741dbe1fce6af66" -->

<!-- pdac:cite id="BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS" digest="sha256:556b0ae693f9cb4396113be0e3c161fd22a811c5d94db7d976635570241ad1f1" -->

<!-- pdac:cite id="BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT" digest="sha256:7ebb4f852677a608d2037cf65b1910e49d3a2e4ee13cb70e59a83cf525777e94" -->

<!-- pdac:cite id="BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS" digest="sha256:a3c89a729bbc1b61d0b8104ceacce71dc29a6e915ab2a90834c869b72c0a9da8" -->

<!-- pdac:cite id="BR-CALENDAR-DEFAULT-REFRESH-HOURLY" digest="sha256:c575f89178139004976122190d8a13c8136f3992a7a9190d2fd419859c41f54a" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:f538589cc3d3dd54bfc8539b59e38f13f87b49547cf25e651e8ce2906810583e" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-CONNECTION" digest="sha256:1767194797bbc765747ba93591e40552d12a1667a5a5c2086c827c7bb0c3bc8d" -->

<!-- pdac:cite id="TERM-SYNC-HORIZON" digest="sha256:d5e8c5b396642312a552935a31f6134a752040f75fb821ceb329e545f6aac81f" -->

<!-- pdac:cite id="SB-CALENDAR-CONNECT-OUTLOOK" digest="sha256:1b65fce3b6a17f8b0aafbe33bb30a4b9ae43e0cc5fa51d39a2bba8fa5489c346" -->

<!-- pdac:cite id="SB-CALENDAR-CONNECT-DUPLICATE-ACCOUNT" digest="sha256:f3900665713dc2f78171d2306284576fe3fdc4e676b9757e73af1b3bd2ee2949" -->

<!-- pdac:cite id="SB-CALENDAR-CONNECT-MISSING-ACCESS" digest="sha256:736cc5226870fe1cda9a6cc175028c4642e3be9205a3e9a6da387857b4f20a3b" -->

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

<!-- pdac:cite id="FR-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR" digest="sha256:65ebb0f18b44b6f257e75f978ff023fea6449144b23bdc59c4f77665a37fc5e8" -->

<!-- pdac:cite id="UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR" digest="sha256:eaf278bf6b3a77c0dc9ff934b444ab82b5aa23dac9c882f3c16ed698f1f410ff" -->

<!-- pdac:cite id="BR-CALENDAR-SUPPORTED-SYNC-HORIZONS" digest="sha256:5d1b47fcee5b94be9694ea0fa110b322e2a49bad64c13f55960f9ba08d041adf" -->

<!-- pdac:cite id="BR-CALENDAR-HORIZON-CHANGE-RELOADS" digest="sha256:79b8c9607677595440cf3ec07c3501c38a75c4a6fdb12a17365fe24803cbcfc8" -->

<!-- pdac:cite id="BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT" digest="sha256:99f6fe7d4fa4be123e4b54cc5e23a54b6f9810ffc173e74c7295ea1c6e6baf9e" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:f538589cc3d3dd54bfc8539b59e38f13f87b49547cf25e651e8ce2906810583e" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-FEED" digest="sha256:5624402938f25e7089946fa295798b0c0152942eb66ddf9ec900619d2d0a84ea" -->

<!-- pdac:cite id="TERM-SYNC-HORIZON" digest="sha256:d5e8c5b396642312a552935a31f6134a752040f75fb821ceb329e545f6aac81f" -->

<!-- pdac:cite id="SB-CALENDAR-CONFIGURE-SELECTION-AND-HORIZON" digest="sha256:1d30d2d05e242313bb1b6df92582aabf7d14327baa86c663f490a8c13aa9ce43" -->

<!-- pdac:cite id="SB-CALENDAR-DESELECT-FEED" digest="sha256:90876b7145514f5b21efdc45116772200cb482f879c93de9394ccc68f7d610be" -->

<!-- pdac:cite id="SB-CALENDAR-HORIZON-CHANGE-RELOADS" digest="sha256:32a934be9368ab2a4f217b0efdb3b31f5a6c73abbdc43ef10da1da29a3cbf5d4" -->

<!-- pdac:cite id="SB-CALENDAR-UNSUPPORTED-HORIZON" digest="sha256:0ef2bf7bdb22fe129bb9ccdf5a9b808c2213742359b664c6499e472b5b3da8a9" -->

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

<!-- pdac:cite id="FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR" digest="sha256:7bfdd307fcfa7a2c2144d03ccd965f0128ce4b48a8f121cba03ef139b7de5f3b" -->

<!-- pdac:cite id="UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR" digest="sha256:3a3e5fa7533474185fbda7d556bb496ff71feb16302a5e219ff587eba5a386bc" -->

<!-- pdac:cite id="BR-CALENDAR-DISCONNECT-IS-LOCAL" digest="sha256:4b321feb9a5f47fbcc724b1e6d70a152b70dc67343f5a0d518470aaeb67f19d8" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:f538589cc3d3dd54bfc8539b59e38f13f87b49547cf25e651e8ce2906810583e" -->

<!-- pdac:cite id="SB-CALENDAR-DISCONNECT" digest="sha256:08ccad7fc87478aee53ebac67db39444dd69f4badb02aff22d6b89e14f8b8e06" -->

<!-- pdac:cite id="SB-CALENDAR-DISCONNECT-UNKNOWN" digest="sha256:96cbddcb5828d1d7b4bb861ab0218a4e981cc751ea06225292b0e2f813e6537a" -->

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

<!-- pdac:cite id="FR-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925" -->

<!-- pdac:cite id="UC-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:20397f50049cb36f540d4e65b7989c0f38be858535349e86daf4790319b37fe4" -->

<!-- pdac:cite id="BR-CALENDAR-NO-CONCURRENT-SYNC" digest="sha256:cb91537c8a00c86ee5a8efeb0ae78ae4d5c94f112a9f7445c4792e78295b0d78" -->

<!-- pdac:cite id="BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT" digest="sha256:99f6fe7d4fa4be123e4b54cc5e23a54b6f9810ffc173e74c7295ea1c6e6baf9e" -->

<!-- pdac:cite id="BR-CALENDAR-HORIZON-CHANGE-RELOADS" digest="sha256:79b8c9607677595440cf3ec07c3501c38a75c4a6fdb12a17365fe24803cbcfc8" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:0a46d8c63e8a5ca97a1757e7ed3e0528c2c10779a63621783793131b2bf55522" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-SYNC" digest="sha256:668fedd678fe9a2235b9bcebabf155ff3d18fdfdfb36e224f939daf41be97e99" -->

<!-- pdac:cite id="SB-CALENDAR-MANUAL-SYNC" digest="sha256:d2bb53909bbecb5dff7cfdc073dbd4f73fda69653322c0044b6785aadd914c3e" -->

<!-- pdac:cite id="SB-CALENDAR-SYNC-INVALID-STATE" digest="sha256:9605a38c6c360385f8371ef2b3f437b6866427195a5890a2632541ff897bdfd9" -->

<!-- pdac:cite id="SB-CALENDAR-SYNC-CONCURRENT" digest="sha256:f008c597906fc26b4326b2c9d9fdfe0f441b312037e758cb88068188187e362b" -->

<!-- pdac:cite id="SB-CALENDAR-SYNC-NO-FEEDS" digest="sha256:48bc3d8c7c4a4c71c9c2b94a5752f6dc987819b03b1bdb85c39083dee5dd94e9" -->

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

<!-- pdac:cite id="FR-CALENDAR-BACKGROUND-REFRESH" digest="sha256:6395fc275a0ad0d787e23defa29afc83213e042d2ecf161add0059c731822988" -->

<!-- pdac:cite id="UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS" digest="sha256:b9b47a9df92b0d6a7d456d2831e4359e16c414180f0ec2ccb000906a54c94ab5" -->

<!-- pdac:cite id="ACT-CALENDAR-SYNC-SCHEDULER" digest="sha256:93756f5008efb26952e8def217428fcc7601ce2adecce3b0dddf877757493a33" -->

<!-- pdac:cite id="BR-CALENDAR-DEFAULT-REFRESH-HOURLY" digest="sha256:c575f89178139004976122190d8a13c8136f3992a7a9190d2fd419859c41f54a" -->

<!-- pdac:cite id="BR-CALENDAR-NO-CONCURRENT-SYNC" digest="sha256:cb91537c8a00c86ee5a8efeb0ae78ae4d5c94f112a9f7445c4792e78295b0d78" -->

<!-- pdac:cite id="BR-CALENDAR-REFRESH-FAILURES-ISOLATED" digest="sha256:5b87e85948d6eb058eacf38cc24a2ee1a5b8afe75104318de9d208a7878366fe" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:f538589cc3d3dd54bfc8539b59e38f13f87b49547cf25e651e8ce2906810583e" -->

<!-- pdac:cite id="SB-CALENDAR-REFRESH-STALE" digest="sha256:2cf9fb91fc8460221c43e1f02ce2454e619ae4a3416dbbab49cc6165f12b4e8a" -->

<!-- pdac:cite id="SB-CALENDAR-REFRESH-SKIP-FRESH" digest="sha256:adcf47457405c4c43919233af05a0fe529dbdcaec2aab1b5a8cc84f76e183293" -->

<!-- pdac:cite id="SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN" digest="sha256:ac3ec6421a6dcf9c4c0f0a07350c93e5be1f7dc0faa0400a1b3b33d9d9e48404" -->

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

The Agenda assembles Plans, Tasks due within the window, projected Routine occurrences, temporal List Items, and Meal slots. External calendar entries are excluded from the household scope and appear only in the owning member's scope. The projection is read-only; it must not create or modify any record. A temporal List Item linked to a Plan projects independently of that Plan as a separate entry.

Projected entries follow this priority order within a day: overdue → tasks due today → list items with importance → plans (by start time) → routines → list items without importance → completed/checked.

<!-- pdac:cite id="FR-AGENDA-HOUSEHOLD-TIMELINE" digest="sha256:59ee527f3c3b47e2d6e747ad3374de5794d21c2b28897ff86548ccc936638df3" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-HOUSEHOLD-AGENDA" digest="sha256:06da1c10928a2e14a7b2ace4e421c5d68123aa381fe516d913f405bbf4a88893" -->

<!-- pdac:cite id="TERM-HOUSEHOLD-TIMELINE" digest="sha256:410a4fc4b9ef563f593931082c0a49dd92d6d3a90f41a31c56ec86dcb9d358a3" -->

<!-- pdac:cite id="TERM-AGENDA" digest="sha256:a83d53e6c8e54bf187b9038ee34722a44ddd5c98211e1c69089db40b96b26014" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="BR-AGENDA-ENTRY-PRIORITY-ORDER" digest="sha256:4bb5b5f53d3f74ab929768074734785133a92a7ea30c940f17bb0e353e63f1ba" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:c1f6a0090e071a7cbca68eb2f1b200e68156542ace4fe9a8ca0223f3aa71df32" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:043eef067ee7db97dd23b7dc3dc2e7c9b7c52b4c9f5aefc6e3d2066abd179350" -->

<!-- pdac:cite id="BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT" digest="sha256:2bdc7fe6d5bc3727361258ac8d5fd7e22f16d3056002e48b7eaa663a2ff1c150" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-ALL-SOURCES" digest="sha256:1259c37a9fe9ab08722ddf6abb550f9168ed8ae73ad40793bcc7108c3d86677f" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-LIST-ITEMS" digest="sha256:4bb5ce58eb9e9aca02dc0897f38d3f6ffe17afba2dff74f55947180e313027da" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL" digest="sha256:7783dc44b8b6ab700b8fb8b48ee763fe3e9b904ff78b9b37b7169da7ebad11a5" -->

<!-- pdac:cite id="BR-MEALS-AGENDA-PROJECTION" digest="sha256:fecf2e8fbd7fdf7674ba5f7d8870727b2d53f87d2da15c73e0bed19ffb4af998" -->

<!-- pdac:cite id="FR-MEALS-AGENDA-PROJECTION" digest="sha256:c647b93ceb2e80493b6ea0d5df2ab49ac174e5623a111df1a57cf0e03633561c" -->

<!-- pdac:cite id="SB-MEALS-AGENDA-SHOWS-ASSIGNED-MEAL" digest="sha256:a40268ca794a6a5d985fec6059d791c737a26fe70c48296e704d6ebe00a429d9" -->

#### Scenario: Household timeline includes events, tasks, routines, and list items

- GIVEN a family has scheduled events, pending tasks, active routines, and list items with due dates
- WHEN the household timeline is requested for a date
- THEN the Plan, task, routine, and List Item appear for that date
- AND external calendar entries do not appear

#### Scenario: Meal slots appear as distinct household Agenda entries

- GIVEN a meal slot has an assigned meal
- WHEN the household Agenda is requested for that date
- THEN the meal appears as a distinct, non-timed household entry
- AND an Unplanned slot without notes is not shown
- AND the meal is not a Plan

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

The projection includes Plans the member participates in and household Plans with no participants, Tasks assigned to the member, Routines for the member or household scope, temporal List Items, and External Calendar Entries from that member's active connections. A Plan whose participants are only other people is not shown. External entries are scoped to the member whose connection owns them, are read-only, and carry a source label (e.g. "Outlook"). An external entry is included only when the connection is active, the feed is selected, the entry falls within the active sync horizon, and the entry has not been tombstoned as deleted. The projection is read-only; it must not create or modify any record.

<!-- pdac:cite id="FR-AGENDA-MEMBER-AGENDA" digest="sha256:328356fe526ea42592dbe73abaf794a0e32b7764d771baffe1ce7bfd2ac462db" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-MEMBER-AGENDA" digest="sha256:500f25f0376043d38af118a8e6824e8c032a27831f7aa01719c04fda98dc2c44" -->

<!-- pdac:cite id="TERM-AGENDA-SCOPE" digest="sha256:7d64b169305cffdcfae55f7c66209f15ca27c3d00065ff7123253fc234d394e9" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-ENTRY" digest="sha256:a544452b97f1df5874ba287953f6e9ad857c23c3dd1902fc5f3d22bca412ca6b" -->

<!-- pdac:cite id="BR-AGENDA-MEMBER-SCOPE-PLANS" digest="sha256:fb1cc45142f9871212ec0c144e92003d0cdeeb427ce92397612c38ca1f2a175a" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY" digest="sha256:949e939555ba5883283a4fde1554e44e12a29571db306d54222f01114f3a782d" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:c1f6a0090e071a7cbca68eb2f1b200e68156542ace4fe9a8ca0223f3aa71df32" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:0a46d8c63e8a5ca97a1757e7ed3e0528c2c10779a63621783793131b2bf55522" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-INCLUDES-IMPORTED" digest="sha256:2e6e2071df4c87715883ec209e11c2f221c09046119a4bc5b393f3a1644c8370" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL" digest="sha256:7783dc44b8b6ab700b8fb8b48ee763fe3e9b904ff78b9b37b7169da7ebad11a5" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY" digest="sha256:8231ad3d04776f037c212779854491d81a971d0eca1fd320e617a373d65c93df" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-HOUSEHOLD-PLAN-WITHOUT-PARTICIPANTS" digest="sha256:d8d590d90b5ca72952ef0cb73e71e4fe022c50e4fe547cf21da0150a1eb8af53" -->

#### Scenario: Member agenda includes personal events and imported entries

- GIVEN a member participates in an event and has an active Outlook connection with entries
- WHEN the member agenda is requested for that date
- THEN both the native event and the external entries appear
- AND external entries carry a source label and are read-only

#### Scenario: Member Agenda includes unassigned household Plans only

- GIVEN a household has one Plan with no participants
- AND another Plan whose only participant is a different member
- WHEN this member's Agenda is requested
- THEN the Plan without participants appears
- AND the Plan for the other member does not appear

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

### Requirement: Calendar Subscriptions

Household members with access to the household timeline can create and manage private read-only ICS subscriptions. A subscription is scoped to either all household Plans or the Plans of one household member. It expires 12 months after creation; renewal creates a new subscription. The owner can see access metadata and revoke a subscription.

<!-- pdac:cite id="FR-CALENDAR-MANAGE-SUBSCRIPTIONS" digest="sha256:d6628e5804a8e959841593a3a5b86044b264a42b170ee6b1233c7e6e9ebd6f61" -->

#### Scenario: Member creates a household subscription

- **WHEN** a member with timeline access creates a labeled household subscription
- **THEN** DomusMind returns a private ICS URL once
- **AND** the subscription expires 12 months after creation
- **AND** the owner can see its label, scope, expiry, last successful access and access count

#### Scenario: Member creates a single-member subscription

- **WHEN** a member with timeline access selects one current household member
- **THEN** the feed contains only Plans that include that member

#### Scenario: Owner revokes a subscription

- **WHEN** the owner revokes an active subscription
- **THEN** later feed requests using its URL are denied

### Requirement: Read-only ICS Subscription Feed

An active subscription provides a read-only ICS feed of DomusMind Plans within its scope. The feed includes Plan title and timing only. Imported Outlook entries, descriptions, locations and participants are excluded. External clients cannot change DomusMind Plans.

<!-- pdac:cite id="FR-CALENDAR-SERVE-SUBSCRIPTION-FEED" digest="sha256:bc9e6ff594f9ef7ba062d7e02e5f50b42b20f7407ef9c9bbf0c5b1a6b011a454" -->

<!-- pdac:cite id="BR-CALENDAR-SUBSCRIPTION-ACCESS" digest="sha256:f7db6fc0557ada6415b104ae666d440ece7d3752dab4dcbcfff063274ce987d5" -->

#### Scenario: External client reads an active feed

- **WHEN** a client requests an active subscription
- **THEN** DomusMind returns an ICS feed with only Plan title and timing within the chosen scope
- **AND** successful access time and count are updated

#### Scenario: Inactive subscription is requested

- **WHEN** a client requests an invalid, revoked or expired subscription
- **THEN** DomusMind denies the request without returning calendar information

---

## Notes

1. **Recurring Plans** — Recurring-plan management is not current product behavior; it is planned for V2 (decided: Q-0044). This specification does not define recurrence rules or recurring occurrences.

1. **Event completion** — `CompleteEvent` is listed as a future command in the context document. No feature spec exists. The context document notes "completed events cannot change schedule" as an invariant, which implies the state exists but its behavioral lifecycle is unspecified. This spec does not include a Completion requirement; the invariant is noted here only.

2. **Plan details** — A household member may edit a scheduled Plan's title, description, or colour without changing its identity or schedule. See Plan Details Editing.

1. **Reminder scheduling vs. delivery** — Reminders are offsets relative to a Plan's start; delivery (notification infrastructure) is outside Calendar. Reminders remain unchanged when a Plan is cancelled, and cannot be added or removed after cancellation (decided: Q-0043).

2. **Cancelled Plan participants** — Participants remain unchanged when a Plan is cancelled; they cannot be added or removed after cancellation (decided: Q-0043).

1. **Phase 1 sync: pull only** — External calendar synchronization is pull-based only. Webhook subscriptions are explicitly out of scope for Phase 1.

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:c575d9e19b4520f13a09facea0c1196b6a42ec1f4554865cdb470485d46eb4b0" -->

1. **`pausedUntil` for connections** — No equivalent time-bounded pause exists in the Calendar external connection model (unlike Routine Pause in Tasks). Connections are either active or disconnected.

2. **Background refresh triggers** — Catch-up triggers fire on login and when Agenda opens in Member scope. The exact staleness threshold (distinct from the 60-minute scheduled interval) is not specified in the source documents.

---

## Source References

- `docs/04_contexts/calendar.md` — primary context document: aggregates, invariants, commands, events, boundary rules, external calendar model
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
- `docs/03_domain/ubiquitous-language.md` — Plan vs Event terminology, Agenda architectural invariant
