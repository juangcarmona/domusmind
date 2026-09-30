---
id: TERM-EXTERNAL-CALENDAR-FEED
type: domain-term
title: "External Calendar Feed"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Selected calendar"
  - "Provider calendar"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (ExternalCalendarFeed); openspec/specs/calendar/spec.md (External Calendar Configuration); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarFeed.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

One provider calendar under an external calendar connection, for example a person's main Outlook calendar or a "School" calendar. It records the calendar's identity and display name, whether it is selected, the sync window and when it was last synchronized (observed: legacy calendar.md, ExternalCalendarFeed; external-calendar-api.md, Get connection detail).

Only selected feeds import entries. Deselecting a feed stops its entries from appearing and clears its sync state (observed: calendar spec, External Calendar Configuration).

## Distinguish From

- **Connection**: the account-level link; a connection holds several feeds (observed: legacy calendar.md).
- **List**: a DomusMind shared list is unrelated despite the everyday word "calendar list" (inferred).

## Usage

Chosen by the person when configuring a connection; the default provider calendar may be preselected on connection (observed: external-calendar-api.md, Connect Outlook account).
