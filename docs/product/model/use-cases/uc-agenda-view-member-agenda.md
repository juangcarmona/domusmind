---
id: UC-AGENDA-VIEW-MEMBER-AGENDA
type: use-case
title: "See one person's Agenda"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-AGENDA-PROJECTION-READ-ONLY"
  - "BR-AGENDA-ENTRY-PRIORITY-ORDER"
  - "BR-AGENDA-MEMBER-SCOPE-PLANS"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY"
  - "BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
  - "BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Scope: Member, Member + Day = Timeline, External calendar entry rules); openspec/specs/calendar/spec.md (Member Agenda Projection, Background Feed Refresh); docs/_legacy/06_interfaces/external-calendar-api.md (View Agenda in member scope); interview: product owner decision Q-0041 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See what one person's day, week or month looks like, including gaps, conflicts and overload (observed: agenda.md, Member scope).

## Trigger

The person taps a household member in the Agenda, or follows a link to that person's Agenda (observed: agenda.md, Click Semantics, Entry Points).

## Preconditions

- The person viewed belongs to the household.

## Main Flow

1. DomusMind shows that person's tasks, the plans they take part in plus household plans without participants (decided: Q-0041), routines for them or the household they are responsible for, temporal list items, and, when viewing their own scope, their imported external entries within the window (observed: agenda.md; calendar spec).
2. In Day mode it shows a timeline: timed plans as blocks by start and duration from the earliest relevant time (06:00 to 23:00 at least), an all-day lane above, and tasks, routines and list items in a compact non-timed section (observed: agenda.md).
3. External entries carry a source label such as "Outlook" and are read-only (observed: calendar spec).

## Alternative Flows

- 1a. One of the person's connections is stale: opening the Agenda triggers a catch-up sync (observed: calendar spec).
- 2a. Week mode shows plans as time blocks with compact task and routine lanes (observed: agenda.md, Week).

## Failure Conditions

- Not specified in the sources.

## Postconditions

- The person has seen the individual picture; no record was created or changed (observed: calendar spec).

Resolved conflict: the openspec includes plans "where the member participates or is a household plan", while agenda.md shows household plans "the person participates in". The product owner decided that the Member scope shows the plans the person takes part in plus household plans without participants (decided: Q-0041).
