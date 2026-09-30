---
id: FR-AREAS-ADD-SUPPORT
type: functional-requirement
title: Add Support people to an Area
status: draft
derived-from:
- UC-AREAS-ADD-SUPPORT
- BR-AREAS-UNIQUE-SUPPORT
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
- BR-AREAS-OWNER-NOT-SUPPORT
verification:
- scenario-ref: SB-AREAS-ADD-SUPPORT
- scenario-ref: SB-AREAS-REJECT-DUPLICATE-SUPPORT
- scenario-ref: SB-AREAS-REJECT-OWNER-AS-SUPPORT
uses-terms:
- TERM-AREA
- TERM-AREA-SUPPORT
provenance:
  source: "openspec/specs/areas/spec.md (Requirement: Secondary Owner Assignment); interview: product owner decision Q-0017 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let the household add one or more people of the same household as Support of an Area without changing its Owner. Adding a person who is already Support of that Area MUST be rejected, and so MUST adding the Area's Owner as Support (decided: Q-0017).

## Rationale

Support gives backup or shared accountability coverage (observed: openspec/specs/areas/spec.md, Secondary Owner Assignment).
