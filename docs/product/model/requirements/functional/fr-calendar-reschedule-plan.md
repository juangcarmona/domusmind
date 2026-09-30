---
id: FR-CALENDAR-RESCHEDULE-PLAN
type: functional-requirement
title: "Reschedule a plan"
status: draft
derived-from:
  - "UC-CALENDAR-RESCHEDULE-PLAN"
  - "BR-CALENDAR-PLAN-END-AFTER-START"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
verification:
  - scenario-ref: "SB-CALENDAR-RESCHEDULE-PLAN"
  - scenario-ref: "SB-CALENDAR-RESCHEDULE-CANCELLED-PLAN"
  - scenario-ref: "SB-CALENDAR-RESCHEDULE-INVALID-TIMES"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Rescheduling); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (Reschedule)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household member change the start, and optionally the end, of a scheduled plan while keeping its identity, participants and reminders unchanged, and MUST reject rescheduling a cancelled plan (observed: calendar spec).

## Rationale

Plans move often; the household should not have to re-enter who is involved (inferred).
