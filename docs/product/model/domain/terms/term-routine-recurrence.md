---
id: TERM-ROUTINE-RECURRENCE
type: domain-term
title: Routine Recurrence
status: draft
defined-in: BC-TASKS
synonyms:
  - RecurrenceRule
  - RoutineSchedule
uses-terms:
  - TERM-ROUTINE
provenance:
  source: "openspec/specs/tasks/spec.md (Routine Creation); docs/_legacy/04_contexts/tasks.md (Routine Integrity, Routine Projection Model); src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs; interview: product owner decision Q-0058 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

The rule describing on which dates a routine occurs: a frequency (Daily, Weekly, Monthly or Yearly) plus day selectors. Weekly needs at least one day of the week; Monthly at least one day of the month; Yearly a month of the year and at least one day of the month (observed: openspec/specs/tasks/spec.md, Routine Creation). It may carry a time of day (observed: openspec/specs/tasks/spec.md, Routine Creation) and an end time after that time (decided: Q-0058; observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/RoutineSchedule.cs).

## Distinguish From

- The repeat of a Plan or a list item: shared temporal vocabulary does not imply a shared concept; a routine recurrence governs operational work, not attendance (observed: docs/_legacy/04_contexts/tasks.md, Invariants, Routine Integrity).

## Usage

Evaluated against a date range when building the Agenda; each matching date yields a projected occurrence (observed: docs/_legacy/04_contexts/tasks.md, Routine Projection Model).
