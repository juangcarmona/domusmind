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

The product MUST let any household member who can view the household timeline create a private subscription for either the household or one selected member’s Plans, list subscriptions they own, and revoke one. Each subscription MUST have a member-provided label, expire 12 months after creation, and be renewable by creating a new subscription. The owner MUST be able to see each subscription’s scope, expiry, last successful access time and successful access count. The private URL MUST be revealed only at creation. A revoked or expired subscription MUST no longer provide feed access.

## Rationale

Members need to share DomusMind calendar information with their external calendar clients while retaining control over access (issue #16).
