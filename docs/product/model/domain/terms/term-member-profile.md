---
id: TERM-MEMBER-PROFILE
type: domain-term
title: "Person Profile"
status: draft
defined-in: BC-FAMILY
synonyms:
  - "Member Profile"
uses-terms:
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Profile Update); docs/_legacy/04_contexts/family-member-management.md (section 8 Extension seam); src/backend/DomusMind.Domain/Family/FamilyMember.cs (UpdateProfile, DisplayName); interview: product owner decision Q-0060 (E-0158); interview: product owner decision Q-0023 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Definition

The optional details a person keeps about themselves: preferred name, phone, email and a household note (observed: openspec/specs/family/spec.md, Member Profile Update). The profile also includes an avatar (icon and colour), and the preferred name, when set, is shown instead of the name (decided: Q-0060, medium confidence; observed: src/backend/DomusMind.Domain/Family/FamilyMember.cs).

## Distinguish From

- Not the core details (name, role, birth date), which only managers change, except that a person may change their own name and birth date (observed: openspec family spec, Member Core Details Update; decided: Q-0023).
- The profile email is a contact address, separate from the sign-in email (observed: FamilyMember.cs, PrimaryEmail).

## Usage

Edited by the person themselves or by any manager. Addresses, emergency information and document metadata are planned: V2 (observed: family-member-management.md, section 8).
