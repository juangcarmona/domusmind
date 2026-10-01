---
id: BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT
type: business-rule
title: "A plan-linked list does not project its items through the plan"
status: draft
applies-to:
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - UC-LISTS-UPDATE-LIST
  - UC-AGENDA-VIEW-HOUSEHOLD-AGENDA
  - UC-AGENDA-VIEW-MEMBER-AGENDA
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-PLAN
  - TERM-PROJECTED-LIST-ITEM
  - TERM-AGENDA
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection); openspec/specs/web-app/spec.md (Agenda Plan-List Reference Cue); docs/_legacy/04_contexts/shared-lists-item-model.md (Plan-linked temporal list item); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda, Anti-Patterns); docs/_legacy/00_product/surfaces/agenda.md (Projected List Item Scope Placement Rules, Lists in Agenda); openspec/specs/lists/spec.md (Purpose)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A plan linked to a list shows in the Agenda only a compact cue with the list name and unchecked count, which navigates to the list; the plan never expands the list's items inline. Items of that list appear in the Agenda only through their own due date, reminder or repeat, as separate entries, never in the plan's time slot and independently of the plan's schedule or visibility (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Plan-linked temporal list item; docs/_legacy/00_product/surfaces/lists.md, Relationship with Agenda; openspec/specs/calendar/spec.md, Household Timeline Projection; openspec/specs/web-app/spec.md, "two independent mechanisms").

## Rationale

A linked list remains a list; it must not become the plan's task batch (observed: docs/_legacy/00_product/surfaces/lists.md, Anti-Patterns). Keeping the two appearance mechanisms independent keeps the Agenda unambiguous (observed: agenda.md, "two independent appearance mechanisms").

## Examples

- A "Party prep" list linked to Saturday's party plan shows as "Party prep - 5" on the plan; its undated items do not appear in the Agenda.
- "Pack swimsuit" due Friday on the list linked to Saturday's trip appears on Friday, while the trip on Saturday shows its list cue (inferred example of the rule).

## Exceptions

None.
