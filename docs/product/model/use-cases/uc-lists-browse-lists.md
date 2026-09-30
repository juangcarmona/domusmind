---
id: UC-LISTS-BROWSE-LISTS
type: use-case
title: "Browse the household's lists"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by: []
uses-terms:
  - TERM-LIST
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (List Retrieval); docs/_legacy/00_product/surfaces/lists.md (List Switcher, List Switcher Behavior)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See which lists exist and which still have remaining work.

## Trigger

A person opens Lists.

## Preconditions

The person belongs to a household.

## Main Flow

1. The product shows every active list of the household with its name and number of unchecked items, plus any Area or linked-plan cue.
2. The person picks a list to work in.

## Alternative Flows

- The household has no lists yet: the switcher is empty and creating a list is offered (inferred).

## Failure Conditions

None stated.

## Postconditions

The person knows which lists need attention. Archived lists are not shown by default.
