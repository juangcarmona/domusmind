---
id: FR-CALENDAR-SERVE-SUBSCRIPTION-FEED
type: functional-requirement
title: Serve a read-only calendar subscription feed
status: draft
derived-from:
  - UC-CALENDAR-READ-SUBSCRIPTION-FEED
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
verification:
  - scenario-ref: SB-CALENDAR-READ-SUBSCRIPTION-FEED
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
  - TERM-PLAN
---

## Requirement

The product MUST let an external calendar client read a valid, active ICS subscription feed. The feed MUST contain only DomusMind Plans within its household or single-member scope and expose only each Plan’s title and timing. It MUST exclude imported Outlook entries, descriptions, locations and participants. It MUST deny access when the URL is invalid, revoked or expired, and MUST NOT allow the client to change DomusMind Plans.

## Rationale

External calendar clients need a read-only projection of the DomusMind calendar information the subscription owner is authorized to share (issue #16).
