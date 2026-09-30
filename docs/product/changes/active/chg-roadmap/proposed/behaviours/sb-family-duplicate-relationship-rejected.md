---
id: SB-FAMILY-DUPLICATE-RELATIONSHIP-REJECTED
type: structured-behaviour
title: "A duplicate relationship is rejected"
status: draft
illustrates:
  - BR-FAMILY-RELATIONSHIP-INTEGRITY
given:
  - "A relationship of a given type already exists between two people"
when: "The same relationship type is submitted for the same pair"
then:
  - "The relationship is rejected"
uses-terms:
  - TERM-RELATIONSHIP
provenance:
  source: "openspec/specs/family/spec.md (Relationship Assignment (V1.1), scenario \"Duplicate relationship is rejected\")"
  confidence: low
  recovered-from: documentation
---

## Intent

Planned: V1.1.

## Boundaries

Different relationship types between the same pair are not covered.
