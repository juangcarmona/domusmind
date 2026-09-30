---
id: FR-LISTS-ARCHIVE-LIST
type: functional-requirement
title: "Archive a list without losing data"
status: draft
derived-from:
  - UC-LISTS-ARCHIVE-LIST
  - BR-LISTS-ARCHIVE-STATE
  - BR-LISTS-ARCHIVED-READ-ONLY
verification:
  - scenario-ref: "SB-LISTS-ARCHIVE"
  - scenario-ref: "SB-LISTS-ARCHIVE-TWICE-REJECTED"
  - scenario-ref: "SB-LISTS-ARCHIVED-ADD-REJECTED"
uses-terms:
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Archive); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household archive an active list, removing it from the active collection while preserving every item's name, quantity, note and checked state, and without effect on linked Areas or Plans. Archiving an already archived list MUST be rejected. While archived, the list MUST be read-only: adding, checking or editing its items MUST be rejected until it is restored (decided: Q-0031).

## Rationale

Households put lists away between uses without losing them.
