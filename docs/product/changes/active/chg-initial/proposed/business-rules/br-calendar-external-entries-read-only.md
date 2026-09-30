---
id: BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY
type: business-rule
title: "Imported external calendar entries are read-only"
status: draft
applies-to:
  - "BC-CALENDAR"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-AGENDA-INSPECT-ENTRY"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose, External Calendar Synchronization, Member Agenda Projection); docs/_legacy/04_contexts/calendar.md (External Calendar Entry Boundary); docs/_legacy/00_product/surfaces/agenda.md (External calendar entry rules)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Imported external calendar entries cannot be edited in DomusMind and nothing is ever written back to the provider (observed: calendar spec; legacy calendar.md; legacy system-spec.md, "no write-back").

## Rationale

The provider remains the source of truth for a person's external calendar; DomusMind only gives visibility (inferred).

## Examples

- Selecting an Outlook stand-up in the Agenda opens read-only detail with no edit action (observed: agenda.md).

## Exceptions

None.
