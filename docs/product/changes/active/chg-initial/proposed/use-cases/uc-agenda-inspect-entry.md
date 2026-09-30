---
id: UC-AGENDA-INSPECT-ENTRY
type: use-case
title: "Inspect an Agenda entry"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-LISTS-EDIT-ONLY-IN-LISTS"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Interaction Grammar: Selection, Inspector / Bottom Sheet, UX Grammar)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Understand an entry without opening an editor or leaving the Agenda (observed: agenda.md, Inspector).

## Trigger

The person taps or clicks an entry in Day or Week (observed: agenda.md, Click Semantics).

## Preconditions

- The entry is visible in the Agenda.

## Main Flow

1. DomusMind marks the entry selected and opens its detail beside the canvas (desktop) or in a bottom sheet (mobile).
2. The detail shows the entry type or source, its time range if timed, and type-specific facts: for a plan its participants and recurrence or reminders; for a task its status and due date; for a routine its recurrence and scope (observed: agenda.md).
3. For native entries it offers Edit, which opens the entry's edit dialog; on close the Agenda refreshes the window (observed: agenda.md, Editing).

## Alternative Flows

- 2a. A projected list item shows its list, title, due date or reminder, checked state, importance and note, all read-only, with a single "Open in Lists" action (observed: agenda.md, UX Grammar).
- 2b. An imported external entry shows its source label and read-only state and may offer "Open in Outlook" (observed: agenda.md).
- 1a. Deselecting closes the detail without navigating (observed: agenda.md).

## Failure Conditions

- Not specified in the sources.

## Postconditions

- The person understands the entry; for native entries they may continue to edit it.
