---
id: SB-CALENDAR-ADD-DUPLICATE-REMINDER
type: structured-behaviour
title: "A second reminder with the same offset is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
  - "BR-CALENDAR-REMINDER-OFFSET-UNIQUE"
given:
  - "a plan has a reminder 30 minutes before its start"
when: "a household member adds another reminder 30 minutes before"
then:
  - "the addition is rejected"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders: Duplicate reminder offset is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that offsets are unique per plan.

## Boundaries

None.
