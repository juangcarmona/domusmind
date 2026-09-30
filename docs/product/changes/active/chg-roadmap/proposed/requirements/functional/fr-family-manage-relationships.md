---
id: FR-FAMILY-MANAGE-RELATIONSHIPS
type: functional-requirement
title: "Record and remove relationships between people"
status: draft
derived-from:
  - UC-FAMILY-MANAGE-RELATIONSHIPS
  - BR-FAMILY-RELATIONSHIP-INTEGRITY
  - BR-FAMILY-MANAGERS-ADMINISTER
verification:
  - scenario-ref: "SB-FAMILY-RELATIONSHIP-ASSIGNED"
  - scenario-ref: "SB-FAMILY-RELATIONSHIP-UNKNOWN-MEMBER-REJECTED"
  - scenario-ref: "SB-FAMILY-DUPLICATE-RELATIONSHIP-REJECTED"
  - scenario-ref: "SB-FAMILY-SELF-RELATIONSHIP-REJECTED"
  - scenario-ref: "SB-FAMILY-RELATIONSHIP-REMOVED"
uses-terms:
  - TERM-RELATIONSHIP
provenance:
  source: "openspec/specs/family/spec.md (Relationship Assignment); docs/_legacy/01_system/system-spec.md (Deferred to V1.1); interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0024 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Requirement

Planned: V1.1. The product MUST let a manager, and only a manager (decided: Q-0024), record parent-child, spouse, sibling and care relationships between two different people of the household, reject duplicates of the same type for the same pair, and let relationships be removed (observed: openspec/specs/family/spec.md, Relationship Assignment). A care relationship is not a person role; there is no Caregiver role (decided: Q-0021).

## Rationale

Care and kinship structures inform later rules such as finding responsible adults (observed: docs/_legacy/04_contexts/family.md, Relationship).
