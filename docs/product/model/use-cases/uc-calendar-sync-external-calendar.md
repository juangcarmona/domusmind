---
id: UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
type: use-case
title: "Sync an external calendar now"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-NO-CONCURRENT-SYNC"
  - "BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT"
  - "BR-CALENDAR-HORIZON-CHANGE-RELOADS"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization); docs/_legacy/04_contexts/calendar.md (Sync Rule, Recovery Rule); docs/_legacy/06_interfaces/external-calendar-api.md (Sync one connection, Sync all connections for one member)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Bring one's Agenda up to date with recent changes in Outlook without waiting for the hourly refresh (observed: calendar spec).

## Trigger

The person presses "Sync now" on a connection (observed: external-calendar-api.md).

## Preconditions

- The person owns an active connection (observed: calendar spec).

## Main Flow

1. The person asks to sync the connection.
2. DomusMind pulls new, updated and deleted occurrences from every selected calendar within the horizon, incrementally where it can (observed: calendar spec).
3. DomusMind updates the imported entries and each calendar's sync state and reports the counts of imported, updated and deleted entries (observed: calendar spec; external-calendar-api.md).

## Alternative Flows

- 1a. The person uses "Sync calendars" to sync all of their connections at once; each eligible connection is synced separately and the request returns before all finish (observed: external-calendar-api.md, Sync all connections).
- 2a. Stored sync state is invalid or stale: DomusMind clears that calendar's local entries, performs a fresh load and stores new sync state (observed: calendar spec).
- 2b. No calendar is selected: nothing is imported and the sync completes without error (observed: calendar spec).

## Failure Conditions

- A sync of the same connection is already running: the request is rejected or deferred (observed: calendar spec).
- The provider authorization can no longer be refreshed: the sync fails and the connection shows authorisation expired (observed: external-calendar-api.md; ExternalCalendarConnection.cs).

## Postconditions

- Imported entries within the horizon reflect the provider and remain read-only; no plan was created (observed: calendar spec).
