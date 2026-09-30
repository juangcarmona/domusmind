---
id: SB-AREAS-UNOWNED-FIRST
type: structured-behaviour
title: Unowned Areas are listed first
status: draft
illustrates:
- UC-AREAS-REVIEW-OWNERSHIP
given:
- A household has both owned and unowned Areas
when: A person views the Areas surface
then:
- Unowned Areas appear before owned Areas in the default view
uses-terms:
- TERM-OWNERSHIP-GAP
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Unowned Area appears at the top of the list)'
  confidence: high
  recovered-from: documentation
---

## Intent

Keeps accountability gaps in view without searching for them.

## Boundaries

Does not assert the order within a group.
