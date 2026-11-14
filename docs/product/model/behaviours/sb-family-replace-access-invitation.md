---
id: SB-FAMILY-REPLACE-ACCESS-INVITATION
type: structured-behaviour
title: A manager replaces a pending invitation link
status: active
illustrates:
  - UC-FAMILY-MANAGE-INVITATION
  - BR-FAMILY-MANAGERS-ADMINISTER
given:
  - A household person has a pending invitation tied to their email.
  - The requester is a household manager.
when: The manager replaces the pending invitation.
then:
  - The old invitation link is invalidated.
  - A new single-use link for the same person and email is available to copy and share.
  - The person remains Invited.
uses-terms:
  - TERM-MEMBER-ACCESS
provenance:
  source: UC-FAMILY-MANAGE-INVITATION; FR-FAMILY-MANAGE-MEMBER-ACCESS; BR-FAMILY-MANAGERS-ADMINISTER
  confidence: high
  recovered-from: documentation
---

## Intent

Give the manager a fresh invitation link while preserving the intended recipient.

## Boundaries

Provider credentials and household membership keep their existing rules. The example adds no new access policy.
