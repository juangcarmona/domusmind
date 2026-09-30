---
id: FR-AREAS-TRANSFER-OWNERSHIP
type: functional-requirement
title: Transfer ownership of an Area explicitly
status: draft
derived-from:
- UC-AREAS-TRANSFER-OWNERSHIP
- BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
verification:
- scenario-ref: SB-AREAS-TRANSFER-OWNERSHIP
- scenario-ref: SB-AREAS-REJECT-OUTSIDE-TRANSFER
uses-terms:
- TERM-AREA
- TERM-RESPONSIBILITY-TRANSFER
provenance:
  source: "openspec/specs/areas/spec.md (Requirement: Responsibility Transfer; Notes); interview: product owner decision Q-0012 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

When the household changes the Owner of an Area that already has one, the product MUST treat the change as an explicit transfer of ownership to another person of the same household (decided: Q-0012). The Area MUST remain active and owned throughout, exactly one Owner MUST exist afterwards, and the handover MUST be traceable. A transfer to a person outside the household MUST be rejected, leaving the existing Owner in place.

## Rationale

Handovers of accountability are intentional and auditable, not silent overwrites (observed: openspec/specs/areas/spec.md, Responsibility Transfer). Transfer and assignment are one household action; only changing an existing Owner is a transfer (decided: Q-0012).
