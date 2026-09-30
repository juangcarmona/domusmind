---
id: BC-DOCUMENTS
type: bounded-context
title: "Household Documents"
status: draft
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "documentation"
---

## Responsibility

Planned: V2. Tracking the household's important documents, such as passports, insurance or school papers, and where they are (observed: docs/_legacy/09_roadmap/roadmap.md, V2 candidate area "documents", possible capability "important document tracking").

## Language

No vocabulary is defined yet; "Document as a standalone bounded-context concept" is a term to avoid until scope changes (observed: docs/_legacy/03_domain/ubiquitous-language.md, Terms to Avoid; docs/_legacy/01_system/system-spec.md, Out of Scope for V1 "Documents as a standalone context").

## Boundaries

- Must build on the V1 core and must not blur time, execution, responsibility and list-based coordination: dates would show through the Agenda, work would be tasks, ownership would be Areas (observed: docs/_legacy/09_roadmap/roadmap.md, V2 Rule; inferred mapping).
- Not part of the current product (see CON-PRODUCT-CURRENT-SCOPE).

## External Relationships

- Would depend on the household and its people; expiry dates could feed Household Administration's renewal visibility (inferred).
