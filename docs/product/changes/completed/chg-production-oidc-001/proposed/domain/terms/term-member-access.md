---
id: TERM-MEMBER-ACCESS
type: domain-term
title: "Person Access"
status: draft
defined-in: BC-FAMILY
synonyms:
  - "Member Access"
  - "Access Status"
uses-terms:
  - TERM-MEMBER
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (MemberAccessStatus 5-state model); src/backend/DomusMind.Domain/Family/Family.cs (LinkMemberAccount)"
  confidence: high
  recovered-from: documentation
---

## Definition

Whether a household person may use DomusMind. Access is separate from provider credentials and has four states:
- No Access: no invitation or provider identity is bound to the person.
- Invited or Provisioned: a single-use invitation link is pending for the person's email; no provider identity is bound yet.
- Active: a provider identity is bound and application access is enabled.
- Disabled: a provider identity remains bound but application access is denied.

## Distinguish From

- Not the person's membership: a person with no access is still fully part of the household.
- Not the provider's sign-in account or credentials, which remain outside the household domain.

## Usage

Managed by managers through invitations and application access. Shown in the directory next to each person.
