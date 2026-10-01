---
id: UC-LISTS-MAKE-LIST-PRIVATE
type: use-case
title: "Keep a list private to one person"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-PRIVATE-LIST
  - BR-LISTS-HOUSEHOLD-SCOPED
uses-terms:
  - TERM-LIST
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Purpose: shared by default, optionally private); interview: product owner decision Q-0027 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Goal

Keep a personal list in the household product without sharing it with everyone.

## Trigger

A person wants a list that only they see.

## Preconditions

The list exists and is shared with the household (the default).

## Main Flow

1. The person marks the list as private to one person.
2. The product stops showing the list to the other people of the household (decided: Q-0027).

## Alternative Flows

Undecided (inferred: gap): whether privacy is chosen at creation, changed later, or both, and whether a private list can be shared again.

## Failure Conditions

None stated in the sources.

## Postconditions

The list is private to one person and is not shared with the rest of the household.
