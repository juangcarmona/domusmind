---
id: BC-PROPERTY
type: bounded-context
title: "Property and Maintenance"
status: draft
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "documentation"
---

## Responsibility

Planned: V2. The home itself: planning its upkeep and maintenance (observed: docs/_legacy/09_roadmap/roadmap.md, V2 candidate area "property / maintenance", possible capability "property maintenance planning").

## Language

No vocabulary is defined yet; "Property" and "Asset" are terms to avoid until scope changes (observed: docs/_legacy/03_domain/ubiquitous-language.md, Terms to Avoid). "maintenance" appears today only as an example Area (observed: docs/_legacy/03_domain/ubiquitous-language.md, Responsibility Domain).

## Boundaries

- Must build on the V1 core and must not blur time, execution, responsibility and list-based coordination: dates would show through the Agenda, work would be tasks, ownership would be Areas (observed: docs/_legacy/09_roadmap/roadmap.md, V2 Rule; inferred mapping).
- Not part of the current product (see CON-PRODUCT-CURRENT-SCOPE).

## External Relationships

- Maintenance work would be tasks or routines and its owner an Area owner, keeping execution and ownership where they are (inferred from roadmap.md, V2 Rule).
