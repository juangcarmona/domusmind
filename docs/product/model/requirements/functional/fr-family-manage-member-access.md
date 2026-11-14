---
id: FR-FAMILY-MANAGE-MEMBER-ACCESS
type: functional-requirement
title: Managers control people's access
status: active
derived-from:
  - UC-FAMILY-PROVISION-MEMBER-ACCESS
  - UC-FAMILY-ACCEPT-ACCESS-INVITATION
  - UC-FAMILY-MANAGE-INVITATION
  - UC-FAMILY-DISABLE-MEMBER-ACCESS
  - UC-FAMILY-ENABLE-MEMBER-ACCESS
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
verification:
  - scenario-ref: SB-FAMILY-MANAGER-PROVISIONS-ACCESS
  - scenario-ref: SB-FAMILY-MANAGER-DISABLES-ACCESS
  - scenario-ref: SB-FAMILY-MANAGER-ENABLES-ACCESS
  - scenario-ref: SB-FAMILY-PET-ACCESS-REJECTED
  - scenario: A manager tries to disable their own access and the product rejects it
  - scenario: An invited person with a mismatched or unverified provider email cannot accept
  - scenario-ref: SB-FAMILY-ACCEPT-ACCESS-INVITATION
  - scenario-ref: SB-FAMILY-REVOKE-ACCESS-INVITATION
  - scenario-ref: SB-FAMILY-REPLACE-ACCESS-INVITATION
uses-terms:
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
provenance:
  source: openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (MemberAccessStatus, Permission rules)
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a manager invite people with a copyable single-use link, replace or revoke pending links, and disable or re-enable their application access. It MUST show each person's access as No Access, Invited or Provisioned, Active or Disabled. It MUST reject these actions from non-managers, MUST reject invitations for pets, and MUST NOT let a manager disable their own access. It MUST leave provider credentials to the provider.

## Rationale

Access decides who can use DomusMind; the household needs one clear authority over it.
