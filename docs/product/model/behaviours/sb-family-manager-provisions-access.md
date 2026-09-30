---
id: SB-FAMILY-MANAGER-PROVISIONS-ACCESS
type: structured-behaviour
title: "A manager gives a person access"
status: draft
illustrates:
  - UC-FAMILY-PROVISION-MEMBER-ACCESS
given:
  - "A person has No Access"
  - "The requester is a manager"
  - "The person is not a pet"
when: "Access is provisioned"
then:
  - "The person is linked to a sign-in account"
  - "The person's access becomes Invited or Provisioned"
uses-terms:
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning, scenario \"Manager provisions access for a member\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes how people start using DomusMind.

## Boundaries

None.
