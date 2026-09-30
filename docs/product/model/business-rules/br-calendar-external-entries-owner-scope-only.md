---
id: BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY
type: business-rule
title: "External entries appear only in their owner's Member scope"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-AGENDA-SCOPE"
  - "TERM-HOUSEHOLD-TIMELINE"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection, Member Agenda Projection); docs/_legacy/04_contexts/calendar.md (Projection Rule); docs/_legacy/00_product/surfaces/agenda.md (External calendar entry rules)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

In Phase 1, imported external calendar entries appear only in the Member-scope Agenda of the person whose connection imported them, never in Household scope or the household timeline (observed: calendar spec; agenda.md).

## Rationale

A person's external calendar is personal; the household picture shows only household planning (inferred).

## Examples

- Ana's Outlook stand-up shows in Ana's Member scope, not in the Household board nor in Leo's scope (observed: calendar spec, household exclusion scenario; inferred for other members).

## Exceptions

None stated. Whether a later phase may widen this is not specified (inferred from "phase 1").
