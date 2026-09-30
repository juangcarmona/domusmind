---
id: UC-TASKS-PAUSE-ROUTINE
type: use-case
title: Pause a routine
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-PAUSE-RESUME
  - BR-TASKS-PAUSE-UNTIL-RESUMES
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: "openspec/specs/tasks/spec.md (Routine Pause, Notes 8); src/backend/DomusMind.Domain/Tasks/Routine.cs; interview: product owner decision Q-0007 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Temporarily stop a routine from showing up, without losing it (observed: openspec/specs/tasks/spec.md, Routine Pause).

## Trigger

The routine does not apply for a while, e.g. during holidays (inferred example).

## Preconditions

The routine is active.

## Main Flow

1. The person pauses the routine, optionally giving a date until which it is paused.
2. The product marks the routine as paused.

## Alternative Flows

- Pause until a date: the pause names an end date, and the routine resumes automatically on that date (decided: Q-0007). The current model has no pause-until date yet (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs).

## Failure Conditions

- The routine is already paused: the pause is rejected (observed: openspec/specs/tasks/spec.md, scenario Paused routine cannot be paused again).

## Postconditions

The routine keeps its identity and definition and no longer appears in the Agenda.
