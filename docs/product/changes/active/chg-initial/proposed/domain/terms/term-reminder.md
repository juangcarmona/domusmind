---
id: TERM-REMINDER
type: domain-term
title: "Reminder"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Reminder Offset"
uses-terms:
  - "TERM-PLAN"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders); docs/_legacy/04_contexts/calendar.md (Reminder, Reminder Integrity); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; docs/_legacy/03_domain/ubiquitous-language.md (Reminder); interview: product owner decision Q-0043 (E-0158); interview: product owner decision Q-0046 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A scheduled prompt tied to a time-bound commitment. On a plan, a reminder is an offset before the plan's start, for example 30 minutes, 2 hours or 24 hours before; each offset appears at most once per plan (observed: calendar spec, Event Reminders; legacy calendar.md, Reminder). The domain code stores the offset in minutes and requires it to be greater than zero (observed: CalendarEvent.cs, AddReminder).

The same word is reused on a list item, where the reminder is an absolute moment used for Agenda projection (observed: legacy ubiquitous-language.md, Reminder). Shared vocabulary does not mean a shared record.

## Distinguish From

- **Notification**: the delivery of a prompt to a person. Calendar defines reminder schedules; delivery is not a Calendar responsibility (observed: calendar spec, Event Reminders).
- **Task**: a reminder is not work to be done (observed: legacy ubiquitous-language.md, Reminder).

## Usage

People add and remove reminders on a plan; the Agenda inspector shows a plan's reminders (observed: calendar spec; agenda.md, Inspector). Reminders on a cancelled plan are kept as they are and can no longer be added or removed (decided: Q-0043).

Undecided, deferred (Q-0046): whether DomusMind delivers reminder notifications to people and through which channel is left to a later decision; until then a reminder is informational.
