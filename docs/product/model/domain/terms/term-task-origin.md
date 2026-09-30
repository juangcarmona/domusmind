---
id: TERM-TASK-ORIGIN
type: domain-term
title: Task Origin
status: draft
defined-in: BC-TASKS
synonyms:
  - TaskOrigin
uses-terms:
  - TERM-TASK
provenance:
  source: docs/_legacy/04_contexts/tasks.md (Internal Entities, TaskOrigin); openspec/specs/tasks/spec.md (Task Creation)
  confidence: medium
  recovered-from: documentation
---

## Definition

Where a task came from. Possible origins: manual, or external integration (observed: docs/_legacy/04_contexts/tasks.md, Internal Entities, TaskOrigin). Tasks created by people have a manual origin (observed: openspec/specs/tasks/spec.md, Task Creation).

## Distinguish From

- Routine (TERM-ROUTINE) and Plan (TERM-PLAN) are never origins: tasks are not generated from them (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, TaskOrigin note).

## Usage

Shown in a person's task list (observed: docs/_legacy/04_contexts/tasks.md, Read Models, Personal Task List). The current model records no origin on a task (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs); the external-integration origin has no described capability.
