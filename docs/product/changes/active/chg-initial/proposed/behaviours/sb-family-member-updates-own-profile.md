---
id: SB-FAMILY-MEMBER-UPDATES-OWN-PROFILE
type: structured-behaviour
title: "A person updates their own profile"
status: draft
illustrates:
  - UC-FAMILY-UPDATE-MEMBER-PROFILE
given:
  - "A household member is signed in"
when: "That person updates their own preferred name, phone, email or household note"
then:
  - "The profile is updated"
uses-terms:
  - TERM-MEMBER-PROFILE
provenance:
  source: "openspec/specs/family/spec.md (Member Profile Update, scenario \"Member updates their own profile\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes people maintain their own contact details without a manager.

## Boundaries

None.
