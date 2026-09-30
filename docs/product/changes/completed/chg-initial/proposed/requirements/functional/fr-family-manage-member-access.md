---
id: FR-FAMILY-MANAGE-MEMBER-ACCESS
type: functional-requirement
title: "Managers control people's access"
status: draft
derived-from:
  - UC-FAMILY-PROVISION-MEMBER-ACCESS
  - UC-FAMILY-DISABLE-MEMBER-ACCESS
  - UC-FAMILY-ENABLE-MEMBER-ACCESS
  - UC-FAMILY-RESET-MEMBER-ACCESS
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
verification:
  - scenario-ref: "SB-FAMILY-MANAGER-PROVISIONS-ACCESS"
  - scenario-ref: "SB-FAMILY-MANAGER-DISABLES-ACCESS"
  - scenario-ref: "SB-FAMILY-MANAGER-ENABLES-ACCESS"
  - scenario-ref: "SB-FAMILY-PET-ACCESS-REJECTED"
  - scenario: "A manager tries to disable their own access and the product rejects it"
  - scenario: "A manager resets another person's password and that person must change it at their next sign-in"
uses-terms:
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (MemberAccessStatus, Permission rules)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a manager provision, disable, enable and reset access for people, and MUST show each person's access as one of No Access, Invited or Provisioned, Password Reset Required, Active or Disabled. It MUST reject these actions from non-managers, MUST reject provisioning for pets, and MUST NOT let a manager disable their own access (observed: openspec/specs/family/spec.md, Member Access Provisioning).

## Rationale

Access decides who can use DomusMind; the household needs one clear authority over it.
