---
id: UC-AGENDA-CREATE-FROM-AGENDA
type: use-case
title: "Add a plan, task or other item from the Agenda"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by: []
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-PLAN"
  - "TERM-TASK"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Interaction Grammar: Create, Creating, Creating From Canvas)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Capture a new item in context, with who and when already filled in (observed: agenda.md, Creating).

## Trigger

The person presses Add (a floating button on mobile) or clicks an empty time slot in a person's day timeline or in the Week grid (observed: agenda.md).

## Preconditions

- The Agenda is open.

## Main Flow

1. DomusMind opens the creation chooser with the current scope (household or person) as default subject and the current date as default date (observed: agenda.md).
2. The person chooses what to create and completes it; they may override subject, date and time.
3. The Agenda shows the new item (inferred).

## Alternative Flows

- 1a. The person clicked an empty time slot: the date and time of that slot are prefilled (observed: agenda.md, Creating From Canvas).

## Failure Conditions

- Not specified in the sources; the rules of the created item's own area apply (inferred).

## Postconditions

- The new item exists in its owning area (Calendar, Tasks or Lists) and appears in the Agenda (inferred from agenda.md, Data). Which item types the chooser offers is not listed in the source (see lead).
