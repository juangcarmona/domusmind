# Tasks Specification

<!-- pdac-scope: cited -->

## Purpose

Tasks is the household's structured execution layer. It answers: what explicit work needs to be done, who is responsible for doing it, when it should happen, and whether it has been completed.

A **Task** is a concrete, actionable unit of household work. It has an assignee, a due date, and a defined lifecycle. Tasks are created explicitly — they are never automatically generated from Routines or Calendar events.

A **Routine** is a recurring operational definition. It describes household work that repeats on a schedule. A Routine does not produce Task aggregates; it is projected on-the-fly into read surfaces on dates that match its recurrence rule.

Tasks and Routines belong to the Tasks context. A list item that carries a due date is not a Task. A recurring calendar plan is not a Routine. These distinctions are firm.

<!-- pdac:cite id="BC-TASKS" digest="sha256:30abcef1a1eaa3f38dfcb0ada4d54bb3a4f83b4b9c50f1ae3d68b0faa4f835d3" -->

<!-- pdac:cite id="TERM-TASK" digest="sha256:56e9e73c7427f627b729be313364b0e56d485c7be6eb95ece3f5fdf44a3de676" -->

<!-- pdac:cite id="TERM-ROUTINE" digest="sha256:b3f7531e4bd47453d7459e7c09af054bdfaeda3eb93b5df12a35d5b03fbf3a0a" -->

<!-- pdac:cite id="BR-TASKS-TASKS-CREATED-EXPLICITLY" digest="sha256:ab5dfe6e8bdcd0fb7ce78fbf145e2ddd52d1c8b1699647db81cefbb73067ad8b" -->

<!-- pdac:cite id="BR-TASKS-ROUTINES-PROJECTED" digest="sha256:81ea5e83e732a25b07646975f97691092b26fe58b06590a4e1c68e6310e65983" -->

<!-- pdac:cite id="BR-TASKS-DISTINCT-EXECUTION-MODELS" digest="sha256:295866d5d14db84b795007cb1682e24300f976f2b390f1b1c282bbfac6e52848" -->

---

## Requirements

### Requirement: Task Creation

A household SHALL be able to create a task with a title as the only required input.

Optional inputs: assignee (a family member), due date, and a responsibility domain reference for contextual grouping. A newly created task starts in **pending** state. Its origin is manual.

<!-- pdac:cite id="FR-TASKS-CREATE-TASK" digest="sha256:067e01cc2dad399cd448844d43cecae9ae0246159f697916ca69c501fa7cdd56" -->

<!-- pdac:cite id="SB-TASKS-CREATE-TITLE-ONLY" digest="sha256:bcbf32fd1f327efefaa0c56ffaed2c4f54081c811afc71b7d1505d19602d6acd" -->

<!-- pdac:cite id="SB-TASKS-CREATE-WITH-ASSIGNEE-DUE" digest="sha256:64d57651301e3967dc29a1dde6e10cdc0988ca5cd9432bf5dabbd096c9e8e386" -->

<!-- pdac:cite id="SB-TASKS-CREATE-NON-MEMBER-REJECTED" digest="sha256:d019f7ec0755b3ad54ce0daa687185573c376e5cee3972cdd880bda27790b635" -->

<!-- pdac:cite id="UC-TASKS-CREATE-TASK" digest="sha256:a4276bda0f802bca33a83ffac8e5b299941cb3dd119156f18c6e5a24f48b58cc" -->

<!-- pdac:cite id="BR-TASKS-TITLE-REQUIRED" digest="sha256:539e1e0e3f7b209b9de26048cfcef846fa5cb1531c6cbb843db54fd994e49b51" -->

<!-- pdac:cite id="BR-TASKS-ASSIGNEE-IN-HOUSEHOLD" digest="sha256:a9a887ffc284e8db810cf2fd7eee1d859c35371ac3f8c21082cb0eb09b031b29" -->

