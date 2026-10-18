---
id: CON-PRODUCT-PHASED-ROADMAP
type: constraint
title: "The product evolves in additive phases: core, hardening, new domains, intelligence"
status: draft
uses-terms:
  - "TERM-HOUSEHOLD"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (Purpose, V1, V1.1, V2, V3, Planning Rules); decision Q-0004; GitHub issue #18"
  confidence: low
  recovered-from: documentation
---
## Constraint

DomusMind evolves additively, extending the product without weakening area ownership, cross-area isolation or household-facing clarity.

- **V1 - household operating core**: household, people, areas of responsibility, plans and reminders, tasks, routines, shared lists and the Agenda.
- **V1.1 - operational hardening**: reliability, validation completeness, security, projection quality, UX tightening and deferred membership and list lifecycle capabilities.
- **V2 - household domain expansion**: administration, documents, property and maintenance, inventory-aware household state and food or meal coordination, built on the V1 core without blurring time, execution, responsibility and list-based coordination.
- **V3 - intelligence and integrations**: external messaging integrations, calendar synchronization, import and capture shortcuts, reminder routing, automation pipelines, projections and analytics. Intelligence comes after clarity; automation must strengthen the model, not bypass it.

Review-first Inbox interpretation and the explicitly invoked Chats assistant are current product. The conversational assistant may stage supported Plan and List Item create/update proposals in Inbox, and households may configure ordered assistant provider targets, without direct domain writes (CHG-CONVERSATIONAL-ASSISTANT).

## Rationale

A smaller coherent system is better than a larger ambiguous one; the core must be trustworthy before it grows. Assistant capability follows the same ownership and review rules as other household actions.

## Consequences

- Roadmap items remain future intent until refined and accepted through a Product Change.
- Meal coordination, listed under V2, is already current product.
- Assistant action proposals require explicit Inbox review and do not make autonomous execution current scope.
- Public messaging must stay grounded in what exists now.
