---
id: BR-TASKS-PEOPLE-SCOPE-NEEDS-TARGETS
type: business-rule
title: A routine for specific people must name them
status: draft
applies-to:
  - UC-TASKS-CREATE-ROUTINE
  - UC-TASKS-UPDATE-ROUTINE
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-SCOPE
  - TERM-MEMBER
provenance:
  source: openspec/specs/tasks/spec.md (Routine Creation); src/backend/DomusMind.Domain/Tasks/Routine.cs
  confidence: high
  recovered-from: observation
---

## Rule

A routine scoped to specific people MUST name at least one target person.

## Rationale

A people-scoped routine without people would apply to no one (observed: openspec/specs/tasks/spec.md, Routine Creation).

## Examples

- Creating a people-scoped routine with no target people is rejected (observed: openspec/specs/tasks/spec.md, scenario Member-scoped routine requires target members).
- The current model enforces the same rule on update (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs, ValidateScope).

## Exceptions

Household-scoped routines need no target people.
