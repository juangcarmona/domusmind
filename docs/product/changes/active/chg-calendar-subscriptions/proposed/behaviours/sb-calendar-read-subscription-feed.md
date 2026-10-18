---
id: SB-CALENDAR-READ-SUBSCRIPTION-FEED
type: structured-behaviour
title: An external calendar client reads an active subscription
status: draft
illustrates:
  - UC-CALENDAR-READ-SUBSCRIPTION-FEED
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
when: An external calendar client requests a subscription feed.
given:
  - The URL identifies an active subscription that has not been revoked or expired.
then:
  - The client receives the read-only calendar information permitted by the subscription.
  - The request does not create or change a DomusMind plan.
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Intent

Show calendar information the subscription owner has authorized to be shared.

## Boundaries

The feed's exact fields and visibility scopes remain open questions in CHG-CALENDAR-SUBSCRIPTIONS. The client cannot modify DomusMind data.