<!-- pdac:cite id="BR-TASKS-AREA-CATEGORISES-ONLY" digest="sha256:1c63c0149739660ca8a8626e6f2eb9260cab229cdb5bb667aebefb36c89e314e" -->

<!-- pdac:cite id="TERM-TASK" digest="sha256:56e9e73c7427f627b729be313364b0e56d485c7be6eb95ece3f5fdf44a3de676" -->

<!-- pdac:cite id="TERM-TASK-STATUS" digest="sha256:e6d6bb837ec2ff3349211a3c50fe0e0e2719d479234268576994dbc3c853755a" -->

<!-- pdac:cite id="TERM-TASK-ORIGIN" digest="sha256:2a51832806c326ec5b00e75c4ba9115903bb6a739274abbde305ff26d2f13cb0" -->

<!-- pdac:cite id="TERM-TASK-ASSIGNEE" digest="sha256:7ac4fd1f4d0d3e0bdf676d2e226d279f1c4f3f3317dd799200d3fb8304d9258d" -->

<!-- pdac-drift ids="FR-TASKS-TASK-DETAILS, TERM-TASK, UC-TASKS-CREATE-TASK" summary="Optional inputs omit the task description, colour and a due date with a time of day that the model makes current product (Q-0058)" -->

#### Scenario: Household creates a task with title only

- GIVEN a household exists
- WHEN a member creates a task with a title
- THEN a task is created in pending state
- AND no assignee or due date is required

#### Scenario: Household creates a task with an assignee and due date

- GIVEN a valid family member exists
- WHEN a task is created with a title, that member as assignee, and a due date
- THEN a task is created in pending state
- AND it is assigned to the specified member and scheduled for the given due date

#### Scenario: Creating a task with a non-member assignee is rejected

- GIVEN a member ID that does not belong to the household
- WHEN a task is created with that ID as assignee
- THEN the task is not created
- AND a validation error is returned

---

### Requirement: Task Assignment

A household SHALL be able to assign a task to a family member.

A task may have at most one primary assignee at a time. The assignee must be a valid member of the same family. Assignment may be changed after creation. Assignment is always an explicit action — it is never automatic, though it may be contextually motivated by responsibility ownership or calendar events.

<!-- pdac:cite id="FR-TASKS-ASSIGN-TASK" digest="sha256:1e20add7481e8ce3665dd5c74325c52edfe44581db6131d4e3a2e18cbd1bbd3f" -->

<!-- pdac:cite id="SB-TASKS-ASSIGN-MEMBER" digest="sha256:425ca4ed9a429b9a175bb91b68c0900c17268553a4f9b2de91b7b79a54e01f21" -->

<!-- pdac:cite id="SB-TASKS-ASSIGN-NON-MEMBER-REJECTED" digest="sha256:a6613fa721464e1f6d5625403045ddb4c5ecd52eb51df19623bb0bb84eedb4f0" -->

<!-- pdac:cite id="UC-TASKS-ASSIGN-TASK" digest="sha256:df5b78218f38095e36b6ce8f465db2f20b5f5564a8788a4dbbe59b4175dad514" -->

<!-- pdac:cite id="BR-TASKS-ASSIGNEE-IN-HOUSEHOLD" digest="sha256:a9a887ffc284e8db810cf2fd7eee1d859c35371ac3f8c21082cb0eb09b031b29" -->

<!-- pdac:cite id="BR-TASKS-EXPLICIT-ASSIGNMENT" digest="sha256:2191205476c2b2c2c39cf5e35c3e16445da5612bf38f64a8fc5cbc92b74fa541" -->

<!-- pdac:cite id="TERM-TASK-ASSIGNEE" digest="sha256:7ac4fd1f4d0d3e0bdf676d2e226d279f1c4f3f3317dd799200d3fb8304d9258d" -->

#### Scenario: Task is assigned to a member

- GIVEN a task exists
- AND a valid family member exists
- WHEN the task is assigned to that member
- THEN the task records the member as its assignee

#### Scenario: Assigning a task to a non-member is rejected

