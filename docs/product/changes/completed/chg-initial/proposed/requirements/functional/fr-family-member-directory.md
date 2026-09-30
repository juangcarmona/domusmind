---
id: FR-FAMILY-MEMBER-DIRECTORY
type: functional-requirement
title: "Show the household's people directory"
status: draft
derived-from:
  - UC-FAMILY-VIEW-MEMBER-DIRECTORY
  - BR-FAMILY-DIRECTORY-ORDER
verification:
  - scenario: "A household member opens the directory and sees adults, then children, then pets, managers first in each group and then by name"
  - scenario: "A non-manager viewing the directory is not offered to grant access to anyone"
  - scenario: "A manager viewing the directory is offered to grant access only to people without an account who are not pets"
uses-terms:
  - TERM-MEMBER-DIRECTORY
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Household Member Directory); docs/_legacy/04_contexts/family-member-management.md (MemberDirectoryItemResponse, Sort order); interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let every household member view the household's directory in the defined order, and MUST show with each entry what the viewer may do (edit, grant access) and the person's access state, as decided by the product rather than worked out by the viewer's device (observed: openspec/specs/family/spec.md, Household Member Directory; family-member-management.md, MemberDirectoryItemResponse).

## Rationale

Everyone needs to know who is in the household; managers need to see where action is needed.
