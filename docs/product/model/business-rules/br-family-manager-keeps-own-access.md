---
id: BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
type: business-rule
title: "A manager cannot disable their own access"
status: draft
applies-to:
  - UC-FAMILY-DISABLE-MEMBER-ACCESS
  - UC-FAMILY-RESET-MEMBER-ACCESS
uses-terms:
  - TERM-MANAGER
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Permission rules)"
  confidence: high
  recovered-from: documentation
---

## Rule

A manager may not disable their own access (observed: openspec/specs/family/spec.md, Member Access Provisioning). The legacy permission table also forbids a manager from resetting their own password through the administrative action (observed: family-member-management.md, Permission rules: "Regenerate password | manager, not self"); the spec is silent on this second restriction.

## Rationale

Prevents a manager from locking themselves out of the household (inferred).

## Examples

- A manager disables another adult's access: allowed.
- A manager tries to disable their own access: rejected.

## Exceptions

None.