- GIVEN a task exists
- WHEN an assignment is made to a member ID that does not belong to the household
- THEN the assignment is rejected

---

### Requirement: Task Rescheduling

A household SHALL be able to update the due date of a task that is not yet completed or cancelled.

Rescheduling does not change the task's identity or assignment.

<!-- pdac:cite id="FR-TASKS-RESCHEDULE-TASK" digest="sha256:fac1dd4789a915678dad0cc3e00cc4ad23c4c586386daaa76eb3db93116bcb18" -->

<!-- pdac:cite id="SB-TASKS-RESCHEDULE-PENDING" digest="sha256:c39d0c045cbfc4538b88edae4bf4fb79789904da6a1dfe9b4840e8b65d45ccb8" -->

<!-- pdac:cite id="SB-TASKS-RESCHEDULE-COMPLETED-REJECTED" digest="sha256:1f151eb49fdccb72641b3debf883f5b7bfb1c48085c197f69ddfbe59dae86e79" -->

<!-- pdac:cite id="SB-TASKS-RESCHEDULE-CANCELLED-REJECTED" digest="sha256:0641f801074f6b8dca8663cad2694ad989e31d834d99f942fc2980253581180a" -->

<!-- pdac:cite id="UC-TASKS-RESCHEDULE-TASK" digest="sha256:916912c4f36df9f666f5514b9dc88f1ed121af7603368b01068c442c3752f63e" -->

<!-- pdac:cite id="BR-TASKS-CLOSED-TASK-FINAL" digest="sha256:e60f101770703f8596b1d95801c33166e909f67f4c762b909f84c0837e98b4e6" -->

#### Scenario: Due date of a pending task is updated

- GIVEN a task exists in pending state
- WHEN the household provides a new due date
- THEN the task due date is updated
- AND the task remains in its current state and assignment

#### Scenario: Completed task cannot be rescheduled

- GIVEN a task in completed state
- WHEN the household attempts to reschedule it
- THEN the operation is rejected

#### Scenario: Cancelled task cannot be rescheduled

- GIVEN a task in cancelled state
- WHEN the household attempts to reschedule it
- THEN the operation is rejected

---

### Requirement: Task Completion

A household SHALL be able to mark a task as completed.

Completed tasks cannot return to pending. Cancelled tasks cannot be completed. Completion may optionally record which member completed the task and at what time.

<!-- pdac:cite id="FR-TASKS-COMPLETE-TASK" digest="sha256:37265014449d4bc6e107f6c0a712fef40f79ca2b9ad05b0f1744d5efea99a9e7" -->

<!-- pdac:cite id="SB-TASKS-COMPLETE-TASK" digest="sha256:3ae707f9f316ff0ea29829ec4d8d1ca8eaddb830fe9e6741c1e8db599ac06dfa" -->

<!-- pdac:cite id="SB-TASKS-COMPLETE-TWICE-REJECTED" digest="sha256:52c4ea998b1fa7c9360ac72fa9c6dae7b7b45f2f8e7c688592e3f15476af31ee" -->

<!-- pdac:cite id="SB-TASKS-COMPLETE-CANCELLED-REJECTED" digest="sha256:c2d472fae4055737dd7c149f164d86b15ca3a13c65fb0ca0c828ffa272660c7e" -->

<!-- pdac:cite id="UC-TASKS-COMPLETE-TASK" digest="sha256:b7276dd93c2b33856d6050504e7bdb80aa1cc860598b4f8e2e013422b69f9810" -->

<!-- pdac:cite id="BR-TASKS-CLOSED-TASK-FINAL" digest="sha256:e60f101770703f8596b1d95801c33166e909f67f4c762b909f84c0837e98b4e6" -->

<!-- pdac:cite id="TERM-TASK-STATUS" digest="sha256:e6d6bb837ec2ff3349211a3c50fe0e0e2719d479234268576994dbc3c853755a" -->

