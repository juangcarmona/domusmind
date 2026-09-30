---
id: BR-AREAS-OWNER-NOT-SUPPORT
type: business-rule
title: The Owner of an Area is not also its Support
status: draft
applies-to:
- UC-AREAS-ASSIGN-OWNER
- UC-AREAS-ADD-SUPPORT
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: "docs/_legacy/04_contexts/responsibilities.md (Role Consistency); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (AssignPrimaryOwner, TransferPrimaryOwner); interview: product owner decision Q-0017 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

A person cannot be both Owner and Support of the same Area (decided: Q-0017). When a Support person becomes the Owner, they stop being Support, and adding the current Owner as Support is rejected.

## Rationale

The legacy context states it as an invariant, "unless explicitly allowed by model evolution" (observed: docs/_legacy/04_contexts/responsibilities.md, Role Consistency). The domain code removes the new Owner from Support on assignment and transfer (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs), but does not yet reject adding the current Owner as Support. The spec is silent; the product owner confirmed the rule (decided: Q-0017).

## Examples

Luis supports "Finances" and is assigned Owner; Luis is then Owner only (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).

## Exceptions

None.
