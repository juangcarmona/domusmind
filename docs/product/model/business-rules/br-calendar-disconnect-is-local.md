---
id: BR-CALENDAR-DISCONNECT-IS-LOCAL
type: business-rule
title: "Disconnecting only removes DomusMind's own copy"
status: draft
applies-to:
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Disconnection); docs/_legacy/06_interfaces/external-calendar-api.md (Disconnect connection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Disconnecting removes the connection, its imported entries and the locally held delegated access; nothing is done to the provider account or its calendars, and no plan is changed (observed: calendar spec).

## Rationale

DomusMind only ever reads the provider (observed: calendar spec, Purpose).

## Examples

- Ana disconnects her work Outlook; her Outlook calendar is untouched and its entries vanish from her Agenda immediately (observed: calendar spec).

## Exceptions

None.
