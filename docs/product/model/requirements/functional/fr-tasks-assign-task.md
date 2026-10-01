---
id: FR-TASKS-ASSIGN-TASK
type: functional-requirement
title: Assign a task to one person
status: draft
derived-from:
  - UC-TASKS-ASSIGN-TASK
  - BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
  - BR-TASKS-EXPLICIT-ASSIGNMENT
verification:
  - scenario-ref: "SB-TASKS-ASSIGN-MEMBER"
  - scenario-ref: "SB-TASKS-ASSIGN-NON-MEMBER-REJECTED"
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Task Assignment)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person assign a task to a person of the same household, and change that assignment later. A task MUST have at most one primary assignee at a time, and the product MUST NOT assign tasks automatically (observed: openspec/specs/tasks/spec.md, Requirement Task Assignment).

## Rationale

Who does what must be clear and deliberate.
