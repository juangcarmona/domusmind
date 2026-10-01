# Tasks Specification

<!-- pdac-scope: cited -->

## Purpose

Tasks is the household's structured execution layer. It answers: what explicit work needs to be done, who is responsible for doing it, when it should happen, and whether it has been completed.

A **Task** is a concrete, actionable unit of household work. It has an assignee, a due date, and a defined lifecycle. Tasks are created explicitly — they are never automatically generated from Routines or Calendar events.

A **Routine** is a recurring operational definition. It describes household work that repeats on a schedule. A Routine does not produce Task aggregates; it is projected on-the-fly into read surfaces on dates that match its recurrence rule.

Tasks and Routines belong to the Tasks context. A list item that carries a due date is not a Task. A recurring calendar plan is not a Routine. These distinctions are firm.

<!-- pdac:cite id="BC-TASKS" digest="sha256:7a1a64e2267e7795b9766611ab9d65af92e80600a128617a728d4bff899a032d" -->

<!-- pdac:cite id="TERM-TASK" digest="sha256:b6587c9dbd20793090462b177eaa864cd57ab218ef97ff0d931f2f74cfc92f88" -->

<!-- pdac:cite id="TERM-ROUTINE" digest="sha256:f1573ee259ba5b32b606acab9b4df1b9a987d70e2c0789fac00fca446ab642e3" -->

<!-- pdac:cite id="BR-TASKS-TASKS-CREATED-EXPLICITLY" digest="sha256:2f9e4d81eb0cf7d8a7d20c20d9bbfdbeb630dd5f3a7c01ca4a451b2b329c3c8c" -->

<!-- pdac:cite id="BR-TASKS-ROUTINES-PROJECTED" digest="sha256:bf09635a2c49f6ac75543376e471b450a9ff0c436c767ebe2b44db22fcb31b21" -->

<!-- pdac:cite id="BR-TASKS-DISTINCT-EXECUTION-MODELS" digest="sha256:3ea50bfb2dc6ebc1dbb16766f281033693901a7aa78d75fccb9d390c439f7c68" -->

---

## Requirements

### Requirement: Task Creation

A household SHALL be able to create a task with a title as the only required input.

Optional inputs: assignee (a family member), due date, and a responsibility domain reference for contextual grouping. A newly created task starts in **pending** state. Its origin is manual.

<!-- pdac:cite id="FR-TASKS-CREATE-TASK" digest="sha256:d1c1e9c40cb2e7e3741da0d29549200b5f9de486fcb5fc41d400110f28f3e605" -->

<!-- pdac:cite id="SB-TASKS-CREATE-TITLE-ONLY" digest="sha256:7e4869bc6af6db98387a219749b77c77ec6f20ad04b15ca7f9a572bb7d12884e" -->

<!-- pdac:cite id="SB-TASKS-CREATE-WITH-ASSIGNEE-DUE" digest="sha256:bd99b3433bf2452807df67f4a06153f9210e7f4b89a8622e306791d57cce7a38" -->

<!-- pdac:cite id="SB-TASKS-CREATE-NON-MEMBER-REJECTED" digest="sha256:4a802384350e6d1fe85433debacc75774d9d364334deb725c7cbe2fa3c15d79f" -->

<!-- pdac:cite id="UC-TASKS-CREATE-TASK" digest="sha256:3888024aa4c2b69830c1fcdef332c6a182c6bd9a313220909b1ec46962c6c8ac" -->

<!-- pdac:cite id="BR-TASKS-TITLE-REQUIRED" digest="sha256:9f255fd9d139f7a36d5d451032358e6a96e8bafbe1bb76cd732cc822260933a4" -->

<!-- pdac:cite id="BR-TASKS-ASSIGNEE-IN-HOUSEHOLD" digest="sha256:aba53ed6838c6df084ece3c7e2a83b7071ba4e224e5fc1584e97111a023d9ad4" -->

<!-- pdac:cite id="BR-TASKS-AREA-CATEGORISES-ONLY" digest="sha256:9c2fb9c31feb96ab6309a5e768f3bfd87b7d9f028fe5a27e42accaf363a1b916" -->

<!-- pdac:cite id="TERM-TASK" digest="sha256:b6587c9dbd20793090462b177eaa864cd57ab218ef97ff0d931f2f74cfc92f88" -->

<!-- pdac:cite id="TERM-TASK-STATUS" digest="sha256:1bc36ed2dfc13c206af3dceca0bed95fe4ad1a5e45b81cce293bcf05c5725b25" -->

<!-- pdac:cite id="TERM-TASK-ORIGIN" digest="sha256:0bdcc486bb713f9a422f8bbb757c00f0a8444ad703940c3cbebbac7e39479fcb" -->

