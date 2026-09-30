---
id: SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL
type: structured-behaviour
title: "Imported external entries never appear in household scope"
status: draft
illustrates:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY"
given:
  - "a member has imported external calendar entries"
when: "the household-scope timeline is requested"
then:
  - "those external entries do not appear"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-AGENDA-SCOPE"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection: External entries are excluded from the household timeline; Member Agenda Projection: External entries do not appear in household scope)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that a person's external calendar stays out of the household picture. The openspec states it twice, under both projections.

## Boundaries

None.
