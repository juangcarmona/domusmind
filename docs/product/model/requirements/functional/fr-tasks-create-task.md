---
id: FR-TASKS-CREATE-TASK
type: functional-requirement
title: Create a task with a title
status: draft
derived-from:
  - UC-TASKS-CREATE-TASK
  - BR-TASKS-TITLE-REQUIRED
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
verification:
  - scenario-ref: "SB-TASKS-CREATE-TITLE-ONLY"
  - scenario-ref: "SB-TASKS-CREATE-WITH-ASSIGNEE-DUE"
  - scenario-ref: "SB-TASKS-CREATE-NON-MEMBER-REJECTED"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
  - TERM-AREA
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Task Creation)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person create a task with a title as the only required input, optionally with an assignee from the household, a due date and an area for grouping. A new task MUST start pending with a manual origin. An assignee from outside the household MUST be rejected (observed: openspec/specs/tasks/spec.md, Requirement Task Creation).

## Rationale

Capturing work must be cheap, and responsibility must stay within the household.
