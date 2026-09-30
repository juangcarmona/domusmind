---
id: FR-CALENDAR-MANAGE-PLAN-REMINDERS
type: functional-requirement
title: "Add and remove plan reminders"
status: draft
derived-from:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
  - "BR-CALENDAR-REMINDER-OFFSET-UNIQUE"
  - "BR-CALENDAR-REMINDER-BEFORE-START"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
verification:
  - scenario-ref: "SB-CALENDAR-ADD-REMINDER"
  - scenario-ref: "SB-CALENDAR-ADD-DUPLICATE-REMINDER"
  - scenario-ref: "SB-CALENDAR-REMOVE-REMINDER"
  - scenario-ref: "SB-CALENDAR-REMOVE-MISSING-REMINDER"
  - scenario-ref: "SB-CALENDAR-CHANGE-REMINDER-CANCELLED-PLAN"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (AddReminder, RemoveReminder); interview: product owner decision Q-0043 (E-0158); interview: product owner decision Q-0046 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household member add reminders to a plan as offsets before its start (for example 30 minutes or 24 hours before) and remove them, with each offset unique per plan (observed: calendar spec), and MUST reject adding or removing reminders on a cancelled plan (decided: Q-0043). Delivering the reminder is outside this requirement (observed: calendar spec).

Undecided, deferred (Q-0046): whether DomusMind delivers reminder notifications to people and through which channel is left to a later decision; until then a reminder is informational.

## Rationale

Reminders let the household act in time without remembering (inferred from the product's mission).
