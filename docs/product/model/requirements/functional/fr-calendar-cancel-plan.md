---
id: FR-CALENDAR-CANCEL-PLAN
type: functional-requirement
title: "Cancel a plan"
status: draft
derived-from:
  - "UC-CALENDAR-CANCEL-PLAN"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
verification:
  - scenario-ref: "SB-CALENDAR-CANCEL-PLAN"
  - scenario-ref: "SB-CALENDAR-CANCEL-CANCELLED-PLAN"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Cancellation); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (Cancel)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household member cancel a scheduled plan, keeping it in history but no longer active, and MUST reject cancelling an already cancelled plan (observed: calendar spec).

## Rationale

Plans that will not happen must stop demanding attention without losing the record that they existed (inferred).
