---
id: TERM-HOUSEHOLD
type: domain-term
title: "Household"
status: draft
defined-in: BC-FAMILY
synonyms:
  - Family
uses-terms: []
provenance:
  source: "openspec/specs/family/spec.md (Purpose, Household Creation, Invariants); docs/_legacy/04_contexts/family.md (Purpose, Ubiquitous Language Notes); src/backend/DomusMind.Domain/Family/Family.cs"
  confidence: high
  recovered-from: documentation
---

## Definition

The home that DomusMind coordinates: the root unit of identity, permissions and visibility. Everything else in the product belongs to exactly one household. A household has a name and a stable identity that never changes, and it owns its people, pets and (once shipped) relationships (observed: openspec/specs/family/spec.md, Purpose and Household Creation; docs/_legacy/03_domain/ubiquitous-language.md, Family).

## Distinguish From

- Not a sign-in account or tenant: people sign in individually and are linked to the household (observed: openspec family spec, NOTE N7).
- "Family" is the legacy and code name for the same concept; the product says Household.

## Usage

Every plan, task, list, area and meal plan is scoped to one household. The household is created by name and then filled with people (observed: openspec family spec, Household Creation, Member Addition).
