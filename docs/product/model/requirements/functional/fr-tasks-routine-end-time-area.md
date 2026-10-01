---
id: FR-TASKS-ROUTINE-END-TIME-AREA
type: functional-requirement
title: Give a routine an end time and an Area
status: draft
derived-from:
  - UC-TASKS-CREATE-ROUTINE
  - UC-TASKS-UPDATE-ROUTINE
  - BR-TASKS-VALID-ROUTINE-RECURRENCE
verification:
  - scenario: "A person creates a routine from 18:00 to 19:00 in the Pets Area, and the routine shows that time span and Area"
  - scenario: "A routine with an end time but no time of day is rejected"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-RECURRENCE
  - TERM-AREA
provenance:
  source: "src/backend/DomusMind.Domain/Tasks/Routine.cs; src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs; interview: product owner decision Q-0058 (E-0158)"
  confidence: medium
  recovered-from: observation
---

## Requirement

The product MUST let a person give a routine, when creating or updating it, an optional end time and an optional Area for grouping (decided: Q-0058). An end time MUST come with a time of day and be later than it (observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs).

## Rationale

Recurring work often spans a known slot of the day and belongs to an Area of household life.
