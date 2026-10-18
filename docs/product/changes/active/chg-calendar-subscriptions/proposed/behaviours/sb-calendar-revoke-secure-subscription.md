---
id: SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
type: structured-behaviour
title: A member revokes a calendar subscription
status: draft
illustrates:
  - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
when: A member revokes a calendar subscription they own.
given:
  - The subscription is active.
then:
  - The subscription becomes unavailable to external calendar clients.
  - Later requests using its URL are denied.
  - No DomusMind plan is changed.
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Intent

Let a subscription owner withdraw the external calendar client's access.

## Boundaries

The member can revoke only subscriptions they own. The owner experience for replacing or rotating a URL is not specified by issue #16.