<!-- pdac:cite id="TERM-TASK-ASSIGNEE" digest="sha256:001301ae562545bf5ee618ddccb3cf81e0c782a0c84c89c09fe7d39ace6f4dd5" -->

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

<!-- pdac:cite id="FR-TASKS-ASSIGN-TASK" digest="sha256:35a6e863464f3f82694e90a51fd59f0211133d6a31dd220c0d2f0c564a419739" -->

<!-- pdac:cite id="SB-TASKS-ASSIGN-MEMBER" digest="sha256:6e6f1bc636c77c3200802e91382473089b170a19251a409898da78e9d1b4557f" -->

<!-- pdac:cite id="SB-TASKS-ASSIGN-NON-MEMBER-REJECTED" digest="sha256:c9db5e9c64eff443c19cf1ca925d226bebf61ff07161d971935e51dbeded817b" -->

<!-- pdac:cite id="UC-TASKS-ASSIGN-TASK" digest="sha256:c872d9cbc5327a8ea08c76daeccb1d4461d21155d242ed2b8eb4de32759a1df8" -->

<!-- pdac:cite id="BR-TASKS-ASSIGNEE-IN-HOUSEHOLD" digest="sha256:aba53ed6838c6df084ece3c7e2a83b7071ba4e224e5fc1584e97111a023d9ad4" -->

<!-- pdac:cite id="BR-TASKS-EXPLICIT-ASSIGNMENT" digest="sha256:6f864c047ce46b9eefc49753f0e3cffde471f8db1d2ae35333fbd9edc81ee298" -->

<!-- pdac:cite id="TERM-TASK-ASSIGNEE" digest="sha256:001301ae562545bf5ee618ddccb3cf81e0c782a0c84c89c09fe7d39ace6f4dd5" -->

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

<!-- pdac:cite id="FR-TASKS-RESCHEDULE-TASK" digest="sha256:368e20bbad49935ba41dc496265695f8a3815212f3c772463233df4d884b7c77" -->

<!-- pdac:cite id="SB-TASKS-RESCHEDULE-PENDING" digest="sha256:788716387570af927bb936bc56d7be5861b202aa63d1e5ab8b1b5493ba498ded" -->

<!-- pdac:cite id="SB-TASKS-RESCHEDULE-COMPLETED-REJECTED" digest="sha256:0766897dabdfd6be9c3958f1279d61809ce0b6df282529011b3097a7a0dadcee" -->

<!-- pdac:cite id="SB-TASKS-RESCHEDULE-CANCELLED-REJECTED" digest="sha256:8196ee28df6fd5b8dc6e5b21717fa3200e4c71a1f9c3488b465ef940770f9276" -->

<!-- pdac:cite id="UC-TASKS-RESCHEDULE-TASK" digest="sha256:c658d7a3b6a64f5cfa3ac14ad7020570b8f8af5b55178066ce56328acd7650eb" -->

<!-- pdac:cite id="BR-TASKS-CLOSED-TASK-FINAL" digest="sha256:4bac6b1a1b6ec5304266d803673f69b51f49a426a8d098e8fb29c3a1aa636f2f" -->

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

<!-- pdac:cite id="FR-TASKS-COMPLETE-TASK" digest="sha256:db49a03be05e70704f34eb7e1dba5a6b799ee67744c77e994c851f87495a4667" -->

<!-- pdac:cite id="SB-TASKS-COMPLETE-TASK" digest="sha256:8d32aa80f0fe32f720ffb1e1ebbfe8cb6f4e8e815462e64b84183d0e2a0b0de5" -->

<!-- pdac:cite id="SB-TASKS-COMPLETE-TWICE-REJECTED" digest="sha256:7499a0c2f4640c5c23d671007cfc7a4de902c2742ea40770dbadcd49db4856e1" -->

<!-- pdac:cite id="SB-TASKS-COMPLETE-CANCELLED-REJECTED" digest="sha256:dcabfa6f1adf25f9a1b4b3c1c5363f9e81c208e9a419d7b0245771f664b6e170" -->

<!-- pdac:cite id="UC-TASKS-COMPLETE-TASK" digest="sha256:cd82a6613d9e5e1b8f8c458ec8afbfb333232453d85e8e6d12e9b4d88c2f3754" -->

<!-- pdac:cite id="BR-TASKS-CLOSED-TASK-FINAL" digest="sha256:4bac6b1a1b6ec5304266d803673f69b51f49a426a8d098e8fb29c3a1aa636f2f" -->

<!-- pdac:cite id="TERM-TASK-STATUS" digest="sha256:1bc36ed2dfc13c206af3dceca0bed95fe4ad1a5e45b81cce293bcf05c5725b25" -->

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