<!-- pdac-drift ids="TERM-TASK-STATUS, BR-TASKS-CLOSED-TASK-FINAL, FR-TASKS-COMPLETE-TASK, SB-TASKS-COMPLETE-TASK" summary="Scenario Task is marked as completed allows completion from an in-progress state; the model has no in-progress state and only a pending task can be completed (Q-0005)" -->

#### Scenario: Task is marked as completed

- GIVEN a task exists in pending or in-progress state
- WHEN the household marks it as completed
- THEN the task status becomes completed

#### Scenario: Already completed task cannot be completed again

- GIVEN a task in completed state
- WHEN the household attempts to complete it again
- THEN the operation is rejected

#### Scenario: Cancelled task cannot be completed

- GIVEN a task in cancelled state
- WHEN the household attempts to complete it
- THEN the operation is rejected

---

### Requirement: Task Cancellation

A household SHALL be able to cancel a task that should no longer be executed.

Cancellation preserves the task in history but removes it from active work. Cancelled tasks cannot be completed or return to pending.

<!-- pdac:cite id="FR-TASKS-CANCEL-TASK" digest="sha256:5e9f0c5cf475c71536ee549c99d2c638354aa58c6f462e44350123df286b41ae" -->

<!-- pdac:cite id="SB-TASKS-CANCEL-TASK" digest="sha256:b303095cc87548586f3adf3ecf9a84d66899780ecd2a219b639e7ad65ad02a38" -->

<!-- pdac:cite id="SB-TASKS-CANCEL-COMPLETED-REJECTED" digest="sha256:39a82947223d2619dfe0843efad64280ebd9f24a6b60b6d9a30b54a7faca3111" -->

<!-- pdac:cite id="SB-TASKS-CANCEL-TWICE-REJECTED" digest="sha256:c57195479865c3c8011e543afed3ff920397c99af7a2092a9d31baf134d27fa0" -->

<!-- pdac:cite id="UC-TASKS-CANCEL-TASK" digest="sha256:d053307faf2e779cab18860261edc220f47e4b5b8f658610c5212b77a3c2d8f1" -->

<!-- pdac:cite id="BR-TASKS-CLOSED-TASK-FINAL" digest="sha256:e60f101770703f8596b1d95801c33166e909f67f4c762b909f84c0837e98b4e6" -->

<!-- pdac:cite id="TERM-TASK-STATUS" digest="sha256:e6d6bb837ec2ff3349211a3c50fe0e0e2719d479234268576994dbc3c853755a" -->

<!-- pdac-drift ids="TERM-TASK-STATUS, BR-TASKS-CLOSED-TASK-FINAL, FR-TASKS-CANCEL-TASK, SB-TASKS-CANCEL-TASK" summary="Scenario Task is cancelled allows cancelling from an in-progress state; the model has no in-progress state and only a pending task can be cancelled (Q-0005)" -->

#### Scenario: Task is cancelled

- GIVEN a task exists in pending or in-progress state
- WHEN the household cancels the task
- THEN the task status becomes cancelled
- AND it is no longer treated as active work

#### Scenario: Completed task cannot be cancelled

- GIVEN a task in completed state
- WHEN the household attempts to cancel it
- THEN the operation is rejected

#### Scenario: Already cancelled task cannot be cancelled again

- GIVEN a task in cancelled state
- WHEN the household attempts to cancel it again
- THEN the operation is rejected

---

### Requirement: Routine Creation

A household SHALL be able to define a recurring operational routine.

Required inputs: name, scope (Household or Members), kind (Scheduled or Cue), color, and a recurrence rule defined by frequency and day selectors.

Supported frequencies: Daily, Weekly, Monthly, Yearly.
- Weekly requires at least one day of week.
- Monthly requires at least one day of month.
- Yearly requires a month of year and at least one day of month.
- Member-scoped routines require at least one target member.

A newly created routine starts in **active** status. A Routine does not produce Task aggregates — it is projected on-the-fly into read surfaces on dates that match its recurrence rule. A routine may optionally specify an execution time; when provided, it may affect ordering in time-aware views.

