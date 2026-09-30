---
id: JRN-AGENDA-COORDINATE-THE-DAY
type: journey
title: "Coordinate the household's day from the Agenda"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - use-case: "UC-AGENDA-INSPECT-ENTRY"
  - use-case: "UC-AGENDA-NAVIGATE"
  - use-case: "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - use-case: "UC-AGENDA-CREATE-FROM-AGENDA"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Default Entry State, Scope, Interaction Grammar); docs/_legacy/00_product/experience.md (Agenda)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intended Outcome

A person understands what matters today for the household and for one person, and captures what is missing, without leaving the Agenda (observed: agenda.md, Default Entry State, Anti-Patterns "navigate to a separate page").

## Entry Conditions

- The person belongs to a household and opens DomusMind.

## Journey Narrative

1. DomusMind opens on today's household board: the shared row and one row per person, entries in priority order (observed: agenda.md).
2. The person selects an entry to see its detail in the inspector or bottom sheet (observed: agenda.md).
3. They tap a person to switch to that person's scope, keeping Day and today (observed: agenda.md).
4. On the person's timeline they spot a gap or conflict (observed: agenda.md, Timeline).
5. They click an empty slot or press Add to create a plan or task prefilled with that person, date and time (observed: agenda.md).

## Variants and Branches

- The person moves to Week when Day is not enough, or to Month for load awareness (observed: agenda.md).
- A projected list item leads to "Open in Lists" rather than editing in place (observed: agenda.md).

## Completion Conditions

The person has the answer to "what matters today?" and any new item appears in the Agenda (inferred).
