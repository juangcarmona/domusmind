---
id: FR-LISTS-BROWSE-LISTS
type: functional-requirement
title: "Show the household's active lists with unchecked counts"
status: draft
derived-from:
  - UC-LISTS-BROWSE-LISTS
verification:
  - scenario-ref: "SB-LISTS-BROWSE-ACTIVE"
  - scenario-ref: "SB-LISTS-BROWSE-EXCLUDES-ARCHIVED"
uses-terms:
  - TERM-LIST
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Retrieval)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show a summary of every active list of the household including its name and number of unchecked items. Archived lists MUST be excluded by default.

## Rationale

People need to see which lists have remaining work before opening any.