<!-- pdac:cite id="FR-TASKS-CREATE-ROUTINE" digest="sha256:a3a40147d3e2cc80d9bc822f878d5ef81d91f9f3a3e77255921ec0cd42ce5ae6" -->

<!-- pdac:cite id="SB-TASKS-CREATE-WEEKLY-ROUTINE" digest="sha256:65a8de7739f2b91097d53b213361c6abbff255bd889a5635014f0428d5ff9125" -->

<!-- pdac:cite id="SB-TASKS-ROUTINE-INVALID-RECURRENCE-REJECTED" digest="sha256:1aed84198c977170f9713d462020534a7bf93731f9221a08201e0261990b5bce" -->

<!-- pdac:cite id="SB-TASKS-ROUTINE-PEOPLE-SCOPE-NEEDS-TARGETS" digest="sha256:36e2317bdc8e4bfe2037c5761ce3b7f245ff5b4eaef5e2ee2534e2f81b083aa5" -->

<!-- pdac:cite id="UC-TASKS-CREATE-ROUTINE" digest="sha256:bce1c21eeb9d127bc3eaf103c62553f15e7d61254c9c303cda4229fd82a89640" -->

<!-- pdac:cite id="BR-TASKS-VALID-ROUTINE-RECURRENCE" digest="sha256:eabd2d5380a5be02cac33e4ba02aea11f35c38c17d0dfd45640371458c04bec0" -->

<!-- pdac:cite id="BR-TASKS-PEOPLE-SCOPE-NEEDS-TARGETS" digest="sha256:30dc111977f0134237132b0f58f88b5228ac2149483649f25228438ba73f079a" -->

<!-- pdac:cite id="BR-TASKS-ROUTINES-PROJECTED" digest="sha256:81ea5e83e732a25b07646975f97691092b26fe58b06590a4e1c68e6310e65983" -->

<!-- pdac:cite id="TERM-ROUTINE" digest="sha256:b3f7531e4bd47453d7459e7c09af054bdfaeda3eb93b5df12a35d5b03fbf3a0a" -->

<!-- pdac:cite id="TERM-ROUTINE-RECURRENCE" digest="sha256:514741b0585fcf15adc95b595d49926629eb95888fd4f646081fd8fe04379d87" -->

<!-- pdac:cite id="TERM-ROUTINE-SCOPE" digest="sha256:11b499384d7b2303688eeae1346b8a8e24aea683853b3ef0652431c11a73bf4a" -->

<!-- pdac-drift ids="FR-TASKS-CREATE-ROUTINE, TERM-ROUTINE, UC-TASKS-CREATE-ROUTINE" summary="Spec requires a routine kind (Scheduled or Cue); the model decided routine kind is not part of the product (Q-0006)" -->

<!-- pdac-drift ids="FR-TASKS-ROUTINE-END-TIME-AREA, UC-TASKS-CREATE-ROUTINE, TERM-ROUTINE-RECURRENCE" summary="Spec omits the optional routine end time and Area that the model makes current product (Q-0058)" -->

#### Scenario: Household creates a weekly routine

- GIVEN a household exists
- WHEN a member creates a routine with a name, household scope, and a weekly frequency on specific days
- THEN a routine is created in active status
- AND it appears in agenda projections on matching days

#### Scenario: Routine with invalid recurrence is rejected

- GIVEN a weekly frequency is specified
- WHEN no days of week are provided
- THEN the routine is not created
- AND a validation error is returned

#### Scenario: Member-scoped routine requires target members

- GIVEN a member scope is specified
- WHEN no target members are provided
- THEN the routine is not created
- AND a validation error is returned

---

### Requirement: Routine Update

A household SHALL be able to update the definition of an existing routine.

Updatable fields include name, recurrence rule, color, scope, and target members. The routine's identity remains stable. Only future projections are affected; the routine's history is unchanged.

<!-- pdac:cite id="FR-TASKS-UPDATE-ROUTINE" digest="sha256:b565bc4a68a9f123ba2d2d9b6a282ced44d23b577d6be4a38d1cd3cc1b192535" -->

