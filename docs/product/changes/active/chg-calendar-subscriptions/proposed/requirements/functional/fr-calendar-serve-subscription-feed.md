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

The product MUST let an external calendar client read the calendar feed granted by a valid, active subscription URL. It MUST deny access when the URL is invalid, revoked, or expired, and the feed MUST NOT allow the client to change DomusMind plans.

## Rationale

External calendar clients need a read-only projection of the DomusMind calendar information the subscription owner is authorized to share (issue #16).
