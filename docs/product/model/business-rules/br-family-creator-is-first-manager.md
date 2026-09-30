---
id: BR-FAMILY-CREATOR-IS-FIRST-MANAGER
type: business-rule
title: "The person who creates the household is its first manager"
status: draft
applies-to:
  - UC-FAMILY-CREATE-HOUSEHOLD
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-MANAGER
provenance:
  source: "interview: product owner decision Q-0022 (E-0158); openspec/specs/family/spec.md (NOTE N4)"
  confidence: high
  recovered-from: interview
---

## Rule

The person who creates a household becomes its first manager (decided: Q-0022). The specs had left the designation of the first manager unspecified (observed: openspec/specs/family/spec.md, NOTE N4).

## Rationale

A new household needs someone with authority to add its people and configure it, since only managers do so (decided: Q-0024, Q-0025).

## Examples

- Ana creates the household "Casa García": Ana is the household's manager.

## Exceptions

None stated.
