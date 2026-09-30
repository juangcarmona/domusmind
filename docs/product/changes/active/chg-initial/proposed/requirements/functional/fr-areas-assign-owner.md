---
id: FR-AREAS-ASSIGN-OWNER
type: functional-requirement
title: Assign a single Owner to an Area
status: draft
derived-from:
- UC-AREAS-ASSIGN-OWNER
- BR-AREAS-SINGLE-OWNER
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
verification:
- scenario-ref: SB-AREAS-ASSIGN-FIRST-OWNER
- scenario-ref: SB-AREAS-REPLACE-OWNER
- scenario-ref: SB-AREAS-REJECT-OUTSIDE-OWNER
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
provenance:
  source: "openspec/specs/areas/spec.md (Requirement: Primary Owner Assignment); interview: product owner decision Q-0012 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let the household assign a person of the same household as the Owner of an Area. An Area MUST have at most one Owner; assigning an Owner when one exists MUST replace the previous Owner, and that replacement is a traceable transfer (FR-AREAS-TRANSFER-OWNERSHIP) (decided: Q-0012). Assigning a person outside the household MUST be rejected.

## Rationale

One accountable person per Area (observed: openspec/specs/areas/spec.md, Primary Owner Assignment).
