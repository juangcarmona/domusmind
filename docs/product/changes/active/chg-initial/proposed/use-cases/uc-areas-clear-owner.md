---
id: UC-AREAS-CLEAR-OWNER
type: use-case
title: Clear an Area's Owner
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by: []
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-OWNERSHIP-GAP
provenance:
  source: docs/_legacy/04_contexts/responsibilities.md (Suggested future commands)
  confidence: low
  recovered-from: documentation
---

## Goal

An Area deliberately returns to unowned.

## Trigger

A person removes the Owner of an Area without naming a new one.

## Preconditions

The Area has an Owner.

## Main Flow

1. The person clears the Owner.
2. The Area becomes unowned and shows as an ownership gap (inferred).

## Alternative Flows

None.

## Failure Conditions

None known.

## Postconditions

Planned (future): listed only as a suggested future capability (observed: docs/_legacy/04_contexts/responsibilities.md, Commands, Suggested future commands).
