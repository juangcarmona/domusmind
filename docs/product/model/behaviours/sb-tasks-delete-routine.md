---
id: SB-TASKS-DELETE-ROUTINE
type: structured-behaviour
title: Deleting a routine removes its future occurrences
status: active
illustrates:
  - UC-TASKS-DELETE-ROUTINE
given:
  - An active routine exists.
when: A household member deletes the routine.
then:
  - The routine no longer exists.
  - It no longer appears in the Agenda.
covers-transition:
  lifecycle: LC-ROUTINE
  transition: DELETE
uses-terms:
  - TERM-ROUTINE
provenance:
  source: UC-TASKS-DELETE-ROUTINE; FR-TASKS-DELETE-ROUTINE; product owner decision Q-0009; src/backend/DomusMind.Application/Features/Tasks/DeleteRoutine/DeleteRoutineCommandHandler.cs
  confidence: high
  recovered-from: documentation
---

## Intent

Let the household stop recurring work that no longer applies.

## Boundaries

The example describes only this action and its stated result.
