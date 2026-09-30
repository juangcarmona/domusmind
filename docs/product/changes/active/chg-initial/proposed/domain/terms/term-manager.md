---
id: TERM-MANAGER
type: domain-term
title: "Manager"
status: draft
defined-in: BC-FAMILY
synonyms:
  - "Household Manager"
uses-terms:
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Purpose, NOTES N3-N4, Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Permission rules); src/backend/DomusMind.Domain/Family/Family.cs (manager must be adult); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0023 (E-0158); interview: product owner decision Q-0024 (E-0158); interview: product owner decision Q-0025 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

A designation on a person that grants authority over the household's administration: adding people and pets, removing them, managing relationships, changing household settings, editing other people's core details and anyone's role, and provisioning, disabling, enabling or resetting their access (observed: openspec/specs/family/spec.md, Purpose, Member Core Details Update, Member Access Provisioning; decided: Q-0023, Q-0024, Q-0025).

## Distinguish From

- Not a person role: it is a flag held in addition to the role (observed: openspec family spec, Purpose). Only adults can be managers (decided: Q-0022; observed: src/backend/DomusMind.Domain/Family/Family.cs).
- Not the V1.1 "Household Role" permission scale (TERM-HOUSEHOLD-ROLE), in which Manager would be one of several levels (observed: family-member-management.md, HouseholdRole).

## Usage

The Household Manager actor is a person holding this designation. The person who creates the household is its first manager, and the designation is set when adding or editing an adult (decided: Q-0022). Whether the designation can be granted or revoked on its own, outside adding or editing an adult, is not specified (observed: openspec family spec, NOTE N4). Whether a household must always keep a manager is also open (observed: NOTE N3).
