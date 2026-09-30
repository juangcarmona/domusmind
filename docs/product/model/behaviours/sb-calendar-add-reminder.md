---
id: SB-CALENDAR-ADD-REMINDER
type: structured-behaviour
title: "A reminder is added to a plan"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
given:
  - "a plan exists"
  - "the plan has no reminder with the same offset"
when: "a household member adds a reminder with that offset"
then:
  - "the reminder is part of the plan's reminders"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders: Reminder is added to an event)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that reminders are offsets attached to a plan.

## Boundaries

Does not assert anything about notification delivery.
