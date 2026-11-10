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
  - "BC-HOUSEHOLD-FINANCES"
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-MEMBER"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Mission, Category, Positioning); docs/_legacy/00_product/experience.md (Core Experience Principles, Summary); docs/_legacy/03_domain/ubiquitous-language.md (Family)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Constraint

DomusMind models and presents the household as one shared system made of people, time, responsibilities and ongoing work. The household, not an individual, is the unit the product is built around: every area of the product describes shared household reality, and no surface is designed as one person's private productivity space. The household is also the boundary for who belongs and the default scope of its shared state. Sensitive financial records may apply explicit member-level access rules within that household; household-first does not mean universal visibility of every amount.

## Rationale

The mission is to remove the invisible mental load of running a household so that the home no longer depends on one person remembering everything; household financial commitments and costs contribute to that shared reality without erasing legitimate privacy boundaries.

## Consequences

- Plans, tasks, routines, lists, areas, meal plans and financial commitments are organized in household scope; shared views come first where the approved privacy policy allows.
- Person-centric views and private financial records exist inside the household system, not as separate personal finance products.
- Financial access is checked at the record and projection boundary; the interface cannot rely on hiding content alone.
- Proposals that turn DomusMind into a personal task, calendar or accounting product conflict with this constraint (see CON-PRODUCT-NON-GOALS).
