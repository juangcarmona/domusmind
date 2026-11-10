---
id: FR-DOCUMENTS-VERSIONS-AND-LIFECYCLE
type: functional-requirement
title: Preserve document revisions and manage archive/delete explicitly
status: draft
derived-from:
  - UC-DOCUMENTS-MANAGE-ACCESS-AND-LIFECYCLE
  - BR-DOCUMENT-REPLACEMENT-PRESERVES-HISTORY
  - BR-DOCUMENT-EXPIRY-DOES-NOT-CREATE-WORK
verification:
  - scenario: "Replacing a file creates a distinct retrievable version without overwriting its prior bytes; renewing a document can create a superseding logical record."
  - scenario: "Archive, restore and delete are distinct actions and each follows the accepted retention policy; expiry creates no Task, Plan or Agenda event."
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-VERSION
---
## Requirement

The product distinguishes file replacement/version history, a new superseding document, archive/restore and deletion. Previous versions or documents remain retrievable to authorized members according to the accepted retention policy. Expiry metadata causes no automatic household action.

## Rationale

Households need to distinguish a corrected scan from a renewed policy, preserve evidence of past coverage, and intentionally control visibility and deletion.