<!-- pdac:cite id="FR-TASKS-CANCEL-TASK" digest="sha256:91eb534e402f98bddc9a63f67d0ecc50ab493a635e31f6af5ea03167a515fc45" -->

<!-- pdac:cite id="SB-TASKS-CANCEL-TASK" digest="sha256:ba4bcbe70bf0b9f5bcfc1761c9a10fe01bb808016102a8162b9fb830f405e55b" -->

<!-- pdac:cite id="SB-TASKS-CANCEL-COMPLETED-REJECTED" digest="sha256:9265cf4377c79a440ae565ecf1ba51d4a8d302ad4bc35fdc73738156d5f186e0" -->

<!-- pdac:cite id="SB-TASKS-CANCEL-TWICE-REJECTED" digest="sha256:ca2ecb6fe8c5433c5f6523281f2ddb65685f019f6710b973abd363e0b9c45432" -->

<!-- pdac:cite id="UC-TASKS-CANCEL-TASK" digest="sha256:e58d5c37b2f8e9234673fecf7679eb1c1c00497402dcb0840d61df14bd4e98b7" -->

<!-- pdac:cite id="BR-TASKS-CLOSED-TASK-FINAL" digest="sha256:4bac6b1a1b6ec5304266d803673f69b51f49a426a8d098e8fb29c3a1aa636f2f" -->

<!-- pdac:cite id="TERM-TASK-STATUS" digest="sha256:1bc36ed2dfc13c206af3dceca0bed95fe4ad1a5e45b81cce293bcf05c5725b25" -->

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

<!-- pdac:cite id="FR-TASKS-CREATE-ROUTINE" digest="sha256:b9390e584d6706d601fc674e6b000f9c94da1966898fe7152f965d7cff3a2eb6" -->

<!-- pdac:cite id="SB-TASKS-CREATE-WEEKLY-ROUTINE" digest="sha256:eff8dacf1e602666de42c7dbc7a0a4c1fce193a91eefebdeb72820d67e614519" -->

<!-- pdac:cite id="SB-TASKS-ROUTINE-INVALID-RECURRENCE-REJECTED" digest="sha256:486d5d40cfea21bec0c696474a36cf2441456e51722689daced476c5bd99efd1" -->

<!-- pdac:cite id="SB-TASKS-ROUTINE-PEOPLE-SCOPE-NEEDS-TARGETS" digest="sha256:cd675efd0e911298900061287f5ef53c77128f270c243b3f0e5edc0a6adc89e8" -->

<!-- pdac:cite id="UC-TASKS-CREATE-ROUTINE" digest="sha256:0a115c53fc106cd5b0f77757efccc9df1376e7b76c7677a9f42a047a9fc9337d" -->

<!-- pdac:cite id="BR-TASKS-VALID-ROUTINE-RECURRENCE" digest="sha256:e93f7bceee66970541848b07b793c377aa5049fbf6320e55e76b82e64f492293" -->

<!-- pdac:cite id="BR-TASKS-PEOPLE-SCOPE-NEEDS-TARGETS" digest="sha256:e0bde31bdfdd057c7ae27171e774ad946cd075a313cbc18693f706ef32824a69" -->

<!-- pdac:cite id="BR-TASKS-ROUTINES-PROJECTED" digest="sha256:bf09635a2c49f6ac75543376e471b450a9ff0c436c767ebe2b44db22fcb31b21" -->

<!-- pdac:cite id="TERM-ROUTINE" digest="sha256:f1573ee259ba5b32b606acab9b4df1b9a987d70e2c0789fac00fca446ab642e3" -->

<!-- pdac:cite id="TERM-ROUTINE-RECURRENCE" digest="sha256:52f074eba287bb1cbcc4cbea700c1647a1a61a7fc174c304679c9974bafe26ff" -->

<!-- pdac:cite id="TERM-ROUTINE-SCOPE" digest="sha256:8621bbaba01ef03590a036c03fd6723ac6ff4adedfc94b4b8bc138248377371e" -->

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

<!-- pdac:cite id="FR-TASKS-UPDATE-ROUTINE" digest="sha256:205e4b6973d5e3fe36362f042265eb8113945c4af643b350865f634746d0525f" -->

<!-- pdac:cite id="SB-TASKS-UPDATE-ROUTINE" digest="sha256:dfbba5840850f84040a61ab551eddc37d62dab7115f56917dfbc79d9f0ddbfe4" -->

<!-- pdac:cite id="UC-TASKS-UPDATE-ROUTINE" digest="sha256:73b974209b21239fdbefc70640c928004d478bf518f690759396097dd98ba6c9" -->

