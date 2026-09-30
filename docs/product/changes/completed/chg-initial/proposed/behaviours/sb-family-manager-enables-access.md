---
id: SB-FAMILY-MANAGER-ENABLES-ACCESS
type: structured-behaviour
title: "A manager re-enables a person's access"
status: draft
illustrates:
  - UC-FAMILY-ENABLE-MEMBER-ACCESS
given:
  - "A person has Disabled access"
  - "The requester is a manager"
when: "Access is enabled"
then:
  - "The person's access becomes Active"
uses-terms:
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning, scenario \"Manager enables a member's access\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes disabled access can be restored.

## Boundaries

None.
