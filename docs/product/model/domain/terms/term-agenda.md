---
id: TERM-AGENDA
type: domain-term
title: "Agenda"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Agenda View"
  - "Unified Agenda"
uses-terms:
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Purpose, Role, Data); openspec/specs/calendar/spec.md (Purpose, Household Timeline Projection, Member Agenda Projection); docs/_legacy/03_domain/ubiquitous-language.md (Agenda); docs/_legacy/01_system/system-spec.md (Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The Agenda is DomusMind's unified read surface across Calendar, Tasks and Lists: the household's primary temporal surface. It gathers plans (Calendar), tasks and projected routine occurrences (Tasks), temporal list items (Lists) and, in a person's own scope only, imported external calendar entries, and shows them together for a date window (observed: agenda.md, Data; calendar spec, Purpose).

It answers what is happening in the household, when, who is involved, what needs attention, where load is concentrated and what comes next, for the household as a whole or for one person, by day, week or month (observed: agenda.md, Purpose).

The Agenda is not a bounded context and owns no records: the write model stays divided (Calendar owns plans, Tasks owns tasks and routines, Lists owns list items) and only the read surface is unified (observed: agenda.md, Data; legacy system-spec.md).

## Distinguish From

- **Household timeline**: the household-scope temporal read model that feeds the Household scope of the Agenda; the Agenda is the surface, the timeline one of its projections (observed: calendar spec, Household Timeline Projection).
- **Calendar**: the area owning plans; the Agenda is not Calendar's own view, it spans three areas (observed: calendar spec, Purpose).
- **Today, Planning, Member Agenda**: retired separate surfaces absorbed into the Agenda (observed: agenda.md, What This Surface Replaces).

## Usage

Opens by default on Household scope, Day mode, today. People switch scope (Household or one person) and mode (Day, Week, Month), inspect entries and create native items from it. Plans, tasks and routines can be edited from the Agenda; projected list items and imported external entries cannot (observed: agenda.md, Default Entry State, Interaction Grammar).
