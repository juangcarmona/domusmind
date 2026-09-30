---
id: FR-ROADMAP-AI-ASSISTED-INTERPRETATION
type: functional-requirement
title: "AI-assisted interpretation of household input"
status: draft
derived-from:
  - "CON-PRODUCT-PHASED-ROADMAP"
verification:
  - scenario: "A person types \"dentist for Hugo Tuesday 5pm\" and DomusMind proposes a plan with Hugo as participant, which the person confirms (inferred)."
uses-terms:
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-LIST-ITEM"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V3 - Intelligence and Integrations); decision Q-0004; docs/_legacy/00_product/strategy.md (Vision)"
  confidence: "low"
  recovered-from: "documentation"
---

## Requirement

Planned: V3. The product MAY interpret household input with AI assistance, for example turning free text into a proposed plan, task or list item, and MAY make useful suggestions (observed: docs/_legacy/09_roadmap/roadmap.md, V3 "AI-assisted interpretation of household input"; docs/_legacy/00_product/strategy.md, Vision "anticipation and useful suggestions"; examples inferred). Interpretation MUST grow from clear household structure and MUST leave the decision to a person (observed: docs/_legacy/00_product/strategy.md, Vision; see CON-PRODUCT-NO-MAGIC-AUTOMATION).

## Rationale

V3 aims to reduce friction further by making capture faster, interpretation smarter and coordination more anticipatory; intelligence comes after clarity and automation must strengthen the model, not bypass it (observed: docs/_legacy/09_roadmap/roadmap.md, V3 Outcome, Rule). See CON-PRODUCT-NO-MAGIC-AUTOMATION.
