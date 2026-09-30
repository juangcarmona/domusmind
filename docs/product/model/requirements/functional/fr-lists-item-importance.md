---
id: FR-LISTS-ITEM-IMPORTANCE
type: functional-requirement
title: "Mark items as important"
status: draft
derived-from:
  - UC-LISTS-SET-ITEM-IMPORTANCE
  - BR-LISTS-IMPORTANCE-BINARY
verification:
  - scenario-ref: "SB-LISTS-STAR-ITEM"
  - scenario-ref: "SB-LISTS-STAR-IDEMPOTENT"
uses-terms:
  - TERM-ITEM-IMPORTANCE
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Importance)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household mark or unmark any item as important, as a binary flag. Setting the current value MUST succeed without change. Importance MUST NOT affect temporal fields, checked state or Agenda eligibility.

## Rationale

Some items need attention without becoming tasks.
