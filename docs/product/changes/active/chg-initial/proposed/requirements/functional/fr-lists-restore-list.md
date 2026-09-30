---
id: FR-LISTS-RESTORE-LIST
type: functional-requirement
title: "Restore an archived list unchanged"
status: draft
derived-from:
  - UC-LISTS-RESTORE-LIST
  - BR-LISTS-ARCHIVE-STATE
verification:
  - scenario-ref: "SB-LISTS-RESTORE"
  - scenario: "Restoring a list that is not archived is rejected."
uses-terms:
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Restore); src/backend/DomusMind.Domain/Lists/SharedList.cs (Restore)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household restore an archived list to active use with no item data modified; the restored list MUST behave like any other active list. Restoring a list that is not archived MUST be rejected.

## Rationale

Reuse matters more than one-time completion.
