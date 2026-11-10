---
id: FR-DOCUMENTS-CONTEXTUAL-ASSOCIATIONS
type: functional-requirement
title: Associate documents with authorized household context entities
status: draft
derived-from:
  - UC-DOCUMENTS-ASSOCIATE-WITH-CONTEXT
  - BR-DOCUMENT-ASSOCIATION-DOES-NOT-TRANSFER-MEANING
  - BR-DOCUMENT-ACCESS-INTERSECTION
verification:
  - scenario: "A same-household supported entity can link to a Document without changing the target, while cross-household, unsupported and unauthorized links are rejected."
  - scenario: "A target surface never exposes a restricted Document to a member who lacks Document access."
uses-terms:
  - TERM-DOCUMENT-ASSOCIATION
---
## Requirement

Members can create and remove typed Document associations only to supported, same-household entities validated by the owning context contract. An association does not duplicate target data, transfer business meaning, create work or grant access. Initial target types are limited to approved Household Member and existing context records; Asset support is gated by alignment of the separate Household Assets model. Finance and School contexts are not required.

## Rationale

Contextual attachment makes paperwork retrievable where it matters while preserving one authoritative owner for every household record and permission decision.
