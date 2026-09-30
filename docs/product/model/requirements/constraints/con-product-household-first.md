---
id: CON-PRODUCT-HOUSEHOLD-FIRST
type: constraint
title: "DomusMind is household-first, not person-first"
status: draft
applies-to:
  - "BC-FAMILY"
  - "BC-RESPONSIBILITIES"
  - "BC-CALENDAR"
  - "BC-TASKS"
  - "BC-LISTS"
  - "BC-MEAL-PLANNING"
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-MEMBER"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Mission, Category, Positioning); docs/_legacy/00_product/experience.md (Core Experience Principles, Summary); docs/_legacy/03_domain/ubiquitous-language.md (Family)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Constraint

DomusMind models and presents the household as one shared system made of people, time, responsibilities and ongoing work. The household, not an individual, is the unit the product is built around: every area of the product describes shared household reality, and no surface is designed as one person's private productivity space (observed: docs/_legacy/00_product/strategy.md, Category, Mission; docs/_legacy/00_product/experience.md, Core Experience Principles "Household-first").

The household is also the boundary for who belongs, what they can see and the data each area holds (observed: docs/_legacy/03_domain/ubiquitous-language.md, Family).

## Rationale

The mission is to remove the invisible mental load of running a household so that the home no longer depends on one person remembering everything; the goal is explicitly not better personal productivity (observed: docs/_legacy/00_product/strategy.md, Mission).

## Consequences

- Plans, tasks, routines, lists, areas and meal plans belong to a household, and shared views (the household Agenda, the Areas surface, lists) come first (inferred from docs/_legacy/00_product/strategy.md, Positioning; docs/_legacy/00_product/experience.md, Agenda default entry).
- Person-centric views, such as a person's Agenda scope, exist inside the household system rather than as a separate personal product (observed: docs/_legacy/00_product/experience.md, Surface Roles, Agenda).
- Proposals that turn DomusMind into a personal task or calendar tool conflict with this constraint (see CON-PRODUCT-NON-GOALS).
