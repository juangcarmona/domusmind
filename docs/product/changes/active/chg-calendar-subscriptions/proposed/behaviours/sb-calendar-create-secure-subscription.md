---
id: SB-CALENDAR-CREATE-SECURE-SUBSCRIPTION
type: structured-behaviour
title: An authorized member creates a private calendar subscription
status: draft
illustrates:
  - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
when: An authorized household member creates a calendar subscription.
given:
  - The member has access to the calendar information selected for sharing.
then:
  - The member receives a private URL for the new subscription.
  - The subscription is associated with its owner.
  - The subscription provides read-only access and does not change any plan.
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Intent

Make it possible for an authorized household member to share calendar information with an external calendar client.

## Boundaries

The member receives access only to their subscription URL. The roles allowed to create subscriptions and the available visibility scopes remain open questions in CHG-CALENDAR-SUBSCRIPTIONS.
