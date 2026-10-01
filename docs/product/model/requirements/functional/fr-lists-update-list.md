---
id: FR-LISTS-UPDATE-LIST
type: functional-requirement
title: "Update a list's name and associations"
status: draft
derived-from:
  - UC-LISTS-UPDATE-LIST
  - BR-LISTS-CONTEXT-LINKS-INFORMATIONAL
verification:
  - scenario-ref: "SB-LISTS-RENAME"
  - scenario-ref: "SB-LISTS-CLEAR-PLAN-LINK"
uses-terms:
  - TERM-LIST
  - TERM-LIST-KIND
  - TERM-AREA
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Update)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household update a list's name, Area association, linked Plan and kind. At least one field MUST be provided; fields not provided MUST stay unchanged. The Area association and the plan link MUST be clearable explicitly.

## Rationale

Lists outlive the context they were created in; their details must be adjustable.
