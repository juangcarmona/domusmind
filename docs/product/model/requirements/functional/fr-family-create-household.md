---
id: FR-FAMILY-CREATE-HOUSEHOLD
type: functional-requirement
title: "Create a household from a name"
status: draft
derived-from:
  - UC-FAMILY-CREATE-HOUSEHOLD
  - BR-FAMILY-HOUSEHOLD-NAME-REQUIRED
  - BR-FAMILY-CREATOR-IS-FIRST-MANAGER
verification:
  - scenario-ref: "SB-FAMILY-HOUSEHOLD-CREATED"
  - scenario-ref: "SB-FAMILY-HOUSEHOLD-EMPTY-NAME-REJECTED"
  - scenario: "A person creates a household and is its manager"
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Household Creation); interview: product owner decision Q-0022 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a household be created with a name as the only required input. The new household MUST get a stable identity that never changes, MUST start with no pets or relationships, and MUST have the person who created it as its first manager (decided: Q-0022). The product MUST reject a missing or empty name (observed: openspec/specs/family/spec.md, Household Creation).

## Rationale

The household is the root every other part of DomusMind hangs from.
