---
id: UC-CALENDAR-READ-SUBSCRIPTION-FEED
type: use-case
title: Read a DomusMind calendar subscription
status: draft
primary-actor: ACT-EXTERNAL-CALENDAR-CLIENT
supporting-actors:
  - ACT-HOUSEHOLD-MEMBER
bounded-context: BC-CALENDAR
governed-by:
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
  - TERM-PLAN
---

## Goal

Show the DomusMind calendar information the subscription owner chose to share.

## Trigger

The external calendar client requests a member's subscription feed.

## Preconditions

- The request presents a valid subscription URL.
- The subscription is active and has not expired.

## Main Flow

1. The external calendar client requests the feed.
2. DomusMind returns the current read-only calendar feed for that subscription.
3. The external calendar client displays the feed according to its own refresh behavior.

## Alternative Flows

None recorded.

## Failure Conditions

- The subscription URL is invalid, revoked, or expired: feed access is denied.

## Postconditions

- The external calendar client can read only the information allowed by the subscription.
- The request does not create or change any DomusMind plan.
