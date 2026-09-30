---
id: SB-TASKS-ROUTINE-PEOPLE-SCOPE-NEEDS-TARGETS
type: structured-behaviour
title: People-scoped routine without people is rejected
status: draft
illustrates:
  - UC-TASKS-CREATE-ROUTINE
  - BR-TASKS-PEOPLE-SCOPE-NEEDS-TARGETS
given:
  - "A people scope is specified"
when: A routine is created with no target people
then:
  - "The routine is not created"
  - "A validation error is returned"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-SCOPE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Creation, scenario Member-scoped routine requires target members)
  confidence: high
  recovered-from: documentation
---

## Intent

A people-scoped routine must apply to someone.

## Boundaries

None.
