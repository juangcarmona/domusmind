---
id: UC-LISTS-SEE-ITEMS-IN-AGENDA
type: use-case
title: "See dated list items in the Agenda"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-AGENDA-PROJECTION
  - BR-LISTS-EDIT-ONLY-IN-LISTS
  - BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT
  - BR-AGENDA-ENTRY-PRIORITY-ORDER
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
  - TERM-AGENDA
  - TERM-LIST-ITEM
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Agenda Projection); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda, UX Grammar: Edit path); docs/_legacy/04_contexts/shared-lists-item-model.md (Projection Rules, Agenda Scope Placement Rules)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See time-relevant list items alongside plans and tasks, and jump to their list.

## Trigger

A person views the Agenda for a period.

## Preconditions

At least one list item has a due date, reminder or repeat occurrence in the period.

## Main Flow

1. The product shows each qualifying list item in the Agenda as a list item entry with its list name, distinct from tasks and plans, in the household section.
2. The person selects a projected item.
3. The product shows a read-only view with title, due date, checked state and list name, and a single action "Open in Lists".
4. The person opens it in Lists and edits it there.

## Alternative Flows

- The item is checked: it is still shown, de-emphasised.
- A plan in the Agenda has a linked list: the plan shows the list name and unchecked count; selecting that cue opens the list.

## Failure Conditions

None stated.

## Postconditions

The person has seen the item in time context; any change is made in Lists.
