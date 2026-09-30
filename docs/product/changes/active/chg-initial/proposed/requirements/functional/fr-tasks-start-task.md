---
id: FR-TASKS-START-TASK
type: functional-requirement
title: Mark a task as in progress
status: draft
derived-from:
  - UC-TASKS-START-TASK
verification:
  - scenario: "A person starts a pending task and its status becomes in progress"
uses-terms:
  - TERM-TASK
  - TERM-TASK-STATUS
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Commands, Invariants); openspec/specs/tasks/spec.md (Notes 2); interview: product owner decision Q-0005 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Requirement

Planned (future), low confidence: the current task lifecycle is pending, then completed or cancelled, with no in-progress state (decided: Q-0005). A later version MAY let a person mark a pending task as in progress (observed: docs/_legacy/04_contexts/tasks.md, Commands StartTask; Invariants, Lifecycle). The entry rule is undocumented (observed: openspec/specs/tasks/spec.md, Notes 2) and the state is absent from the current model (observed: src/backend/DomusMind.Domain/Tasks/Enums/HouseholdTaskStatus.cs).

## Rationale

Shows that someone has picked the work up.
