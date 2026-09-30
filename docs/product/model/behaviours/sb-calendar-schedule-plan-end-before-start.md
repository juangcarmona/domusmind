---
id: SB-CALENDAR-SCHEDULE-PLAN-END-BEFORE-START
type: structured-behaviour
title: "A plan ending before it starts is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "BR-CALENDAR-PLAN-END-AFTER-START"
given:
  - "a household member provides a start and an end for a new plan"
  - "the end is earlier than the start"
when: "the member schedules the plan"
then:
  - "no plan is created"
  - "a validation error is returned"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling: Event with end time before start time is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that a plan can never have a negative duration.

## Boundaries

Does not assert what happens when the end equals the start; the domain code rejects an equal end for timed plans (EventTime.cs).
