---
id: UC-TASKS-REVIEW-HOUSEHOLD-WORK
type: use-case
title: Review household work
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-TASKS
governed-by: []
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
  - TERM-TASK-ASSIGNEE
  - TERM-ROUTINE
  - TERM-AREA
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Read Models; Queries); interview: product owner decisions Q-0005, Q-0052 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Goal

There is no dedicated Tasks surface: the household reviews its work in the Agenda and in Areas (decided: Q-0052).

Understand the state of household work: what is open, who has what, and which routines keep the home running (observed: docs/_legacy/04_contexts/tasks.md, Read Models).

## Trigger

A person wants an overview beyond one day.

## Preconditions

The household exists.

## Main Flow

1. The person opens a task overview.
2. The product shows tasks by status (pending, completed; no in-progress state, decided: Q-0005), grouped by assignee, due date or area (observed: docs/_legacy/04_contexts/tasks.md, Read Models, Task Board).

## Alternative Flows

- Personal list: only the tasks assigned to one person, with title, due date, status and origin (observed: docs/_legacy/04_contexts/tasks.md, Personal Task List).
- Household overview: all open tasks grouped by area (observed: docs/_legacy/04_contexts/tasks.md, Household Task Overview).
- Routine overview: active routines with their next occurrence and target people (observed: docs/_legacy/04_contexts/tasks.md, Routine Overview).

## Failure Conditions

None stated.

## Postconditions

The person knows what work is open and who holds it; nothing is changed.
