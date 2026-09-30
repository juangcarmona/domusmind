---
id: FR-AREAS-ARCHIVE-AREA
type: functional-requirement
title: Archive an Area
status: draft
derived-from:
- UC-AREAS-ARCHIVE-AREA
verification:
- scenario-ref: SB-AREAS-ARCHIVE-AREA
uses-terms:
- TERM-AREA
provenance:
  source: "openspec/specs/areas/spec.md (Requirement: Area Archiving); interview: product owner decision Q-0013 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let the household archive an Area. An archived Area MUST be retained, excluded from the default active view, and shown when the archived filter is applied. Archiving an Area is current product (decided: Q-0013).

## Rationale

Areas fall out of use without their history being lost (observed: openspec/specs/areas/spec.md, Area Archiving). The domain code has no archived state yet; tasks and routines lose their reference to an archived Area (decided: Q-0010; BR-TASKS-ARCHIVED-AREA-CLEARED) (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).
