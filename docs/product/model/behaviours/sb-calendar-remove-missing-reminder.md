---
id: SB-CALENDAR-REMOVE-MISSING-REMINDER
type: structured-behaviour
title: "Removing a reminder the plan does not have is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
  - "BR-CALENDAR-REMINDER-OFFSET-UNIQUE"
given:
  - "a plan has no reminder with a specific offset"
when: "a household member tries to remove a reminder with that offset"
then:
  - "the removal is rejected"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders: Removing a non-existent reminder is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that only existing reminders can be removed.

## Boundaries

None.
