---
id: UC-TASKS-RESUME-ROUTINE
type: use-case
title: Resume a routine
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
  source: "openspec/specs/tasks/spec.md (Routine Resume); src/backend/DomusMind.Domain/Tasks/Routine.cs; interview: product owner decision Q-0007 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Bring a paused routine back into the household's view (observed: openspec/specs/tasks/spec.md, Routine Resume).

## Trigger

The routine applies again.

## Preconditions

The routine is paused.

## Main Flow

1. The person resumes the routine.
2. The product marks the routine as active.

## Alternative Flows

- Automatic resume: a routine paused until a date resumes by itself on that date, without a person acting (decided: Q-0007).

## Failure Conditions

- The routine is not paused: the resume is rejected (observed: openspec/specs/tasks/spec.md, scenario Non-paused routine cannot be resumed).

## Postconditions

The routine appears again in the Agenda on matching dates; occurrences from the paused period are not added retroactively.
