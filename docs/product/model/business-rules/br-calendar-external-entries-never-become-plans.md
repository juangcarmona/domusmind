---
id: BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS
type: business-rule
title: "External calendar data never becomes household plans"
status: draft
applies-to:
  - "BC-CALENDAR"
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose, all external calendar requirements); docs/_legacy/04_contexts/calendar.md (External Calendar Entry Boundary, Projection Rule)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Connecting, configuring, synchronizing, refreshing or disconnecting an external calendar never creates, converts or changes a household plan (observed: calendar spec, each external calendar requirement).

## Rationale

Imported data is integration state, not household planning; mixing them would put a person's work calendar into the household's shared plans (observed: calendar spec, Purpose; inferred for the motivation).

## Examples

- After connecting Outlook, the household's plans are unchanged (observed: calendar spec, connect scenario).
- Disconnecting removes imported entries but leaves every plan untouched (observed: calendar spec, disconnect scenario).

## Exceptions

None.
