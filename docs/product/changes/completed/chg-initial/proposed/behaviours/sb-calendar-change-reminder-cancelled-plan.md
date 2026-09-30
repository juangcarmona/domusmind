---
id: SB-CALENDAR-CHANGE-REMINDER-CANCELLED-PLAN
type: structured-behaviour
title: "Reminders on a cancelled plan cannot be changed"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
given:
  - "a plan is cancelled"
  - "the plan has a reminder 24 hours before its start"
when: "a household member tries to remove that reminder"
then:
  - "the removal is rejected"
  - "the plan keeps its reminder"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (Lifecycle); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (RemoveReminder); interview: product owner decision Q-0043 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Intent

Establishes that nothing changes on a cancelled plan, reminders included (decided: Q-0043).

## Boundaries

Adding a reminder to a cancelled plan is rejected under the same rule; this example covers only the removal. The domain code still allows the removal today (observed: CalendarEvent.cs).
