---
id: SB-TASKS-PAUSE-PAUSED-REJECTED
type: structured-behaviour
title: Paused routine cannot be paused again
status: draft
illustrates:
  - UC-TASKS-PAUSE-ROUTINE
  - BR-TASKS-PAUSE-RESUME
given:
  - "A routine in paused status"
when: A person tries to pause it
then:
  - "The operation is rejected"
uses-terms:
  - TERM-ROUTINE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Pause, scenario Paused routine cannot be paused again)
  confidence: high
  recovered-from: documentation
---

## Intent

Pause is a transition from active only.

## Boundaries

None.
