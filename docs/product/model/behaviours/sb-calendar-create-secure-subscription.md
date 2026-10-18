---
id: SB-CALENDAR-CREATE-SECURE-SUBSCRIPTION
type: structured-behaviour
title: An authorized member creates a private calendar subscription
status: draft
illustrates:
  - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
when: A household member with timeline access creates a labeled Household or single-member subscription.
given:
  - The member can view the household timeline and, for a single-member scope, chooses a current household member.
then:
  - The member receives a private URL for the new subscription.
  - The subscription is associated with its owner.
  - The subscription expires in 12 months, provides read-only access to Plan titles and timing only, and does not change any Plan.
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Intent

Make it possible for an authorized household member to share calendar information with an external calendar client.

## Boundaries

The member sees the private URL once. They can later review its label, scope, expiry, last access time and access count. The scope is Household or one household member; imported Outlook entries and Plan descriptions, locations and participants are never included.
