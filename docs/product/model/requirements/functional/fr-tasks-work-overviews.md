---
id: FR-TASKS-WORK-OVERVIEWS
type: functional-requirement
title: Provide task and routine overviews
status: draft
derived-from:
  - UC-TASKS-REVIEW-HOUSEHOLD-WORK
verification:
  - scenario: "A person opens their personal task list and sees only the tasks assigned to them with title, due date, status and origin"
  - scenario: "A person opens the household overview and sees all open tasks grouped by area"
  - scenario: "A person opens the routine overview and sees active routines with their next occurrence and target people"
uses-terms:
  - TERM-TASK
  - TERM-ROUTINE
  - TERM-AREA
  - TERM-TASK-ASSIGNEE
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Read Models; Queries); interview: product owner decision Q-0005 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST offer: a task board of pending and completed tasks groupable by assignee, due date or area; a personal task list per person; a household overview of all open tasks grouped by area; and a routine overview of active routines with their next occurrence and target people (observed: docs/_legacy/04_contexts/tasks.md, Read Models; Queries: tasks by household, by assignee, due today, pending; active routines). The legacy board also has an in-progress column; the lifecycle has no in-progress state (decided: Q-0005).

## Rationale

Operational coordination needs more than one day at a time.
