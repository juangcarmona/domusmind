---
id: SB-CALENDAR-SCHEDULE-PLAN-MINIMUM
type: structured-behaviour
title: "A plan is scheduled with only a title and a start"
status: draft
illustrates:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START"
  - "BR-CALENDAR-PLAN-BELONGS-TO-ONE-HOUSEHOLD"
given:
  - "a household exists"
when: "a household member schedules a plan with a title and a start"
then:
  - "a plan is created in scheduled status"
  - "the plan belongs to that household"
uses-terms:
  - "TERM-PLAN"
  - "TERM-HOUSEHOLD"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling: Household creates an event with minimum inputs)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that a title and a start are enough to capture a plan.

## Boundaries

None.
