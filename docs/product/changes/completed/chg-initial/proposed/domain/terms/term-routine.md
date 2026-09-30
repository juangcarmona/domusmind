---
id: TERM-ROUTINE
type: domain-term
title: Routine
status: draft
defined-in: BC-TASKS
synonyms: []
uses-terms:
  - TERM-ROUTINE-RECURRENCE
  - TERM-ROUTINE-SCOPE
  - TERM-ROUTINE-OCCURRENCE
  - TERM-TASK
provenance:
  source: "openspec/specs/tasks/spec.md (Purpose, Routine Creation, Notes 1); docs/_legacy/04_contexts/tasks.md (Aggregate Roots, Routine; Routine Projection Model); src/backend/DomusMind.Domain/Tasks/Routine.cs; interview: product owner decision Q-0006 (E-0158); interview: product owner decision Q-0007 (E-0158); interview: product owner decision Q-0058 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

A recurring definition of operational household work: a pattern describing work that repeats on a schedule, not a task instance (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Aggregate Roots, Routine). Examples: weekly trash, daily pet feeding, monthly bill review, weekly grocery shopping (observed: docs/_legacy/04_contexts/tasks.md, Routine).

A routine has a name, a scope (the whole household or specific people), a colour and a recurrence; it is active or paused (observed: openspec/specs/tasks/spec.md, Routine Creation, Routine Pause). Routine kind is not part of the product (decided: Q-0006). A pause may name a date until which it lasts, and the routine resumes automatically on that date (decided: Q-0007). It may carry a time of day, which may affect ordering in time-aware views (observed: openspec/specs/tasks/spec.md, Routine Creation), and an end time after that time and an Area (decided: Q-0058; observed: src/backend/DomusMind.Domain/Tasks/Routine.cs, ValueObjects/RoutineSchedule.cs).

## Distinguish From

- Task (TERM-TASK): a routine does not produce tasks; it is projected on the fly into read surfaces on dates matching its recurrence (observed: openspec/specs/tasks/spec.md, Purpose).
- Recurring plan (TERM-PLAN): a recurring calendar plan is not a routine; routines define operational patterns, not fixed-time attendance (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Invariants, Routine Integrity).

## Usage

Shown in the Agenda as projected occurrences on matching dates while active (observed: openspec/specs/tasks/spec.md, Agenda Projection). Earlier feature specs spoke of routines "generating tasks"; that wording is stale and the projection model is canonical (observed: openspec/specs/tasks/spec.md, Notes 1).
