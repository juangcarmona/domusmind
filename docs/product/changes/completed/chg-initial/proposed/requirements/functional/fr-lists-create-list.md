---
id: FR-LISTS-CREATE-LIST
type: functional-requirement
title: "Create a list from a name"
status: draft
derived-from:
  - UC-LISTS-CREATE-LIST
  - BR-LISTS-LIST-NAME-REQUIRED
  - BR-LISTS-CONTEXT-LINKS-INFORMATIONAL
verification:
  - scenario-ref: "SB-LISTS-CREATE-NAME-ONLY"
  - scenario-ref: "SB-LISTS-CREATE-LINKED-TO-PLAN"
uses-terms:
  - TERM-LIST
  - TERM-LIST-KIND
  - TERM-AREA
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Creation); docs/_legacy/00_product/surfaces/lists.md (Creation Model); interview: product owner decision Q-0029 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household create a list with a name as the only required input. It MUST allow optionally associating an Area, linking a Plan and choosing a kind at creation (a list created without a kind gets a generic default kind, decided: Q-0029), and MUST allow each of these to be set or changed later. A new list MUST start with no items.

## Rationale

Creating a list must be frictionless so capture is never blocked by metadata.
