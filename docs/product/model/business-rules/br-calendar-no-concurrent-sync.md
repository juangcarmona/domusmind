---
id: BR-CALENDAR-NO-CONCURRENT-SYNC
type: business-rule
title: "A connection never syncs twice at the same time"
status: draft
applies-to:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization, Background Feed Refresh); docs/_legacy/06_interfaces/external-calendar-api.md (409 sync already in progress); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (TryAcquireLease)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Two synchronizations of the same connection never run at the same time; a second request while one is running is rejected or deferred, whether it comes from the person or the background refresh (observed: calendar spec; ExternalCalendarConnection.cs sync lease).

## Rationale

Concurrent syncs would race on the same entries and sync state (inferred).

## Examples

- Ana presses "Sync now" while the hourly refresh is running: her request is rejected or deferred (observed: calendar spec).

## Exceptions

The legacy API also rejects a configuration change while a sync is in progress (observed: external-calendar-api.md, Configure connection 409).
