---
id: UC-CALENDAR-EDIT-PLAN-DETAILS
type: use-case
title: "Edit a plan's title, description or colour"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (Commands: RenameEvent); openspec/specs/calendar/spec.md (Note 3); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (Edit, Repaint); docs/_legacy/00_product/surfaces/agenda.md (Editing)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Correct or refine how a plan is described without changing when it happens (inferred from legacy calendar.md, RenameEvent, and CalendarEvent.cs).

## Trigger

A person opens the plan's edit action, for example from the Agenda inspector (observed: agenda.md, Editing).

## Preconditions

- The plan exists and is not cancelled (observed: CalendarEvent.cs).

## Main Flow

1. The person changes the plan's title and optionally its description.
2. DomusMind keeps the plan's identity and records the new details (observed: CalendarEvent.cs Edit; calendar spec Note 3, identity stability).

## Alternative Flows

- 1a. The person changes the plan's colour (observed: CalendarEvent.cs Repaint).

## Failure Conditions

- The plan is cancelled: the edit is rejected (observed: CalendarEvent.cs).
- The new title is empty or too long: rejected (observed: EventTitle.cs).

## Postconditions

- The plan shows its new details in the Agenda (inferred).

The openspec lists renaming as an active command with no specified behaviour (observed: calendar spec, Note 3); this use case rests on legacy prose and the domain code, confidence medium.
