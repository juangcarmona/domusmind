---
id: SB-CALENDAR-SYNC-SUCCESS
type: structured-behaviour
title: A connection reports a successful sync
status: active
illustrates:
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
given:
  - The connection is Syncing.
  - All selected feeds can be read.
when: The synchronization completes without feed errors.
then:
  - The connection becomes Healthy.
  - The last successful sync is updated.
covers-transition:
  lifecycle: LC-EXTERNAL-CALENDAR-CONNECTION
  transition: SYNC-SUCCESS
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
provenance:
  source: FR-CALENDAR-SHOW-CONNECTION-STATUS; UC-CALENDAR-SYNC-EXTERNAL-CALENDAR; product owner decision Q-0049; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; src/backend/DomusMind.Application/Features/Calendar/SyncExternalCalendarConnection/SyncExternalCalendarConnectionCommandHandler.cs
  confidence: high
  recovered-from: documentation
---

## Intent

A connection reports a successful sync.

## Boundaries

The example concerns the local connection and leaves provider data and household Plans unchanged.
