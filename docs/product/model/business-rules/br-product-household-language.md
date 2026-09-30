---
id: BR-PRODUCT-HOUSEHOLD-LANGUAGE
type: business-rule
title: "Household-facing text uses household words, not model words"
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
  - "TERM-PLAN"
  - "TERM-AREA"
  - "TERM-LIST"
  - "TERM-TASK"
  - "TERM-ROUTINE"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Language Rules); docs/_legacy/00_product/experience.md (Household Language, Core Experience Principles); docs/_legacy/03_domain/ubiquitous-language.md (Language Layers, Consistency Rules, Terms to Avoid); docs/_legacy/00_product/public-site.md (Copy Guardrails)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Everything a household sees (product surfaces and public copy) uses natural household language. Where the internal model term differs, the household term is used (observed: docs/_legacy/00_product/strategy.md, Language Rules; docs/_legacy/00_product/experience.md, Household Language; docs/_legacy/03_domain/ubiquitous-language.md, Language Layers, Consistency Rules):

| Internal model | Household language |
|---|---|
| Family | Household |
| Member | Person |
| Event | Plan |
| Responsibility (Responsibility Domain) | Area |
| Shared List | List |

Task stays Task and Routine stays Routine by design. Preferred words include household, people, plans, routines, tasks, lists, areas, today, this week, what matters, what needs attention and who owns what. Household-facing text avoids architecture words such as operational state, bounded context, domain model, coordination infrastructure, execution layer, accountability model, aggregate and projection (observed: docs/_legacy/00_product/strategy.md, Language Rules).

"Chore" is not used as a separate concept; "Routine" is used only for recurring household work, and a list item is never called a task (observed: docs/_legacy/03_domain/ubiquitous-language.md, Consistency Rules).

## Rationale

The product must stay close to lived household reality; one product, one language across every surface (observed: docs/_legacy/00_product/strategy.md, Language Rules; docs/_legacy/00_product/experience.md, Core Experience Principles "One product, one language").

## Examples

- A scheduled dentist appointment is shown as a Plan, never as an Event.
- The Areas surface says "Area" and "Owner", not "Responsibility Domain" or "Primary Owner assignment".
- A family is called a household on every screen.

## Exceptions

Internal documents keep their internal names; the domain term artifacts record both names as synonyms (observed: docs/_legacy/03_domain/ubiquitous-language.md, Language Layers). The strategy table omits Shared List to List, which the experience document adds (observed: docs/_legacy/00_product/strategy.md vs docs/_legacy/00_product/experience.md); both apply.
