---
id: UC-AREAS-RENAME-AREA
type: use-case
title: Rename an Area
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by:
- BR-AREAS-ANY-MEMBER-MANAGES
- BR-AREAS-NAME-REQUIRED
uses-terms:
- TERM-AREA
provenance:
  source: "openspec/specs/areas/spec.md (Area Renaming); docs/_legacy/00_product/surfaces/areas.md (Inspector); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (Rename); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

An Area's name reflects how the household refers to it.

## Trigger

A person clicks the Area name in the Area detail to rename it inline (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector).

## Preconditions

The Area exists. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person edits the name.
2. The product updates the Area with the new name (observed: openspec/specs/areas/spec.md, Area Renaming).

## Alternative Flows

None.

## Failure Conditions

- The new name is blank or longer than 100 characters: the rename is rejected (observed: src/backend/DomusMind.Domain/Responsibilities/ValueObjects/ResponsibilityAreaName.cs).

## Postconditions

The Area carries the new name; ownership is unchanged.
