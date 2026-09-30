---
id: BR-AREAS-SAME-HOUSEHOLD-PEOPLE
type: business-rule
title: Only people of the Area's household can own or support it
status: draft
applies-to:
- UC-AREAS-ASSIGN-OWNER
- UC-AREAS-ADD-SUPPORT
uses-terms:
- TERM-AREA
- TERM-MEMBER
- TERM-HOUSEHOLD
provenance:
  source: openspec/specs/areas/spec.md (Primary Owner Assignment, Secondary Owner Assignment, Responsibility Transfer); docs/_legacy/04_contexts/responsibilities.md (Family Consistency)
  confidence: high
  recovered-from: documentation
---

## Rule

Every Owner and Support person of an Area must be an existing person of the same household as the Area; any other assignment or transfer is rejected.

## Rationale

Family defines who exists; Responsibilities defines who is accountable (observed: docs/_legacy/04_contexts/responsibilities.md, Boundaries With Other Contexts, Family Consistency; openspec/specs/areas/spec.md, Primary and Secondary Owner Assignment).

## Examples

Assigning a person from another household as Owner is rejected and the existing Owner is unchanged (observed: openspec/specs/areas/spec.md, scenarios Non-family member cannot be assigned as owner and Transfer to a non-family member is rejected).

## Exceptions

None. Note: the rule is not enforced inside the domain aggregate itself (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs); where it is checked is not visible from the domain code.
