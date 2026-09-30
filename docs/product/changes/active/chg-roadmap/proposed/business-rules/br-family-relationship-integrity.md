---
id: BR-FAMILY-RELATIONSHIP-INTEGRITY
type: business-rule
title: "Relationships link two distinct existing people, once per type"
status: draft
applies-to:
  - UC-FAMILY-MANAGE-RELATIONSHIPS
uses-terms:
  - TERM-RELATIONSHIP
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Relationship Assignment, Invariants); docs/_legacy/04_contexts/family.md (Structural Integrity, Valid State Rules); docs/_legacy/01_system/system-spec.md (Deferred to V1.1)"
  confidence: low
  recovered-from: documentation
---

## Rule

Planned: V1.1. A relationship must link two different people who both belong to the same household; the same relationship type cannot be recorded twice for the same pair; a removed person cannot take part in new relationships (observed: openspec/specs/family/spec.md, Relationship Assignment, Invariants).

## Rationale

Relationships describe real care and kinship structures that later rules depend on (observed: docs/_legacy/04_contexts/family.md, Relationship).

## Examples

- Ana is parent of Diego: allowed.
- Ana is parent of Diego a second time: rejected.
- Ana is spouse of Ana: rejected.

## Exceptions

The legacy text allows duplicates "unless explicitly modeled" (observed: family.md, Valid State Rules); the spec has no exception.
