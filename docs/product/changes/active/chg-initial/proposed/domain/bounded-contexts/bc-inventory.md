---
id: BC-INVENTORY
type: bounded-context
title: "Household Inventory"
status: draft
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004; docs/_legacy/00_product/experience.md (Lists)"
  confidence: "low"
  recovered-from: "documentation"
---

## Responsibility

Planned: V2. Inventory-aware household state: richer awareness of what the household has in stock, including pantry and supply state (observed: docs/_legacy/09_roadmap/roadmap.md, V2 candidate area "inventory-aware household state", possible capabilities "richer household stock awareness", "pantry or supply-state modeling").

## Language

No vocabulary is defined yet; "Inventory" is a term to avoid until scope changes (observed: docs/_legacy/03_domain/ubiquitous-language.md, Terms to Avoid). Today supply needs are captured as list items (observed: experience.md, Lists "restocking").

## Boundaries

- Must build on the V1 core and must not blur time, execution, responsibility and list-based coordination: dates would show through the Agenda, work would be tasks, ownership would be Areas (observed: docs/_legacy/09_roadmap/roadmap.md, V2 Rule; inferred mapping).
- Not part of the current product (see CON-PRODUCT-CURRENT-SCOPE).

## External Relationships

- Would need a clear line against Lists, which today hold restocking and shopping lists, and against Meal Planning's shopping-list derivation (inferred; open question).
