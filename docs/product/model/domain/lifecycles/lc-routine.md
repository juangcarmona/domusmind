---
id: LC-ROUTINE
type: domain-lifecycle
title: Routine lifecycle
status: active
subject: TERM-ROUTINE
states:
  - id: ACTIVE
    title: Active
    initial: true
  - id: PAUSED
    title: Paused
  - id: REMOVED
    title: Removed
    terminal: true
transitions:
  - id: PAUSE
    title: Pause a routine
    from:
      - ACTIVE
    to: PAUSED
    trigger: A member pauses the routine
    initiated-by: &a1
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-TASKS-PAUSE-RESUME
    realized-by:
      - UC-TASKS-PAUSE-ROUTINE
  - id: RESUME
    title: Resume a routine
    from:
      - PAUSED
    to: ACTIVE
    trigger: A member resumes the routine
    initiated-by: *a1
    governed-by:
      - BR-TASKS-PAUSE-RESUME
    realized-by:
      - UC-TASKS-RESUME-ROUTINE
  - id: RESUME-ON-DATE
    title: Resume at the end of a dated pause
    from:
      - PAUSED
    to: ACTIVE
    trigger: The date specified for the end of the pause arrives
    governed-by:
      - BR-TASKS-PAUSE-UNTIL-RESUMES
    realized-by:
      - UC-TASKS-PAUSE-ROUTINE
  - id: DELETE
    title: Delete a routine
    from:
      - ACTIVE
      - PAUSED
    to: REMOVED
    trigger: A member deletes the routine
    initiated-by: *a1
    realized-by:
      - UC-TASKS-DELETE-ROUTINE
provenance:
  source: TERM-ROUTINE; BR-TASKS-PAUSE-RESUME; BR-TASKS-PAUSE-UNTIL-RESUMES; UC-TASKS-DELETE-ROUTINE; product owner decisions Q-0007 and Q-0009; src/backend/DomusMind.Domain/Tasks/Routine.cs
  confidence: medium
  recovered-from: documentation
---

## Purpose

Show when a recurring definition contributes occurrences to the Agenda and when the household has stopped using it.

## Invariants

A Routine keeps its own identity and recurrence. It projects occurrences without creating Tasks. Resuming never recreates occurrences missed during a pause.

## State Semantics

Active routines appear on matching dates. Paused routines keep their definition and are absent from the Agenda. Removed means the routine no longer exists; it is a product outcome, not a stored status.

## Transition Semantics

A member can pause, resume or delete a routine. A dated pause ends automatically on its specified date under the accepted Q-0007 decision. RoutinePaused, RoutineResumed and RoutineDeleted record implemented domain changes.

## Boundaries

Automatic resumption is accepted intent; the current Routine domain class has no dated-pause mechanism. The lifecycle does not claim that this mechanism has been delivered. Deletion removes a definition rather than completing household work.
