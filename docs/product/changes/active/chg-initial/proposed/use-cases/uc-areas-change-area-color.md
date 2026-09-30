---
id: UC-AREAS-CHANGE-AREA-COLOR
type: use-case
title: Change an Area's colour
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by: []
uses-terms:
- TERM-AREA
provenance:
  source: "docs/_legacy/00_product/surfaces/areas.md (Default View, Inspector, Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (Color, Repaint); openspec/specs/areas/spec.md (Notes, Area color); interview: product owner decision Q-0019 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

An Area is recognisable at a glance by its colour cue.

## Trigger

A person clicks the colour dot in the Area detail (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector).

## Preconditions

The Area exists.

## Main Flow

1. The product shows a compact palette.
2. The person picks a colour.
3. The Area shows the new colour cue (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction; the domain code stores and changes an Area colour, src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).

## Alternative Flows

None.

## Failure Conditions

None stated by the sources.

## Postconditions

The Area carries the new colour.

The Areas spec left colour out until it was specified (observed: openspec/specs/areas/spec.md, Notes, Area color), while the surface spec and the domain code include it; the Area colour is part of the product (decided: Q-0019).
