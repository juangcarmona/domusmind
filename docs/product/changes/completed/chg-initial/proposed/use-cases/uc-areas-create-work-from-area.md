---
id: UC-AREAS-CREATE-WORK-FROM-AREA
type: use-case
title: Create a task, routine or plan from an Area
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by: []
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-TASK
- TERM-ROUTINE
- TERM-PLAN
provenance:
  source: docs/_legacy/00_product/surfaces/areas.md (Inspector, Interaction); openspec/specs/areas/spec.md (Cross-Context Referencing)
  confidence: medium
  recovered-from: documentation
---

## Goal

New household work starts already linked to the Area it belongs to.

## Trigger

A person chooses New task, New routine or New plan in the Area detail (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector and Interaction).

## Preconditions

An Area is selected.

## Main Flow

1. The person chooses one explicit creation action; no generic chooser is shown.
2. The product opens the relevant form pre-filled: a task with the Area and the Owner as assignee, a routine with the Area, a plan with the Area and the Owner as participant (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector).
3. The person completes the form in the owning part of the product.

## Alternative Flows

- The Area has no Owner: (inferred) only the Area is pre-filled; the sources do not say.

## Failure Conditions

Handled by the task, routine or plan creation itself.

## Postconditions

The new item references the Area; the Area's ownership is unchanged (observed: openspec/specs/areas/spec.md, Cross-Context Referencing).
