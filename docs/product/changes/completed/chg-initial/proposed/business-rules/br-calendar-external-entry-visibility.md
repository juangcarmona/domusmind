---
id: BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY
type: business-rule
title: "When an imported entry is shown"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (Member Agenda Projection); docs/_legacy/00_product/surfaces/agenda.md (External calendar entry rules); docs/_legacy/04_contexts/calendar.md (External Calendar Entry Boundary)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

An imported entry is shown only when its connection is active, its feed is selected, it falls inside the active sync horizon and the displayed date window, and it has not been deleted at the provider (observed: calendar spec, Member Agenda Projection; agenda.md).

## Rationale

Stored integration data must never outlive the person's choices or the provider's own deletions (inferred).

## Examples

- Entries from a deselected "School" calendar stop appearing (observed: calendar spec).
- An Outlook meeting deleted at the provider disappears after the next sync (observed: legacy calendar.md).

## Exceptions

None.
