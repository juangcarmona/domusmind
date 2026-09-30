---
id: BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT
type: business-rule
title: "Only selected provider calendars import entries"
status: draft
applies-to:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration, External Calendar Synchronization); docs/_legacy/04_contexts/calendar.md (External Calendar Connection Integrity); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarFeed.cs (UpdateSelection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Only selected provider calendars import entries. Deselecting a calendar stops its stored entries appearing and clears its sync state; a connection with nothing selected syncs nothing and completes without error (observed: calendar spec).

## Rationale

The person decides which parts of their external calendar are relevant to the household (inferred).

## Examples

- Ana deselects her "Holidays" calendar; its entries leave her Agenda (observed: calendar spec, deselected feed scenario).

## Exceptions

Each provider calendar may be selected only once per connection (observed: legacy calendar.md).
