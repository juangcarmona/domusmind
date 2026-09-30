---
id: "SB-MEALS-LOCKED-SLOT-REJECTED"
type: "structured-behaviour"
title: "Changing a locked slot without unlocking it is rejected"
status: "draft"
illustrates:
  - "UC-MEALS-UPDATE-SLOT"
  - "BR-MEALS-LOCKED-SLOT"
given:
  - "a slot is locked"
when: "a person submits a content change without unlocking the slot"
then:
  - "the change is rejected"
  - "the slot content is unchanged"
uses-terms:
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household attempts to mutate a locked slot)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Locking protects stable meals from accidental change.

## Boundaries

None.
