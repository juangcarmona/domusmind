---
id: TERM-AREA
type: domain-term
title: Area
status: draft
defined-in: BC-RESPONSIBILITIES
synonyms:
- Responsibility Domain
- Responsibility
uses-terms:
- TERM-HOUSEHOLD
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: "openspec/specs/areas/spec.md (Purpose, Notes); docs/_legacy/04_contexts/responsibilities.md (Aggregate Roots, Ubiquitous Language Notes); docs/_legacy/00_product/surfaces/areas.md; src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs; interview: product owner decision Q-0040 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

A named domain of household life, such as school, food, finances, maintenance, administration, pets, travel or logistics, for which accountability is made explicit. An Area belongs to exactly one household and may have an Owner and any number of Support people (observed: openspec/specs/areas/spec.md, Purpose; docs/_legacy/04_contexts/responsibilities.md, Aggregate Roots). In the domain model it is a Responsibility Domain; in household-facing language it is an Area (observed: openspec/specs/areas/spec.md, Notes, Terminology convergence).

## Distinguish From

- Not a category or tag: referencing an Area from a task, plan, routine or list records organisational context but does not make the Area a label; ownership semantics belong only to Areas (observed: openspec/specs/areas/spec.md, Cross-Context Referencing; docs/_legacy/04_contexts/responsibilities.md, Ubiquitous Language Notes).
- Not a unit of execution: owning an Area does not mean executing every related task or plan (observed: openspec/specs/areas/spec.md, Purpose).
- Not the Household Manager role or any permission scope (observed: docs/_legacy/00_product/surfaces/areas.md, Role and Non-Goals).

## Usage

The Areas surface lists a household's Areas with their Owner, Support people and ownership gaps (observed: docs/_legacy/00_product/surfaces/areas.md, Default View). Tasks, plans, routines and lists may optionally reference an Area (meal plans do not, decided: Q-0040); sparse usage is acceptable (observed: openspec/specs/areas/spec.md, Area Creation). An Area has a name (required, at most 100 characters) and a colour cue (observed: src/backend/DomusMind.Domain/Responsibilities/ValueObjects/ResponsibilityAreaName.cs; src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs; docs/_legacy/00_product/surfaces/areas.md, Default View).
