---
id: SB-CALENDAR-AUTH-EXPIRED
type: structured-behaviour
title: A connection reports expired provider authorization
status: active
illustrates:
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
given:
  - The connection is Syncing.
  - Provider authorization cannot be refreshed.
when: The synchronization attempts to obtain provider access.
then:
  - The connection reports Authorization expired.
  - The member is told to reconnect the account.
covers-transition:
  lifecycle: LC-EXTERNAL-CALENDAR-CONNECTION
  transition: AUTH-EXPIRED
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
provenance:
  source: FR-CALENDAR-SHOW-CONNECTION-STATUS; UC-CALENDAR-SYNC-EXTERNAL-CALENDAR; product owner decision Q-0049; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; src/backend/DomusMind.Application/Features/Calendar/SyncExternalCalendarConnection/SyncExternalCalendarConnectionCommandHandler.cs
  confidence: high
  recovered-from: documentation
---

## Intent

A connection reports expired provider authorization.

## Boundaries

This is accepted product intent. The outer exception handler can overwrite AuthExpired with Failed in the inspected implementation; the example does not claim that defect is fixed.
