---
id: FR-DOCUMENTS-SEARCH
type: functional-requirement
title: Search and filter permitted household documents
status: draft
derived-from:
  - UC-DOCUMENTS-FIND-AND-RETRIEVE
  - BR-DOCUMENT-ACCESS-INTERSECTION
verification:
  - scenario: "A member finds a permitted Document by title and supported metadata or filters; inaccessible Documents and their metadata are absent from all result and count shapes."
uses-terms:
  - TERM-DOCUMENT
---
## Requirement

Members can search accessible documents by approved descriptive metadata and filter by category, associated context/member, lifecycle and expiration state. File-content indexing and sensitive reference-number indexing are disabled unless separately approved with access-aware design. Search prioritizes retrieval and does not require manual folder navigation.

## Rationale

The household problem is remembering where paperwork lives. Search and contextual retrieval solve that more directly than deep folder trees or complex taxonomy.