<!-- pdac:cite id="SB-TASKS-UPDATE-ROUTINE" digest="sha256:21edeb0c219f76d65f74dfa7228a1c6b75e5c4e70b091947c573a0f3fa0172ca" -->

<!-- pdac:cite id="UC-TASKS-UPDATE-ROUTINE" digest="sha256:98a3a56262d80553acf4ace63bae2760a64958a4e6198b0e8d4e3f702208e591" -->

<!-- pdac:cite id="TERM-ROUTINE" digest="sha256:b3f7531e4bd47453d7459e7c09af054bdfaeda3eb93b5df12a35d5b03fbf3a0a" -->

<!-- pdac-drift ids="FR-TASKS-ROUTINE-END-TIME-AREA, UC-TASKS-UPDATE-ROUTINE" summary="Updatable fields omit the routine end time and Area that the model lets a person change (Q-0058)" -->

#### Scenario: Routine definition is updated

- GIVEN an active routine exists
- WHEN the household provides updated fields
- THEN the routine definition is updated
- AND future agenda projections reflect the new definition

---

### Requirement: Routine Pause

A household SHALL be able to pause an active routine.

A paused routine stops appearing in agenda projections. The routine retains its identity and definition. A routine that is already paused cannot be paused again. A pause may optionally specify a date until which the routine is paused.

<!-- pdac:cite id="FR-TASKS-PAUSE-ROUTINE" digest="sha256:8059ca8e3c8a08d6d4627760012c7ee4083b609c6c45182d54c4ab5cb6fdef35" -->

<!-- pdac:cite id="SB-TASKS-PAUSE-ROUTINE" digest="sha256:635c3711c6b6efd2e4b0ae1586ff64b05404cd17877f9d30d27b1a51e66d3c54" -->

<!-- pdac:cite id="SB-TASKS-PAUSE-PAUSED-REJECTED" digest="sha256:d8abc9a465e274ab1fe94ca193410bb5d5e5e4620b62689aa7206570388139d3" -->

<!-- pdac:cite id="FR-TASKS-PAUSE-ROUTINE-UNTIL" digest="sha256:62f7787cf2a6fb3ef3cf1f00f1fd05ea435746ddf84f56bc9405215dcbd9fdc0" -->

<!-- pdac:cite id="UC-TASKS-PAUSE-ROUTINE" digest="sha256:474615dce1430aae101a7b2145525600ab09646917d7b7e8d22aadb1dd990ad0" -->

<!-- pdac:cite id="BR-TASKS-PAUSE-RESUME" digest="sha256:cb9742c0594f5caa828c8d828424d4bd6c132e2ab43fa923717aa5d390a9727f" -->

<!-- pdac:cite id="BR-TASKS-PAUSE-UNTIL-RESUMES" digest="sha256:3c90b3061e875911df751e0468e4447f1b141584662bd13d67d802165b3a17f4" -->

#### Scenario: Active routine is paused

- GIVEN a routine in active status
- WHEN the household pauses the routine
- THEN the routine status becomes paused
- AND it no longer appears in agenda projections

#### Scenario: Paused routine cannot be paused again

- GIVEN a routine in paused status
- WHEN the household attempts to pause it
- THEN the operation is rejected

---

### Requirement: Routine Resume

A household SHALL be able to resume a paused routine.

A resumed routine becomes active and begins appearing in agenda projections again. Occurrences missed during the paused period are not retroactively recreated. A routine that is not paused cannot be resumed.

<!-- pdac:cite id="FR-TASKS-RESUME-ROUTINE" digest="sha256:ca14a5d469152463f35cf906cc9e4e84cd6f987682c54d4710f5c30744e29052" -->

<!-- pdac:cite id="SB-TASKS-RESUME-ROUTINE" digest="sha256:d95bb1922cb988daa720d26b33eb358fe2c4f65ed633bdb570b923c386430a62" -->

