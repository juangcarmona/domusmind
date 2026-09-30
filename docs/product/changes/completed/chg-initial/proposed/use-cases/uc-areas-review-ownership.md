---
id: UC-AREAS-REVIEW-OWNERSHIP
type: use-case
title: Review household ownership on the Areas surface
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by: []
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
- TERM-OWNERSHIP-GAP
provenance:
  source: "openspec/specs/areas/spec.md (Ownership Visibility); docs/_legacy/00_product/surfaces/areas.md (Entry Points, Default View, Main List, Inspector, Data, Interaction); interview: product owner decision Q-0020 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Goal

Understand in seconds which Areas exist, who owns and supports each one, and where ownership is missing (observed: docs/_legacy/00_product/surfaces/areas.md, Purpose and Success Criteria).

## Trigger

A person opens Areas from the main navigation, from a contextual link in the Agenda, Lists or an item's detail, or from the onboarding follow-up (observed: docs/_legacy/00_product/surfaces/areas.md, Entry Points).

## Preconditions

The person is signed in to the household.

## Main Flow

1. The product shows a compact list of Areas ordered unowned, partially assigned, fully assigned, then archived if shown; within a group, manually ordered if supported, otherwise alphabetical (observed: openspec/specs/areas/spec.md, Ownership Visibility; docs/_legacy/00_product/surfaces/areas.md, Main List).
2. Each row shows the colour cue, name, Owner or a gap indicator, Support people, and counts of linked open tasks, plans and routines (observed: docs/_legacy/00_product/surfaces/areas.md, Default View).
3. The person selects an Area to open its detail: identity, Owner, Support, related work (tasks, plans and routines, plus linked lists as contextual memory; decided: Q-0020) and creation actions (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector).

## Alternative Flows

- The person searches by Area name, Owner name or Support name (observed: docs/_legacy/00_product/surfaces/areas.md, Data).
- The person filters by all, unowned, mine, active or archived (observed: docs/_legacy/00_product/surfaces/areas.md, Data).
- The person clicks an Owner or Support name and goes to that person's Agenda (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).
- The person opens a linked item to edit it (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Failure Conditions

None stated by the sources.

## Postconditions

The person knows the ownership state of the household's Areas. Nothing is changed.
