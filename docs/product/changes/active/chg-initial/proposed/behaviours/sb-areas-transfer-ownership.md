---
id: SB-AREAS-TRANSFER-OWNERSHIP
type: structured-behaviour
title: Transferring an Area hands ownership to another person
status: draft
illustrates:
- UC-AREAS-TRANSFER-OWNERSHIP
- BR-AREAS-SINGLE-OWNER
given:
- An Area has an Owner
- Another person of the household exists
when: The household transfers ownership to the other person
then:
- The other person becomes the Owner
- The previous Owner no longer holds the Owner role
- The Area remains active
uses-terms:
- TERM-RESPONSIBILITY-TRANSFER
- TERM-AREA-OWNER
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household transfers Area ownership)'
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes the explicit handover of accountability.

## Boundaries

Does not assert how the handover is shown to the household afterwards.
