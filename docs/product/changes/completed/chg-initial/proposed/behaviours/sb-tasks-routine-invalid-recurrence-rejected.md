---
id: SB-TASKS-ROUTINE-INVALID-RECURRENCE-REJECTED
type: structured-behaviour
title: Weekly routine without days is rejected
status: draft
illustrates:
  - UC-TASKS-CREATE-ROUTINE
  - BR-TASKS-VALID-ROUTINE-RECURRENCE
given:
  - "A weekly frequency is specified"
when: A routine is created with no days of the week
then:
  - "The routine is not created"
  - "A validation error is returned"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-RECURRENCE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Creation, scenario Routine with invalid recurrence is rejected)
  confidence: high
  recovered-from: documentation
---

## Intent

A routine must resolve to concrete dates.

## Boundaries

Monthly and yearly selector rules are stated but have no scenario.
