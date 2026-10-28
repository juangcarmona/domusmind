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
when: "The manager invites the person using their email"
then:
  - "A copyable single-use invitation link is tied to that person and email"
  - "No provider identity is linked until the person accepts with a matching verified email"
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
