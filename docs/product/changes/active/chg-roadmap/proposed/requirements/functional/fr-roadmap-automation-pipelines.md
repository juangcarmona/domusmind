---
id: FR-ROADMAP-AUTOMATION-PIPELINES
type: functional-requirement
title: "Automations that strengthen the household model"
status: draft
derived-from:
  - "CON-PRODUCT-PHASED-ROADMAP"
verification:
  - scenario: "An automation proposes a routine's next step and a person accepts it before anything is created (inferred)."
uses-terms:
  - "TERM-ROUTINE"
  - "TERM-TASK"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V3 - Intelligence and Integrations); decision Q-0004"
  confidence: "low"
  recovered-from: "documentation"
---

## Requirement

Planned: V3. The product MAY offer automation pipelines over household state (observed: docs/_legacy/09_roadmap/roadmap.md, V3 "automation pipelines"). Any automation MUST strengthen the existing model rather than bypass it, and MUST respect area ownership (observed: docs/_legacy/09_roadmap/roadmap.md, V3 Rule), which with CON-PRODUCT-NO-MAGIC-AUTOMATION means no household record appears without a person's decision (inferred).

## Rationale

V3 aims to reduce friction further by making capture faster, interpretation smarter and coordination more anticipatory; intelligence comes after clarity and automation must strengthen the model, not bypass it (observed: docs/_legacy/09_roadmap/roadmap.md, V3 Outcome, Rule). See CON-PRODUCT-NO-MAGIC-AUTOMATION.
