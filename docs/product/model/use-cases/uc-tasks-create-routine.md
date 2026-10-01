---
id: UC-TASKS-CREATE-ROUTINE
type: use-case
title: Create a routine
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
  source: "openspec/specs/tasks/spec.md (Routine Creation); docs/_legacy/04_contexts/tasks.md (Routine, Routine Integrity); src/backend/DomusMind.Domain/Tasks/Routine.cs; interview: product owner decision Q-0006 (E-0158); interview: product owner decision Q-0058 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Define recurring household work once so it shows up where it matters on the right days (observed: openspec/specs/tasks/spec.md, Routine Creation).

## Trigger

A person identifies work that repeats, such as weekly trash or daily pet feeding.

## Preconditions

The household exists and the person belongs to it.

## Main Flow

1. The person gives the routine a name, a scope (the household or specific people), a colour, and a recurrence (frequency and days).
2. Optionally, the person sets a time of day, an end time after it and an Area (decided: Q-0058; observed: openspec/specs/tasks/spec.md, Routine Creation; src/backend/DomusMind.Domain/Tasks/Routine.cs).
3. The product creates the routine as active.

## Alternative Flows

- People scope: the person also chooses at least one target person (observed: openspec/specs/tasks/spec.md, Routine Creation).

## Failure Conditions

- The recurrence is invalid, e.g. weekly with no days: the routine is not created and a validation error is shown (observed: openspec/specs/tasks/spec.md, scenario Routine with invalid recurrence is rejected).
- People scope with no target people: the routine is not created (observed: openspec/specs/tasks/spec.md, scenario Member-scoped routine requires target members).

## Postconditions

An active routine exists and appears in the Agenda on matching dates; no tasks are created (observed: openspec/specs/tasks/spec.md, scenario Household creates a weekly routine).
