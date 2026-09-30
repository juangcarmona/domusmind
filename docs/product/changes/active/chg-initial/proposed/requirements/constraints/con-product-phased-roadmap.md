---
id: CON-PRODUCT-PHASED-ROADMAP
type: constraint
title: "The product evolves in additive phases: core, hardening, new domains, intelligence"
status: draft
uses-terms:
  - "TERM-HOUSEHOLD"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (Purpose, V1, V1.1, V2, V3, Planning Rules); decision Q-0004"
  confidence: "low"
  recovered-from: "documentation"
---

## Constraint

Planned: V1.1, V2, V3. DomusMind evolves in additive phases, each extending the product without weakening area ownership, cross-area isolation or household-facing clarity (observed: docs/_legacy/09_roadmap/roadmap.md, Purpose, Planning Rules):

- **V1 - household operating core**: household, people, areas of responsibility, plans and reminders, tasks, routines, shared lists and the Agenda (observed: docs/_legacy/09_roadmap/roadmap.md, V1).
- **V1.1 - operational hardening**: no new areas; reliability, validation completeness, security, projection quality, UX tightening, edge cases; may complete deferred capabilities such as relationships between people, removing a person and unfinished list lifecycle operations (observed: docs/_legacy/09_roadmap/roadmap.md, V1.1).
- **V2 - household domain expansion**: administration, documents, property and maintenance, inventory-aware household state and food or meal coordination, built on the V1 core without blurring time, execution, responsibility and list-based coordination (observed: docs/_legacy/09_roadmap/roadmap.md, V2).
- **V3 - intelligence and integrations**: messaging integrations, calendar synchronization, import and capture shortcuts, reminder routing, automation pipelines, projections and analytics, AI-assisted interpretation of household input; intelligence comes after clarity and automation must strengthen the model, not bypass it (observed: docs/_legacy/09_roadmap/roadmap.md, V3).

The roadmap is a single legacy document; everything beyond the current product is intent, not commitment (inferred).

## Rationale

A smaller coherent system is better than a larger ambiguous one; the core must be trustworthy before it grows (observed: docs/_legacy/09_roadmap/roadmap.md, Planning Rules 2, V1.1 Outcome).

## Consequences

- Every roadmap capability is modelled as a low-confidence draft derived from this constraint (decision Q-0004): the V2 domains BC-ADMINISTRATION, BC-DOCUMENTS, BC-PROPERTY and BC-INVENTORY with their use cases, and the V3 FR-ROADMAP-* requirements.
- Meal coordination, listed under V2, is already current product (decision Q-0001).
- Public messaging may describe future direction but must stay grounded in what exists now (observed: docs/_legacy/09_roadmap/roadmap.md, Planning Rule 4).
