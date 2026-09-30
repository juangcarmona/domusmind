---
id: SB-TASKS-RESUME-ACTIVE-REJECTED
type: structured-behaviour
title: Active routine cannot be resumed
status: draft
illustrates:
  - UC-TASKS-RESUME-ROUTINE
  - BR-TASKS-PAUSE-RESUME
given:
  - "A routine in active status"
when: A person tries to resume it
then:
  - "The operation is rejected"
uses-terms:
  - TERM-ROUTINE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Resume, scenario Non-paused routine cannot be resumed)
  confidence: high
  recovered-from: documentation
---

## Intent

Resume is a transition from paused only.

## Boundaries

None.
