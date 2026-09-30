---
id: SB-CALENDAR-SCHEDULE-PLAN-UNKNOWN-PARTICIPANT
type: structured-behaviour
title: "A plan with a participant from outside the household is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD"
given:
  - "a participant does not belong to the household"
when: "a household member schedules a plan including that participant"
then:
  - "no plan is created"
  - "a validation error is returned"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling: Event with invalid participant reference is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that participants are always people of the plan's household.

## Boundaries

None.
