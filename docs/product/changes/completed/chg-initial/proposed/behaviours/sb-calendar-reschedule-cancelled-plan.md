---
id: SB-CALENDAR-RESCHEDULE-CANCELLED-PLAN
type: structured-behaviour
title: "A cancelled plan cannot be rescheduled"
status: draft
illustrates:
  - "UC-CALENDAR-RESCHEDULE-PLAN"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
given:
  - "a plan is cancelled"
when: "a household member tries to reschedule it"
then:
  - "the rescheduling is rejected"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Rescheduling: Cancelled event cannot be rescheduled)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that cancellation is final for the schedule.

## Boundaries

None.
