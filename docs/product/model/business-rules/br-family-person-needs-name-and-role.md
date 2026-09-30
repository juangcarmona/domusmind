---
id: BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
type: business-rule
title: "A person needs a name and a valid role"
status: draft
applies-to:
  - UC-FAMILY-ADD-MEMBER
  - UC-FAMILY-REGISTER-PET
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
uses-terms:
  - TERM-MEMBER
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Member Addition); src/backend/DomusMind.Domain/Family/ValueObjects/MemberRole.cs, MemberName.cs; interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

Every person added to a household must have a name and one role from the allowed set: Adult, Child or Pet (decided: Q-0021; observed: src/backend/DomusMind.Domain/Family/ValueObjects/MemberRole.cs). A role outside the set is rejected. The code limits names to 100 characters (observed: src/backend/DomusMind.Domain/Family/ValueObjects/MemberName.cs).

The specs also listed a Caregiver role; there is no Caregiver role in the product (decided: Q-0021).

## Rationale

The role decides how the person takes part in the rest of the product (observed: openspec family spec, Pet Registration).

## Examples

- "Lucia, Child" is accepted.
- "Rex, Robot" is rejected with a validation error (observed: scenario "Member addition fails with an invalid role").

## Exceptions

None.
