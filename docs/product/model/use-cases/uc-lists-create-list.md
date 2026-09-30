---
id: UC-LISTS-CREATE-LIST
type: use-case
title: "Create a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-LIST-NAME-REQUIRED
  - BR-LISTS-CONTEXT-LINKS-INFORMATIONAL
uses-terms:
  - TERM-LIST
  - TERM-LIST-KIND
  - TERM-AREA
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (List Creation); docs/_legacy/00_product/surfaces/lists.md (Creation Model, Entry Points); src/backend/DomusMind.Domain/Lists/SharedList.cs (Create)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The household has a new, empty list ready to fill.

## Trigger

A person wants to group things to remember, buy or prepare, from the Lists surface or from the context of a plan or an Area (observed: docs/_legacy/00_product/surfaces/lists.md, Creation Model).

## Preconditions

The person belongs to a household.

## Main Flow

1. The person starts a new list and gives it a name.
2. Optionally, the person associates an Area, links a Plan or chooses a kind.
3. The product creates the list, empty, and makes it available to the household.

## Alternative Flows

- From a plan or an Area: the list is created already linked to that context (observed: docs/_legacy/00_product/surfaces/lists.md, Creation Model).

## Failure Conditions

- The name is empty: the list is not created.

## Postconditions

The list exists, is active, has no items and is visible to all household members.
