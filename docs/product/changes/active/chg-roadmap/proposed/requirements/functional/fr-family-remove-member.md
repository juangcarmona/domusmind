---
id: FR-FAMILY-REMOVE-MEMBER
type: functional-requirement
title: "Remove people and pets from the household"
status: draft
derived-from:
  - UC-FAMILY-REMOVE-MEMBER
  - BR-FAMILY-MANAGERS-ADMINISTER
verification:
  - scenario-ref: "SB-FAMILY-MEMBER-REMOVED"
  - scenario-ref: "SB-FAMILY-PET-REMOVED"
uses-terms:
  - TERM-MEMBER
  - TERM-PET
provenance:
  source: "openspec/specs/family/spec.md (Member Removal, Pet Registration); docs/_legacy/01_system/system-spec.md (Deferred to V1.1); interview: product owner decision Q-0024 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Requirement

Planned: V1.1. The product MUST let a manager remove a person or pet from the household roster, and only a manager (decided: Q-0024); a removed person MUST NOT take part in new relationships, and removal MUST NOT change other areas directly (observed: openspec/specs/family/spec.md, Member Removal; deferred in docs/_legacy/01_system/system-spec.md).

## Rationale

Households change; the roster must reflect who still belongs.
