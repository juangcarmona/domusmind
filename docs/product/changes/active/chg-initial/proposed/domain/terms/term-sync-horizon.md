---
id: TERM-SYNC-HORIZON
type: domain-term
title: "Sync Horizon"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Forward horizon"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection, External Calendar Configuration); docs/_legacy/04_contexts/calendar.md (Horizon Rule); src/backend/DomusMind.Domain/Calendar/ExternalConnections/SyncHorizon.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The bounded time window of external calendar data DomusMind imports for a connection: from one day before now to a chosen number of days forward. The supported forward values are 30, 90, 180 and 365 days; the default is 90 (observed: calendar spec; legacy calendar.md, Horizon Rule; SyncHorizon.cs).

The horizon is part of the identity of the sync state: changing it discards the incremental sync state and triggers a fresh load of the new window (observed: calendar spec, Horizon change triggers rehydration).

## Distinguish From

- **Agenda mode window**: the day, week or month being displayed; entries stored inside the horizon but outside the displayed window are not shown (observed: agenda.md, External calendar entry rules).

## Usage

Set by the person per connection when configuring it (observed: calendar spec, External Calendar Configuration).
