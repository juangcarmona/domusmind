---
id: UC-LISTS-OPEN-LIST
type: use-case
title: "Open a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by: []
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (List Detail); docs/_legacy/00_product/surfaces/lists.md (Active List Behavior, Completed Items)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Work with the full content of one list.

## Trigger

A person selects a list in the switcher, follows a deep link, or follows the list cue of a plan in the Agenda (observed: docs/_legacy/00_product/surfaces/lists.md, Entry Points).

## Preconditions

The list exists in the person's household.

## Main Flow

1. The product shows the list with its unchecked items first, then its checked items, each group in the list's order.
2. Checked items are collapsed under "Completed (N)" and can be expanded.

## Alternative Flows

None.

## Failure Conditions

- The list does not exist in the household: nothing is shown (inferred).

## Postconditions

The person sees every item of the list.
