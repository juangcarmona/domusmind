---
id: TERM-AREA-PARTICIPANT
type: domain-term
title: Area Participant
status: draft
defined-in: BC-RESPONSIBILITIES
synonyms:
- Participant
- Collaborator
uses-terms:
- TERM-AREA
provenance:
  source: "docs/_legacy/04_contexts/responsibilities.md (Internal Entities, Commands, Ubiquitous Language Notes); openspec/specs/areas/spec.md (Notes, Participant role); interview: product owner decision Q-0016 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Definition

A person of the household involved in an Area but not accountable for it (observed: docs/_legacy/04_contexts/responsibilities.md, Internal Entities and Ubiquitous Language Notes).

## Distinguish From

- Owner and Support: both carry accountability; a Participant does not (observed: docs/_legacy/04_contexts/responsibilities.md).
- Plan participant: a person taking part in a plan in Calendar, a different concept (inferred from docs/_legacy/03_domain/ubiquitous-language.md, which lists participation in plans separately).

## Usage

Conflicting status. The legacy context document defines participants and commands to add and remove them (observed: docs/_legacy/04_contexts/responsibilities.md, Commands). The Areas spec deliberately excludes participants until a feature spec exists (observed: openspec/specs/areas/spec.md, Notes, Participant role), the surface keeps participant detail out of the default row (observed: docs/_legacy/00_product/surfaces/areas.md, Default View), and the domain code has no participants (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs). Planned (future), low confidence: Area participants are not part of the current product (decided: Q-0016).
