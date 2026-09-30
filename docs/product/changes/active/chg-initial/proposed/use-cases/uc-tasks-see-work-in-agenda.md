---
id: UC-TASKS-SEE-WORK-IN-AGENDA
type: use-case
title: See tasks and routines in the Agenda
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-ROUTINES-PROJECTED
  - BR-TASKS-PAUSE-RESUME
uses-terms:
  - TERM-TASK
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
  - TERM-AGENDA
provenance:
  source: openspec/specs/tasks/spec.md (Agenda Projection); docs/_legacy/04_contexts/tasks.md (Routine Projection Model); src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs (OccursOn)
  confidence: high
  recovered-from: documentation
---

## Goal

See the household's due tasks and today's routines alongside plans, in one place (observed: openspec/specs/tasks/spec.md, Agenda Projection).

## Trigger

A person opens the Agenda for a date or range.

## Preconditions

Tasks or routines exist in the household.

## Main Flow

1. The person views the Agenda for a date.
2. The product shows each task due on that date and each active routine whose recurrence matches that date, as a projected occurrence.

## Alternative Flows

- A routine scoped to specific people appears for those people; a household routine applies to everyone (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs, AppliesTo). How these rows are laid out is defined with the Agenda (TERM-AGENDA).

## Failure Conditions

- Paused routines are not shown (observed: scenario Paused routine does not appear in Agenda).

## Postconditions

Nothing is created or changed by viewing; tasks and routines stay owned by Tasks.
