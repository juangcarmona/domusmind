---
id: BR-CALENDAR-HORIZON-CHANGE-RELOADS
type: business-rule
title: "Changing the horizon or selection reloads affected calendars"
status: draft
applies-to:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
uses-terms:
  - "TERM-SYNC-HORIZON"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration, External Calendar Synchronization); docs/_legacy/04_contexts/calendar.md (Horizon Rule, Recovery Rule); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (Configure); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarFeed.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Changing the sync horizon discards the incremental sync state of affected calendars so the next sync performs a fresh load of the new window. The same fresh load happens when stored sync state is invalid or stale, after clearing that calendar's local entries (observed: calendar spec; legacy calendar.md, Recovery Rule).

## Rationale

The horizon is part of the identity of the sync state; incremental changes computed for one window are meaningless for another (observed: legacy calendar.md).

## Examples

- Moving from 90 to 180 days forward reloads every selected calendar for the 180-day window (observed: calendar spec).

## Exceptions

None.
