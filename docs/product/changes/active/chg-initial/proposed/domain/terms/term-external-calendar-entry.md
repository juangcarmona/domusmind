---
id: TERM-EXTERNAL-CALENDAR-ENTRY
type: domain-term
title: "External Calendar Entry"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Imported entry"
  - "Outlook entry"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-PLAN"
  - "TERM-AGENDA"
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose, Member Agenda Projection); docs/_legacy/04_contexts/calendar.md (ExternalCalendarEntry, External Calendar Entry Boundary); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarEntry.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A read-only calendar occurrence imported from a selected provider calendar, such as a dentist appointment or a work stand-up from Outlook. It carries a title, start and end, an all-day flag, and may carry the original time zone, location, participant summary, provider status and a link to open it in the provider (observed: legacy calendar.md, ExternalCalendarEntry; ExternalCalendarEntry.cs).

An entry deleted at the provider is removed or marked deleted locally and stops appearing (observed: legacy calendar.md, External Calendar Entry Boundary).

## Distinguish From

- **Plan**: an external entry is never a plan, is never converted into one automatically and never enters household plan-writing flows (observed: calendar spec, Purpose; legacy calendar.md).

## Usage

Shown only in the owning person's Member-scope Agenda, with a source label such as "Outlook", as read-only detail that may offer "Open in Outlook" (observed: calendar spec, Member Agenda Projection; agenda.md, External calendar entry rules).
