---
id: TERM-RELATIONSHIP
type: domain-term
title: "Relationship"
status: draft
defined-in: BC-FAMILY
synonyms: []
uses-terms:
  - TERM-MEMBER
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Relationship Assignment, NOTE N5); docs/_legacy/04_contexts/family.md (Relationship, Dependency Semantics); docs/_legacy/04_contexts/family-member-management.md (Extended Relationship Types); docs/_legacy/01_system/system-spec.md (Deferred to V1.1); interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0024 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Definition

A structural link between two people of the same household that expresses kinship or care: parent to child, spouse to spouse, sibling to sibling, and a care relationship in which one person looks after another. Planned: V1.1 (observed: openspec/specs/family/spec.md, Purpose, Relationship Assignment, NOTE N5). A care relationship is a link between two people, not a person role: there is no Caregiver role (decided: Q-0021). Only managers manage relationships (decided: Q-0024).

## Distinguish From

- Not area ownership or task assignment: relationships express who depends on whom, which later rules may use to find responsible adults (observed: docs/_legacy/04_contexts/family.md, Relationship).
- Only Household and Members defines these semantics; other areas must not infer them (observed: family.md, Dependency Semantics).

## Usage

Planned: V1.1. The legacy roadmap would add grandparent, grandchild, extended family and service relation types (observed: family-member-management.md, Extended Relationship Types).
