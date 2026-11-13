---
id: SB-CALENDAR-SYNC-FAILURE
type: structured-behaviour
title: A connection reports that every selected feed failed
status: active
illustrates:
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
given:
  - The connection is Syncing.
  - Every selected feed fails to synchronize.
when: The synchronization finishes.
then:
  - The connection becomes Failed.
  - The sync failure is reported.
covers-transition:
  lifecycle: LC-EXTERNAL-CALENDAR-CONNECTION
  transition: SYNC-FAILURE
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
provenance:
  source: FR-CALENDAR-SHOW-CONNECTION-STATUS; UC-CALENDAR-SYNC-EXTERNAL-CALENDAR; product owner decision Q-0049; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; src/backend/DomusMind.Application/Features/Calendar/SyncExternalCalendarConnection/SyncExternalCalendarConnectionCommandHandler.cs
  confidence: high
  recovered-from: documentation
---

## Intent

A connection reports that every selected feed failed.

## Boundaries

The example concerns the local connection and leaves provider data and household Plans unchanged.
