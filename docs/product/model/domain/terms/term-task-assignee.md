---
id: TERM-TASK-ASSIGNEE
type: domain-term
title: Assignee
status: draft
defined-in: BC-TASKS
synonyms:
  - TaskAssignment
uses-terms:
  - TERM-TASK
  - TERM-MEMBER
provenance:
  source: openspec/specs/tasks/spec.md (Task Assignment); docs/_legacy/04_contexts/tasks.md (Internal Entities, TaskAssignment; Invariants, Assignment)
  confidence: high
  recovered-from: documentation
---

## Definition

The person of the household responsible for executing a task. A task has zero or one primary assignee at a time, and the assignee must be a person of the same household (observed: openspec/specs/tasks/spec.md, Task Assignment; docs/_legacy/04_contexts/tasks.md, Invariants, Assignment; Ubiquitous Language Notes).

## Distinguish From

- Area owner (TERM-AREA): owning an area may motivate an assignment but never makes it; assignment is always explicit (observed: openspec/specs/tasks/spec.md, Task Assignment).
- Routine target people: a routine scoped to specific people lists them as targets, not as assignees (observed: openspec/specs/tasks/spec.md, Routine Creation).

## Usage

Set at creation or later; can be changed (observed: openspec/specs/tasks/spec.md, Task Creation, Task Assignment). Personal task lists show the tasks assigned to one person (observed: docs/_legacy/04_contexts/tasks.md, Read Models, Personal Task List).