<!-- pdac:cite id="SB-TASKS-RESUME-ACTIVE-REJECTED" digest="sha256:1d800781e760a2aa51115b8d6e6df0e4f552c2a2736b7275b21c4b474575bfba" -->

<!-- pdac:cite id="UC-TASKS-RESUME-ROUTINE" digest="sha256:a1aa36e575fe8521b1c35ed525e537d2e9f34476dbffc6752270b4c442da0ea6" -->

<!-- pdac:cite id="BR-TASKS-PAUSE-RESUME" digest="sha256:cb9742c0594f5caa828c8d828424d4bd6c132e2ab43fa923717aa5d390a9727f" -->

<!-- pdac:cite id="TERM-ROUTINE-OCCURRENCE" digest="sha256:640043ecfd480c0213595ca8413e85affd266d72bcbfc2c2545cbeec77b72be9" -->

#### Scenario: Paused routine is resumed

- GIVEN a routine in paused status
- WHEN the household resumes the routine
- THEN the routine status becomes active
- AND it begins appearing in agenda projections again
- AND no occurrences from the paused period are added retroactively

#### Scenario: Non-paused routine cannot be resumed

- GIVEN a routine in active status
- WHEN the household attempts to resume it
- THEN the operation is rejected

---

### Requirement: Agenda Projection

Tasks with a due date SHALL appear in the Agenda surface on their due date. Active routines SHALL appear in the Agenda surface on-the-fly on dates that match their recurrence rule.

Neither projection creates or modifies aggregates. Tasks and Routines remain owned by the Tasks context. Agenda is a read surface only.

<!-- pdac:cite id="FR-TASKS-AGENDA-PROJECTION" digest="sha256:4f3857f1502529d58be4f5b97a79cb9fe75d0820a289a0a4ea0e1483716ad71c" -->

<!-- pdac:cite id="SB-TASKS-AGENDA-SHOWS-DUE-TASK" digest="sha256:df5be20d603c04e9d6e88b7ce37b20301bae1f7aeda2bad1dd9203424f5d845b" -->

<!-- pdac:cite id="SB-TASKS-AGENDA-SHOWS-ROUTINE-OCCURRENCE" digest="sha256:8f906c3cd815100e77ad9e58b7644e040d6a56fb5a90e044e9d10374723ffe57" -->

<!-- pdac:cite id="SB-TASKS-AGENDA-HIDES-PAUSED-ROUTINE" digest="sha256:4ed39d227d297aa35758b20d17a7e98b67baaba8868678eb2ed1354a11df16c1" -->

<!-- pdac:cite id="UC-TASKS-SEE-WORK-IN-AGENDA" digest="sha256:f5569fbce071d60c16a1a0215d73ca800128b163b02db4723e93e7a3de549a89" -->

<!-- pdac:cite id="BR-TASKS-ROUTINES-PROJECTED" digest="sha256:81ea5e83e732a25b07646975f97691092b26fe58b06590a4e1c68e6310e65983" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="TERM-ROUTINE-OCCURRENCE" digest="sha256:640043ecfd480c0213595ca8413e85affd266d72bcbfc2c2545cbeec77b72be9" -->

#### Scenario: Task with due date appears in Agenda

- GIVEN a task exists with a due date
- WHEN the household views Agenda for that date
- THEN the task appears in the day view for that date

#### Scenario: Active routine appears on its scheduled dates

- GIVEN an active routine with a weekly recurrence on Tuesdays
- WHEN the household views Agenda for a Tuesday
- THEN the routine appears as a projected occurrence on that day

#### Scenario: Paused routine does not appear in Agenda

- GIVEN a routine in paused status
- WHEN the household views Agenda for a date matching its recurrence rule
- THEN the routine does not appear

---

## Notes

1. **Routine generation contradiction** — The feature specs for `update-routine`, `pause-routine`, and `resume-routine` reference "generated tasks" (e.g., "future generated tasks use the new routine configuration," "existing generated tasks remain unchanged"). This directly contradicts `docs/04_contexts/tasks.md`, which explicitly states routines do **not** generate Task aggregates and are projected on-the-fly only. This spec follows the context document as canonical. References to "generated tasks" in the feature specs should be treated as stale.

