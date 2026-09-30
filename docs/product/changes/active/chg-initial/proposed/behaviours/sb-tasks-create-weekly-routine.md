---
id: SB-TASKS-CREATE-WEEKLY-ROUTINE
type: structured-behaviour
title: Weekly household routine created
status: draft
illustrates:
  - UC-TASKS-CREATE-ROUTINE
  - BR-TASKS-ROUTINES-PROJECTED
given:
  - "A household exists"
when: A person creates a routine with a name, household scope and a weekly frequency on specific days
then:
  - "A routine is created in active status"
  - "It appears in the Agenda on the matching days"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-RECURRENCE
  - TERM-ROUTINE-SCOPE
  - TERM-AGENDA
provenance:
  source: openspec/specs/tasks/spec.md (Routine Creation, scenario Household creates a weekly routine)
  confidence: high
  recovered-from: documentation
---

## Intent

A new routine is immediately visible on its days.

## Boundaries

Does not assert that any task is created; none is.
