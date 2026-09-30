---
id: FR-AREAS-DEFAULT-AREAS
type: functional-requirement
title: Start a new household with default Areas
status: draft
derived-from:
- UC-AREAS-CREATE-AREA
verification:
- scenario: A newly created household opens Areas and already sees a starter set of unowned Areas
uses-terms:
- TERM-AREA
- TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/areas/spec.md (Area Creation); docs/_legacy/04_contexts/responsibilities.md (Domain Events Consumed); interview: product owner decision Q-0015 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Requirement

Planned (future): no default Areas for now (decided: Q-0015); a new household starts with no Areas. A later version MAY create a set of default Areas when a household is created, so it begins with useful Areas without admin-heavy setup.

## Rationale

Low-friction start (observed: openspec/specs/areas/spec.md, Area Creation, "may be bootstrapped"; docs/_legacy/04_contexts/responsibilities.md, Domain Events Consumed). Which Areas would form the default set is not specified.