<!-- pdac:cite id="TERM-ROUTINE" digest="sha256:f1573ee259ba5b32b606acab9b4df1b9a987d70e2c0789fac00fca446ab642e3" -->

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

<!-- pdac:cite id="FR-TASKS-PAUSE-ROUTINE" digest="sha256:94897ba144c7f0992ec2607a11223f38a52814e7b6db5fb1bb62d5036bbbda29" -->

<!-- pdac:cite id="SB-TASKS-PAUSE-ROUTINE" digest="sha256:8e9b613f2dfab2aa71458929bfd44f9ac6e573f909e793527b1b915b52470e08" -->

<!-- pdac:cite id="SB-TASKS-PAUSE-PAUSED-REJECTED" digest="sha256:fd1e6f19fbc33652034d66dfe2c4da37533052b5236c1c793df12c238ceb256d" -->

<!-- pdac:cite id="FR-TASKS-PAUSE-ROUTINE-UNTIL" digest="sha256:233bc2edc2f9ee830873b48909f42057bb26f4675290ed54409c0c447986b80b" -->

<!-- pdac:cite id="UC-TASKS-PAUSE-ROUTINE" digest="sha256:793f394ad7cf2a748a62e2d0b5477ce1e24efeca1dc8fada8930a9a2ca167caf" -->

<!-- pdac:cite id="BR-TASKS-PAUSE-RESUME" digest="sha256:02b51181c6067faf1ae9bcbc5264d3285867b9b4115e65f5f604f354b65c661e" -->

<!-- pdac:cite id="BR-TASKS-PAUSE-UNTIL-RESUMES" digest="sha256:3ea220144f10939c624ec8223f3cbac929f69ccfd3ddfa2668424cb3608d69b0" -->

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

<!-- pdac:cite id="FR-TASKS-RESUME-ROUTINE" digest="sha256:affe70380e2ad44e2e5dc875b5f1e58290bf7104680a569140d10498e3340fa3" -->

<!-- pdac:cite id="SB-TASKS-RESUME-ROUTINE" digest="sha256:10f06adf0e49cbc80cc36cf659206023e9add7ad7f6ccad801479cc494161624" -->

<!-- pdac:cite id="SB-TASKS-RESUME-ACTIVE-REJECTED" digest="sha256:e634fb0efccba96a6fb384a0bc3a42e658c39c10b3f3685cb3defdb45a1a633c" -->

<!-- pdac:cite id="UC-TASKS-RESUME-ROUTINE" digest="sha256:b3e8cede2307df1161af5f6473b172d689793aac166d0b266d8de7c944cdfd70" -->

<!-- pdac:cite id="BR-TASKS-PAUSE-RESUME" digest="sha256:02b51181c6067faf1ae9bcbc5264d3285867b9b4115e65f5f604f354b65c661e" -->

<!-- pdac:cite id="TERM-ROUTINE-OCCURRENCE" digest="sha256:b3d668c4762b7cffc6b87c3f84b7cf97b2555263c888b8cbc529d2f9456938e1" -->

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

<!-- pdac:cite id="FR-TASKS-AGENDA-PROJECTION" digest="sha256:ff3d160ef63352bd32051e37528634126e7207898285e4bc28d67ed058cc4c99" -->

<!-- pdac:cite id="SB-TASKS-AGENDA-SHOWS-DUE-TASK" digest="sha256:b508bc8ea4792f3d9e1a98e278b5c101b78565ff466900960bf45153ae0596e2" -->

<!-- pdac:cite id="SB-TASKS-AGENDA-SHOWS-ROUTINE-OCCURRENCE" digest="sha256:f2f38984b71f91e734e9ace77a3d74f022632aeb400794b82b265531066bd009" -->

<!-- pdac:cite id="SB-TASKS-AGENDA-HIDES-PAUSED-ROUTINE" digest="sha256:b78b8597f7b84e10edb8e8d9277899ee3b3477dc84f3ead41e272275525f9848" -->

<!-- pdac:cite id="UC-TASKS-SEE-WORK-IN-AGENDA" digest="sha256:3d35eb57fd6b4b2242d6a5a67c3ff9f90e5409985e924c87b79568e9a190f898" -->

<!-- pdac:cite id="BR-TASKS-ROUTINES-PROJECTED" digest="sha256:bf09635a2c49f6ac75543376e471b450a9ff0c436c767ebe2b44db22fcb31b21" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="TERM-ROUTINE-OCCURRENCE" digest="sha256:b3d668c4762b7cffc6b87c3f84b7cf97b2555263c888b8cbc529d2f9456938e1" -->

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
