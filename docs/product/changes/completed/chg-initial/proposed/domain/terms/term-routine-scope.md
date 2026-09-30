---
id: TERM-ROUTINE-SCOPE
type: domain-term
title: Routine Scope
status: draft
defined-in: BC-TASKS
synonyms: []
uses-terms:
  - TERM-ROUTINE
  - TERM-HOUSEHOLD
  - TERM-MEMBER
provenance:
  source: openspec/specs/tasks/spec.md (Routine Creation); src/backend/DomusMind.Domain/Tasks/Routine.cs
  confidence: high
  recovered-from: documentation
---

## Definition

Who a routine applies to: the whole Household, or specific Members (people). A routine scoped to people must name at least one target person (observed: openspec/specs/tasks/spec.md, Routine Creation). A household-scoped routine applies to everyone in the household (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs, AppliesTo).

## Distinguish From

- Assignee (TERM-TASK-ASSIGNEE): scope targets people for a recurring pattern; it does not assign a task.

## Usage

Shown on routine entries in the Agenda as part of their summary (observed: docs/_legacy/00_product/surfaces/agenda.md, item display grammar; context only).
