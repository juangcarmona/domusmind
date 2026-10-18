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

Show DomusMind Plans within the subscription’s Household or single-member scope.

## Trigger

The external calendar client requests a member's subscription feed.

## Preconditions

- The request presents a valid subscription URL.
- The subscription is active and has not expired.

## Main Flow

1. The external calendar client requests the feed.
2. DomusMind returns the current read-only feed containing Plan title and timing only.
3. The external calendar client displays the feed according to its own refresh behavior.

## Alternative Flows

None recorded.

## Failure Conditions

- The subscription URL is invalid, revoked, or expired: feed access is denied.

## Postconditions

- The external calendar client can read only Plan titles and timing for the selected scope. Imported Outlook entries, descriptions, locations and participants are excluded.
- The request does not create or change any DomusMind plan.
