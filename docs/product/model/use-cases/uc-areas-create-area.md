---
id: UC-AREAS-CREATE-AREA
type: use-case
title: Add an Area
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by:
- BR-AREAS-ANY-MEMBER-MANAGES
- BR-AREAS-ONE-HOUSEHOLD
- BR-AREAS-NAME-REQUIRED
- BR-AREAS-OWNERSHIP-OPTIONAL
uses-terms:
- TERM-AREA
- TERM-HOUSEHOLD
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: "openspec/specs/areas/spec.md (Area Creation); docs/_legacy/00_product/surfaces/areas.md (Header, Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (Create); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

The household has a new named Area of household life to make accountability visible for.

## Trigger

A person chooses Add area on the Areas surface (observed: docs/_legacy/00_product/surfaces/areas.md, Header).

## Preconditions

The household exists. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person enters an Area name.
2. Optionally, they choose an Owner and Support people (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).
3. The product creates the Area in the household (observed: openspec/specs/areas/spec.md, Area Creation).

## Alternative Flows

- No Owner is chosen: the Area is created unowned and shows as an ownership gap (observed: openspec/specs/areas/spec.md, scenario Household creates an Area).

## Failure Conditions

- The name is blank or longer than 100 characters: the Area is not created (observed: src/backend/DomusMind.Domain/Responsibilities/ValueObjects/ResponsibilityAreaName.cs).

## Postconditions

The Area exists in the household, active, with the chosen name and any chosen Owner and Support.
