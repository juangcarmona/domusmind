---
id: BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED
type: business-rule
title: "A cancelled plan cannot be changed"
status: draft
applies-to:
  - "UC-CALENDAR-CANCEL-PLAN"
  - "UC-CALENDAR-RESCHEDULE-PLAN"
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
  - "UC-CALENDAR-EDIT-PLAN-DETAILS"
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Rescheduling, Event Cancellation, Event Participant Management, Notes 4-5); docs/_legacy/04_contexts/calendar.md (Lifecycle); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; interview: product owner decision Q-0043 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Once a plan is cancelled it stays in history but leaves active planning, and nothing about it may change (decided: Q-0043): it cannot be rescheduled, cannot gain or lose participants, cannot gain or lose reminders, cannot have its title, description or colour edited and cannot be cancelled again (observed: calendar spec; CalendarEvent.cs; decided: Q-0043).

## Rationale

Cancellation must be final so the household can trust that a cancelled plan will not reappear changed (inferred).

## Examples

- Rescheduling a cancelled "Parent meeting" is rejected (observed: calendar spec).
- Cancelling it a second time is rejected (observed: calendar spec).
- Removing a participant from, or adding or removing a reminder on, a cancelled plan is rejected (decided: Q-0043).

## Exceptions

None (decided: Q-0043). The decision settles the conflict between legacy calendar.md ("except for archival"), the openspec (Notes 4-5, unspecified) and the domain code, which today still allows removing participants and adding or removing reminders on a cancelled plan (observed: CalendarEvent.cs); that permissiveness is an implementation gap against this rule.
