---
id: FR-AREAS-AREA-COLOR
type: functional-requirement
title: Give each Area a colour cue
status: draft
derived-from:
- UC-AREAS-CHANGE-AREA-COLOR
verification:
- scenario: Picking green from the palette in an Area's detail shows a green dot on that Area's row
uses-terms:
- TERM-AREA
provenance:
  source: "docs/_legacy/00_product/surfaces/areas.md (Default View, Inspector, Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (Color, Repaint); interview: product owner decision Q-0019 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

Each Area MUST carry a colour cue shown on its row, and the household MUST be able to change it inline from the Area detail.

## Rationale

Quick visual recognition (observed: docs/_legacy/00_product/surfaces/areas.md, Default View and Inspector; src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs). The Areas spec left colour out until it was specified (observed: openspec/specs/areas/spec.md, Notes); the Area colour is part of the product (decided: Q-0019).
