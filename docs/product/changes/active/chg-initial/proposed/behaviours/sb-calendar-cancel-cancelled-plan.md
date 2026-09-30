---
id: SB-CALENDAR-CANCEL-CANCELLED-PLAN
type: structured-behaviour
title: "A plan cannot be cancelled twice"
status: draft
illustrates:
  - "UC-CALENDAR-CANCEL-PLAN"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
given:
  - "a plan is cancelled"
when: "a household member tries to cancel it again"
then:
  - "the cancellation is rejected"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Cancellation: Already cancelled event cannot be cancelled again)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that cancellation happens once.

## Boundaries

None.
