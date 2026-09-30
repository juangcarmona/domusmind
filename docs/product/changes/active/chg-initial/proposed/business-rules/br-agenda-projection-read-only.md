---
id: BR-AGENDA-PROJECTION-READ-ONLY
type: business-rule
title: "Agenda projections never change records"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection, Member Agenda Projection); docs/_legacy/00_product/surfaces/agenda.md (Data); docs/_legacy/03_domain/ubiquitous-language.md (Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Building the Agenda, household or member, never creates or changes a plan, task, routine, list item or external entry; it only reads them (observed: calendar spec).

## Rationale

The write model is divided across Calendar, Tasks and Lists; only the read surface is unified (observed: agenda.md, Data).

## Examples

- Opening the Agenda does not turn a routine occurrence into a task (inferred from agenda.md and legacy system-spec.md, routines are projected on the fly).

## Exceptions

Opening the Member-scope Agenda may start a catch-up sync of a stale external calendar connection; that changes imported entries, not household records (observed: calendar spec, Background Feed Refresh).