2. **Task in-progress state** — `docs/04_contexts/tasks.md` defines four task lifecycle states: pending, in progress, completed, cancelled. The `StartTask` command (pending → in progress) is listed in the domain command inventory but has no feature spec. The behavioral rule for initiating the in-progress transition is not currently documented.

<!-- pdac-drift ids="TERM-TASK-STATUS, BR-TASKS-CLOSED-TASK-FINAL" summary="Note lists in progress as a task state; the model decided the lifecycle is pending then completed or cancelled, with no in-progress state (Q-0005)" -->

3. **Unassignment** — `UnassignTask` is listed as a domain command but has no feature spec. The behavior of removing an assignee from a task is not specified.

<!-- pdac-drift ids="FR-TASKS-UNASSIGN-TASK, UC-TASKS-UNASSIGN-TASK" summary="Note leaves unassignment unspecified; the model makes removing a task's assignee current product (Q-0009)" -->

4. **Task Renaming** — `RenameTask` is listed as a domain command but has no feature spec.

<!-- pdac-drift ids="FR-TASKS-RENAME-TASK, UC-TASKS-RENAME-TASK" summary="Note leaves renaming unspecified; the model makes renaming a pending task current product (Q-0009)" -->

5. **Routine Deletion** — `DeleteRoutine` is listed as a domain command but has no feature spec.

<!-- pdac-drift ids="FR-TASKS-DELETE-ROUTINE, UC-TASKS-DELETE-ROUTINE" summary="Note leaves routine deletion unspecified; the model makes deleting a routine current product (Q-0009)" -->

6. **Routine kind (Scheduled | Cue)** — Routine creation requires a `Kind` field with values Scheduled or Cue. The behavioral distinction between these values is not described in any source document.

<!-- pdac-drift ids="FR-TASKS-CREATE-ROUTINE, TERM-ROUTINE, BC-TASKS" summary="Note treats routine kind as a required field; the model decided routine kind is not part of the product (Q-0006)" -->

7. **Responsibility domain reference** — A task may optionally reference a responsibility domain for contextual grouping in read models. The behavior when the referenced domain is archived or deleted is not specified.

<!-- pdac-drift ids="BR-TASKS-ARCHIVED-AREA-CLEARED, BR-TASKS-AREA-CATEGORISES-ONLY" summary="Note leaves the archived-Area case open; the model decided tasks and routines lose their Area reference when the Area is archived (Q-0010)" -->

8. **Timed pause auto-resume** — `pause-routine.md` introduces a `pausedUntil` optional date on Routine Pause. Whether reaching that date triggers automatic resumption or requires an explicit resume command is not specified in any source document.

<!-- pdac-drift ids="FR-TASKS-PAUSE-ROUTINE-UNTIL, BR-TASKS-PAUSE-UNTIL-RESUMES, SB-TASKS-PAUSE-UNTIL-AUTO-RESUME" summary="Note leaves the pause-until effect open; the model decided a routine paused until a date resumes automatically on that date (Q-0007)" -->

---

## Source References

- `docs/04_contexts/tasks.md` — primary context document: aggregate definitions, lifecycle invariants, projection model, domain events, boundary rules
- `specs/features/tasks/create-task.md`
- `specs/features/tasks/assign-task.md`
- `specs/features/tasks/reschedule-task.md`
- `specs/features/tasks/complete-task.md`
- `specs/features/tasks/cancel-task.md`
- `specs/features/tasks/create-routine.md`
- `specs/features/tasks/update-routine.md`
- `specs/features/tasks/pause-routine.md`
- `specs/features/tasks/resume-routine.md`
- `00_product/surfaces/agenda.md` — Agenda projection grammar, temporal entry model, item display grammar
- `docs/03_domain/ubiquitous-language.md` — canonical term definitions for Task, Routine, Agenda, Projection
