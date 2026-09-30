---
id: FR-TASKS-RENAME-TASK
type: functional-requirement
title: Rename an open task
status: draft
derived-from:
  - UC-TASKS-RENAME-TASK
  - BR-TASKS-TITLE-REQUIRED
verification:
  - scenario: "A person renames a pending task and the task shows the new title"
  - scenario: "Renaming a completed task is rejected"
uses-terms:
  - TERM-TASK
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; openspec/specs/tasks/spec.md (Notes 4); interview: product owner decision Q-0009 (E-0158)"
  confidence: medium
  recovered-from: observation
---

## Requirement

The product MUST let a person change the title of a task that is not completed or cancelled (observed: docs/_legacy/04_contexts/tasks.md, Commands, RenameTask; src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs, Rename). Renaming a task is current product (decided: Q-0009); no openspec requirement covers it yet (observed: openspec/specs/tasks/spec.md, Notes 4).

## Rationale

Titles are captured quickly and often need correcting.
