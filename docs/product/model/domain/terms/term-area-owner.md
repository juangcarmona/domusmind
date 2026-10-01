---
id: TERM-AREA-OWNER
type: domain-term
title: Owner
status: draft
defined-in: BC-RESPONSIBILITIES
synonyms:
- Primary Owner
uses-terms:
- TERM-AREA
- TERM-MEMBER
provenance:
  source: openspec/specs/areas/spec.md (Purpose, Primary Owner Assignment); docs/_legacy/04_contexts/responsibilities.md (Ubiquitous Language Notes); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs
  confidence: high
  recovered-from: documentation
---

## Definition

The one person of the household who is accountable for an Area. An Area has at most one Owner at any time (observed: openspec/specs/areas/spec.md, Primary Owner Assignment; docs/_legacy/04_contexts/responsibilities.md, Ubiquitous Language Notes). Household-facing label: Owner; domain label: Primary Owner (observed: openspec/specs/areas/spec.md, Purpose).

## Distinguish From

- Support: backup or shared accountability; Support never replaces the Owner (observed: openspec/specs/areas/spec.md, Secondary Owner Assignment).
- Task assignee: the person doing a task. Owning an Area does not make someone the assignee of its tasks, although the Areas surface pre-fills the Owner as assignee when a task is created from an Area (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector).
- Household Manager: an administrative role in Family, unrelated to Area ownership.

## Usage

Shown on each Area row, or replaced by a gap indicator when missing (observed: docs/_legacy/00_product/surfaces/areas.md, Default View; openspec/specs/areas/spec.md, Ownership Visibility).
