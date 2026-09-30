---
id: SB-AREAS-REJECT-OWNER-AS-SUPPORT
type: structured-behaviour
title: The Owner cannot be added as Support
status: draft
illustrates:
- UC-AREAS-ADD-SUPPORT
- BR-AREAS-OWNER-NOT-SUPPORT
given:
- Ana is the Owner of an Area
when: The household adds Ana as Support of that Area
then:
- The addition is rejected
- Ana remains the Owner and is not listed as Support
uses-terms:
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: "docs/_legacy/04_contexts/responsibilities.md (Role Consistency); interview: product owner decision Q-0017 (E-0158)"
  confidence: high
  recovered-from: interview
---

## Intent

Owner and Support stay distinct roles on an Area (decided: Q-0017).

## Boundaries

Does not cover a Support person becoming Owner, which removes them from Support.
