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

Whether and how a person can sign in to DomusMind. Access links a person to a sign-in account and is always in one of five states:
- No Access: no account is linked.
- Invited or Provisioned: an account exists, a password change is required and the person has never signed in.
- Password Reset Required: an account exists, a password change is required and the person has signed in before.
- Active: an account exists, is not disabled and needs no forced password change.
- Disabled: an account exists but is disabled.
(observed: openspec/specs/family/spec.md, Member Access Provisioning; family-member-management.md, MemberAccessStatus.)

## Distinguish From

- Not the person's membership: a person with no access is still fully part of the household.
- Not the sign-in account itself, which lies outside this area (observed: openspec family spec, NOTE N7).

## Usage

Managed by managers: provision, disable, enable and reset (observed: openspec family spec). Shown in the directory next to each person (observed: family-member-management.md, MemberDirectoryItemResponse).
