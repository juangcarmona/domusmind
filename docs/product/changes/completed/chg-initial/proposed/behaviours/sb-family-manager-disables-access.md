---
id: SB-FAMILY-MANAGER-DISABLES-ACCESS
type: structured-behaviour
title: "A manager disables a person's access"
status: draft
illustrates:
  - UC-FAMILY-DISABLE-MEMBER-ACCESS
given:
  - "A person has Active access"
  - "The requester is a manager"
  - "The person is not the requesting manager"
when: "Access is disabled"
then:
  - "The person's access becomes Disabled"
uses-terms:
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning, scenario \"Manager disables a member's access\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes access can be suspended without removing the person.

## Boundaries

None.
