---
id: BR-CALENDAR-DEFAULT-REFRESH-HOURLY
type: business-rule
title: "Scheduled refresh is on and hourly by default"
status: draft
applies-to:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection, Background Feed Refresh); docs/_legacy/04_contexts/calendar.md (Sync Rule); docs/_legacy/06_interfaces/external-calendar-api.md (Configure connection); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; interview: product owner decision Q-0047 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A new connection has scheduled refresh enabled with a 60-minute interval (observed: calendar spec; ExternalCalendarConnection.cs). Scheduled refresh is set per connection, not as a user setting: the person can turn it off and change the interval of each connection (decided: Q-0047; observed: external-calendar-api.md, Configure connection). The legacy "configurable interval in user settings" wording is superseded (observed: legacy calendar.md; decided: Q-0047).

## Rationale

Keeps the Agenda current without manual syncs (observed: calendar spec, Background Feed Refresh).

## Examples

- Ana connects Outlook at 09:00; without doing anything her entries are refreshed about hourly (inferred from the default).

## Exceptions

Undecided (Q-0047): the allowed interval values and the staleness threshold that triggers catch-up are decided later.
