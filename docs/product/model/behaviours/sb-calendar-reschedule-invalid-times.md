---
id: SB-CALENDAR-RESCHEDULE-INVALID-TIMES
type: structured-behaviour
title: "Rescheduling to an end before the start is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-RESCHEDULE-PLAN"
  - "BR-CALENDAR-PLAN-END-AFTER-START"
given:
  - "a new end is earlier than the new start"
when: "a household member tries to reschedule the plan"
then:
  - "the rescheduling is rejected"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Rescheduling: Rescheduling with invalid times is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that the end-after-start rule also holds on reschedule.

## Boundaries

None.
