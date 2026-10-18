---
id: CON-PRODUCT-NON-GOALS
type: constraint
title: "What DomusMind deliberately is not"
status: draft
uses-terms:
  - "TERM-TASK"
  - "TERM-PLAN"
  - "TERM-LIST"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Non-Goals, Target Audience); docs/_legacy/00_product/experience.md (Scope Guardrails); product-owner decision for CHG-HOUSEHOLD-CHATS"
  confidence: medium
  recovered-from: documentation
---

## Constraint

DomusMind is not, and must not drift into (observed: docs/_legacy/00_product/strategy.md, Non-Goals; docs/_legacy/00_product/experience.md, Scope Guardrails):

- a generic task manager or generic productivity app;
- a traditional calendar application or a calendar-first product;
- a household-work tracker in isolation;
- a budget tool;
- a grocery app by itself;
- a general-purpose messaging product or a replacement for existing family messaging;
- a dashboard full of abstract metrics;
- an admin-heavy household manager.

Its promise is shared clarity, visible responsibility and a better operating model for the household (observed: docs/_legacy/00_product/strategy.md, Non-Goals). Household Chats exists to support that promise through household-scoped conversations and read-only access to household information.

## Rationale

The primary audience is couples and families where one person carries the household mental load; people who only want a personal calendar, a simple list tool or generic productivity are explicitly not the target (observed: docs/_legacy/00_product/strategy.md, Target Audience, Not the Target). The approved Chats capability supports household coordination without replacing general-purpose messaging.

## Consequences

- Features whose value is only personal productivity, finance or general-purpose messaging fall outside the product (inferred).
- Chats conversations remain household-scoped; external messaging integrations remain outside current product scope.
- Surfaces stay operational: no separate routine manager, no hidden work, no metric dashboards as default composition (observed: docs/_legacy/00_product/experience.md, Scope Guardrails).
- Lists stay reusable grouped memory rather than task tracking (observed: docs/_legacy/00_product/strategy.md, Differentiators); the Lists area enforces this with its own rules.
