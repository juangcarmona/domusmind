---
id: SB-CALENDAR-REMOVE-REMINDER
type: structured-behaviour
title: "A reminder is removed from a plan"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
given:
  - "a plan has a reminder with a specific offset"
when: "a household member removes that reminder"
then:
  - "the reminder is no longer part of the plan"
  - "the plan and its other reminders remain unchanged"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders: Reminder is removed from an event)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that removal affects only that reminder.

## Boundaries

None.
