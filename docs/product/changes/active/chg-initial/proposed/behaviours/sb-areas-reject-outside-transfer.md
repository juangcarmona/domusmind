---
id: SB-AREAS-REJECT-OUTSIDE-TRANSFER
type: structured-behaviour
title: An Area cannot be transferred outside the household
status: draft
illustrates:
- UC-AREAS-TRANSFER-OWNERSHIP
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
given:
- An Area has an Owner
- A person does not belong to the household
when: The household attempts to transfer ownership to that person
then:
- The transfer is rejected
- The existing Owner is unchanged
uses-terms:
- TERM-RESPONSIBILITY-TRANSFER
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Transfer to a non-family member is rejected)'
  confidence: high
  recovered-from: documentation
---

## Intent

Keeps accountability inside the household during a transfer.

## Boundaries

None.
