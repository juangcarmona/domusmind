---
id: BR-CALENDAR-SUPPORTED-SYNC-HORIZONS
type: business-rule
title: "Sync horizons are limited to 30, 90, 180 or 365 days"
status: draft
applies-to:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
uses-terms:
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection, External Calendar Configuration); docs/_legacy/04_contexts/calendar.md (Horizon Rule); src/backend/DomusMind.Domain/Calendar/ExternalConnections/SyncHorizon.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A connection imports from one day before now to 30, 90, 180 or 365 days forward; any other forward value is rejected. A new connection starts at 90 days forward (observed: calendar spec; SyncHorizon.cs).

## Rationale

Bounds how much external data DomusMind stores and keeps incremental sync reliable (inferred).

## Examples

- Saving a 60-day horizon is rejected (observed: calendar spec, unsupported horizon scenario).
- A new connection covers yesterday to 90 days ahead (observed: calendar spec).

## Exceptions

None.
