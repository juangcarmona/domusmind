---
id: BC-ADMINISTRATION
type: bounded-context
title: "Household Administration"
status: draft
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "documentation"
---

## Responsibility

Planned: V2. Household administration: the recurring obligations of running a home, such as renewals and deadlines, made visible before they are missed (observed: docs/_legacy/09_roadmap/roadmap.md, V2 candidate area "administration", possible capability "renewal and deadline visibility").

## Language

No vocabulary is defined yet. The legacy vocabulary names "administration" only as an example Area of responsibility (observed: docs/_legacy/03_domain/ubiquitous-language.md, Responsibility Domain) and lists "Contract" among terms to avoid until scope changes (observed: docs/_legacy/03_domain/ubiquitous-language.md, Terms to Avoid).

## Boundaries

- Must build on the V1 core and must not blur time, execution, responsibility and list-based coordination: dates would show through the Agenda, work would be tasks, ownership would be Areas (observed: docs/_legacy/09_roadmap/roadmap.md, V2 Rule; inferred mapping).
- Not part of the current product (see CON-PRODUCT-CURRENT-SCOPE).

## External Relationships

- Would depend on the household and its people for identity and on Areas for ownership (inferred from the V1 dependency pattern, system-spec.md, Context Dependencies).
- Deadlines would appear in the Agenda as a read concern (inferred).
