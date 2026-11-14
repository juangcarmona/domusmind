---
id: SB-FAMILY-REVOKE-ACCESS-INVITATION
type: structured-behaviour
title: A manager revokes a pending invitation
status: active
illustrates:
  - UC-FAMILY-MANAGE-INVITATION
  - BR-FAMILY-MANAGERS-ADMINISTER
given:
  - A household person has a pending invitation.
  - The requester is a household manager.
when: The manager revokes the invitation.
then:
  - The pending link is invalidated.
  - The person returns to No Access.
  - The person remains on the household roster.
covers-transition:
  lifecycle: LC-MEMBER-ACCESS
  transition: REVOKE-INVITATION
uses-terms:
  - TERM-MEMBER-ACCESS
provenance:
  source: UC-FAMILY-MANAGE-INVITATION; FR-FAMILY-MANAGE-MEMBER-ACCESS; BR-FAMILY-MANAGERS-ADMINISTER
  confidence: high
  recovered-from: documentation
---

## Intent

Let a manager withdraw an invitation before it grants application access.

## Boundaries

Provider credentials and household membership keep their existing rules. The example adds no new access policy.
