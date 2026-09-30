---
id: SB-CALENDAR-RESCHEDULE-PLAN
type: structured-behaviour
title: "Rescheduling keeps participants and other details"
status: draft
illustrates:
  - "UC-CALENDAR-RESCHEDULE-PLAN"
given:
  - "a plan exists in scheduled status"
when: "a household member provides a new start"
then:
  - "the plan's schedule is updated"
  - "participants and other plan details remain unchanged"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Rescheduling: Event is rescheduled)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that moving a plan changes only its time.

## Boundaries

None.
