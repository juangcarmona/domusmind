---
id: UC-CALENDAR-MANAGE-SUBSCRIPTIONS
type: use-case
title: Manage calendar subscriptions
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-CALENDAR
governed-by:
  - BR-CALENDAR-SUBSCRIPTION-ACCESS
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Goal

Share DomusMind calendar information with an external calendar client while retaining control over access.

## Trigger

The member wants to see DomusMind calendar information in another calendar application.

## Preconditions

- The person is authenticated as a household member.
- The person is authorized to share the calendar information selected for the subscription; the required role and content scope remain open questions in this Product Change.

## Main Flow

1. The member creates a calendar subscription.
2. DomusMind provides a private subscription URL.
3. The member adds the URL to an external calendar client.
4. The member can review and revoke subscriptions they own.

## Alternative Flows

- The member may create more than one subscription for separate external calendar clients; the issue describes one subscription as representing one external calendar.

## Failure Conditions

- An unauthenticated person or a person without the required household access attempts to create or manage a subscription: access is denied.
- The member attempts to manage another person's subscription: access is denied.

## Postconditions

- A created subscription grants read-only access to its calendar feed until it is revoked or expires.
- Revoking a subscription prevents further feed access.
- No plan is created or changed by creating, reading, or revoking a subscription.
