---
id: UC-TASKS-UPDATE-ROUTINE
type: use-case
title: Update a routine
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by:
  - BR-TASKS-VALID-ROUTINE-RECURRENCE
  - BR-TASKS-PEOPLE-SCOPE-NEEDS-TARGETS
  - BR-TASKS-ROUTINES-PROJECTED
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-RECURRENCE
  - TERM-ROUTINE-SCOPE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: "openspec/specs/tasks/spec.md (Routine Update); src/backend/DomusMind.Domain/Tasks/Routine.cs; interview: product owner decision Q-0006 (E-0158); interview: product owner decision Q-0058 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Change how a routine repeats or who it is for, going forward (observed: openspec/specs/tasks/spec.md, Routine Update).

## Trigger

The household's routine changes.

## Preconditions

The routine exists.

## Main Flow

1. The person changes the routine's name, recurrence, colour, scope or target people (and its end time and Area, decided: Q-0058; observed: src/backend/DomusMind.Domain/Tasks/Routine.cs, Update). Routine kind is not part of the product (decided: Q-0006).
2. The product updates the definition; the routine keeps its identity.

## Alternative Flows

None stated.

## Failure Conditions

- The new recurrence is invalid, or people scope has no target people: the update is rejected (observed: same rules as creation; code enforces scope on update).

## Postconditions

Future Agenda projections reflect the new definition; the routine's history is unchanged (observed: openspec/specs/tasks/spec.md, Routine Update).
