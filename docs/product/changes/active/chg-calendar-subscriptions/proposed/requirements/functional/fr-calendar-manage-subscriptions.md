---
id: FR-CALENDAR-MANAGE-SUBSCRIPTIONS
type: functional-requirement
title: Manage private calendar subscriptions
status: draft
derived-from:
  - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
verification:
  - scenario-ref: SB-CALENDAR-CREATE-SECURE-SUBSCRIPTION
  - scenario-ref: SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Requirement

The product MUST let an authorized household member create a private calendar subscription, review subscriptions they own, and revoke one. A revoked subscription MUST no longer provide feed access.

## Rationale

Members need to share DomusMind calendar information with their external calendar clients while retaining control over access (issue #16).
