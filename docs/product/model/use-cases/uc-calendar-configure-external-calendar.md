---
id: UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR
type: use-case
title: "Choose calendars and horizon for a connection"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-SUPPORTED-SYNC-HORIZONS"
  - "BR-CALENDAR-HORIZON-CHANGE-RELOADS"
  - "BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT"
  - "BR-CALENDAR-DEFAULT-REFRESH-HOURLY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
  - "BR-CALENDAR-NO-CONCURRENT-SYNC"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration); docs/_legacy/04_contexts/calendar.md (ExternalCalendarFeed, Horizon Rule); docs/_legacy/06_interfaces/external-calendar-api.md (Configure connection); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (Configure); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarFeed.cs; interview: product owner decision Q-0047 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Decide which of one's provider calendars appear in one's Agenda and how far ahead (observed: calendar spec).

## Trigger

The person edits a connection in their settings.

## Preconditions

- The person owns an active external calendar connection (observed: calendar spec).

## Main Flow

1. The person selects one or more provider calendars and a forward horizon of 30, 90, 180 or 365 days.
2. DomusMind saves the selection and horizon (observed: calendar spec).
3. Where the horizon changed or calendars were added, DomusMind discards their sync state so the next sync reloads them (observed: calendar spec).

## Alternative Flows

- 1a. The person deselects a calendar: its entries stop appearing and its sync state is cleared (observed: calendar spec).
- 1b. The person turns scheduled refresh on or off for this connection, or changes this connection's interval (observed: external-calendar-api.md; decided: Q-0047).

## Failure Conditions

- Unsupported horizon value: rejected (observed: calendar spec).
- Invalid refresh interval: rejected (observed: external-calendar-api.md). Undecided (Q-0047): the allowed values are decided later.
- A sync is in progress: the change is rejected (observed: external-calendar-api.md, 409).

## Postconditions

- The connection reflects the new selection and horizon; no plan was created or changed (observed: calendar spec).
