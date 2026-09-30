---
id: SB-AREAS-REPLACE-OWNER
type: structured-behaviour
title: Assigning a new Owner replaces the previous one
status: draft
illustrates:
- UC-AREAS-ASSIGN-OWNER
- UC-AREAS-TRANSFER-OWNERSHIP
- BR-AREAS-SINGLE-OWNER
given:
- An Area already has an Owner
when: The household assigns a different person as Owner
then:
- The new person becomes the Owner
- The previous Owner no longer holds that role
uses-terms:
- TERM-AREA-OWNER
provenance:
  source: "openspec/specs/areas/spec.md (scenario: Primary owner is replaced); interview: product owner decision Q-0012 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Pins that an Area never has two Owners.

## Boundaries

Replacing an existing Owner is a Responsibility Transfer (decided: Q-0012); this example does not assert how the handover is traced.
