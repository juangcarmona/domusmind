---
id: BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE
type: business-rule
title: Ownership changes are explicit
status: draft
applies-to:
- UC-AREAS-ASSIGN-OWNER
- UC-AREAS-TRANSFER-OWNERSHIP
- UC-AREAS-ADD-SUPPORT
- UC-AREAS-REMOVE-SUPPORT
uses-terms:
- TERM-AREA
- TERM-RESPONSIBILITY-TRANSFER
provenance:
  source: "docs/_legacy/04_contexts/responsibilities.md (Purpose, Lifecycle Integrity); openspec/specs/areas/spec.md (Responsibility Transfer); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs; interview: product owner decision Q-0012 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

Ownership of an Area changes only through an explicit household action. Changing an existing Owner is a transfer and is traceable afterwards; giving an Owner to an unowned Area is plain assignment (decided: Q-0012).

## Rationale

The context exists to make cognitive ownership visible, structured and traceable (observed: docs/_legacy/04_contexts/responsibilities.md, Purpose and Lifecycle Integrity; openspec/specs/areas/spec.md, Responsibility Transfer: transfer is explicit and auditable).

## Examples

A transfer records both the previous and the new Owner (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs, TransferPrimaryOwner).

## Exceptions

How the household can see the history of transfers is not specified.
