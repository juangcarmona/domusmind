---
id: UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS
type: use-case
title: "Refresh stale external calendars in the background"
status: draft
primary-actor: "ACT-CALENDAR-SYNC-SCHEDULER"
supporting-actors:
  - "ACT-MICROSOFT-OUTLOOK"
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-DEFAULT-REFRESH-HOURLY"
  - "BR-CALENDAR-NO-CONCURRENT-SYNC"
  - "BR-CALENDAR-REFRESH-FAILURES-ISOLATED"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (Background Feed Refresh); docs/_legacy/04_contexts/calendar.md (Sync Rule); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (RecordSyncSuccess, NextScheduledSyncUtc); interview: product owner decision Q-0047 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Keep every person's Agenda current without anyone having to sync manually (observed: calendar spec).

## Trigger

The scheduler evaluates each connection on that connection's refresh interval (60 minutes by default, decided: Q-0047); catch-up is also triggered when a person signs in or opens their Member-scope Agenda with a stale connection (observed: calendar spec).

## Preconditions

- Connections exist with scheduled refresh enabled (observed: external-calendar-api.md, scheduledRefreshEnabled).

## Main Flow

1. The scheduler finds connections whose last successful sync is older than the threshold.
2. It synchronizes each stale connection and updates its sync time (observed: calendar spec).
3. It skips connections synchronized recently (observed: calendar spec).

## Alternative Flows

- 1a. A person opens their Member-scope Agenda with a stale connection: a catch-up sync is triggered for it (observed: calendar spec).
- 1b. A person signs in with a stale connection: a catch-up sync is triggered (observed: calendar spec).

## Failure Conditions

- A connection is already syncing: it is not synced concurrently (observed: calendar spec).
- A connection's refresh fails: the failure is recorded and the other connections still refresh (observed: calendar spec).

## Postconditions

- Stale connections are refreshed; no plan was created (observed: calendar spec). Undecided (Q-0047): the allowed interval values and the staleness threshold that triggers catch-up are decided later.
