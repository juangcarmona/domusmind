---
id: BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE
type: business-rule
title: "Only Household and Members changes who belongs"
status: draft
applies-to:
  - BC-FAMILY
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Identity Boundary Enforcement, Member Removal); docs/_legacy/04_contexts/family.md (Ownership Boundary, Boundaries With Other Contexts)"
  confidence: high
  recovered-from: documentation
---

## Rule

No other part of the product may create, change or remove people, pets or relationships. Other areas refer to them by identity only and react to household changes after they happen (observed: openspec/specs/family/spec.md, Identity Boundary Enforcement; docs/_legacy/04_contexts/family.md, Ownership Boundary).

## Rationale

Keeps one consistent answer to "who belongs to this household" across plans, tasks, areas and lists (observed: family.md, Identity Ownership Rule).

## Examples

- Assigning a task to a person does not change the person.
- Removing a person (planned: V1.1) does not remove their tasks or plans automatically; those areas handle it themselves (observed: openspec family spec, Member Removal).

## Exceptions

None.
