---
id: BR-CALENDAR-REFRESH-FAILURES-ISOLATED
type: business-rule
title: "One connection's refresh failure does not block others"
status: draft
applies-to:
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-SYNC"
provenance:
  source: "openspec/specs/calendar/spec.md (Background Feed Refresh); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (RecordSyncFailure)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When background refresh fails for one connection, every other stale connection is still refreshed (observed: calendar spec). The failing connection records the failure and its next scheduled attempt (observed: ExternalCalendarConnection.cs).

## Rationale

One person's expired Outlook authorization must not stop the rest of the household's calendars from updating (inferred).

## Examples

- Leo's Outlook authorization has expired; Ana's connection still refreshes on schedule (inferred from the rule).

## Exceptions

None.
