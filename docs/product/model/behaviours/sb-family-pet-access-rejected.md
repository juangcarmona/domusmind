---
id: SB-FAMILY-PET-ACCESS-REJECTED
type: structured-behaviour
title: "A pet cannot be given access"
status: draft
illustrates:
  - BR-FAMILY-PET-RESTRICTIONS
given:
  - "A household has a pet"
when: "Access is provisioned for the pet"
then:
  - "The operation is rejected"
uses-terms:
  - TERM-PET
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning, scenario \"Provisioning a Pet is rejected\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes pets never sign in.

## Boundaries

None.
