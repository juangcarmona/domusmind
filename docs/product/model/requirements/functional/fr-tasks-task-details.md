---
id: FR-TASKS-TASK-DETAILS
type: functional-requirement
title: Give a task a description, a colour and a timed due date
status: draft
derived-from:
  - UC-TASKS-CREATE-TASK
verification:
  - scenario: "A person creates a task with a description and a colour, and the task shows both"
  - scenario: "A person creates a task due on a date at a time, and the task appears in the Agenda on that date with that time"
uses-terms:
  - TERM-TASK
  - TERM-AGENDA
provenance:
  source: "src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskSchedule.cs; interview: product owner decision Q-0058 (E-0158)"
  confidence: medium
  recovered-from: observation
---

## Requirement

The product MUST let a person give a task an optional description and an optional colour, and a due date that is either a date only or a date and a time to the minute (decided: Q-0058; observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskSchedule.cs).

## Rationale

Some work needs more than a title to be understood, a colour to be recognised, or a specific time to be done at.
