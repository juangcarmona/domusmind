---
id: SB-CALENDAR-SCHEDULE-ALL-DAY-PLAN
type: structured-behaviour
title: "A plan can be scheduled for a whole day without a time"
status: draft
illustrates:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START"
given:
  - "a household exists"
when: "a household member schedules \"School trip\" on a Friday with no time of day"
then:
  - "a scheduled all-day plan exists on that Friday"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "src/backend/DomusMind.Domain/Calendar/ValueObjects/EventTime.cs (Day, DayRange); docs/_legacy/00_product/surfaces/agenda.md (all-day lane); interview: product owner decision Q-0045 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Intent

Establishes that a plan does not need a time of day (decided: Q-0045).

## Boundaries

Does not assert how the plan is laid out in the Agenda; a multi-day plan spanning several dates is also accepted under the same rule.
