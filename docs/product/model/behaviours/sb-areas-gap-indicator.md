---
id: SB-AREAS-GAP-INDICATOR
type: structured-behaviour
title: A missing Owner is shown as a gap
status: draft
illustrates:
- UC-AREAS-REVIEW-OWNERSHIP
given:
- An Area has no Owner
when: A person views the Areas list
then:
- A visible gap indicator is shown in place of the Owner
uses-terms:
- TERM-OWNERSHIP-GAP
- TERM-AREA-OWNER
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Ownership gap is indicated on the Area row)'
  confidence: high
  recovered-from: documentation
---

## Intent

Makes missing ownership obvious on the row itself.

## Boundaries

Does not prescribe the visual form of the indicator.
