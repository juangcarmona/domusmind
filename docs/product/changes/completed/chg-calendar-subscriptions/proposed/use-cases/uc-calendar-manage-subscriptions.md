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
- The member can view the household timeline. They may choose either the household timeline or one household member’s plans.

## Main Flow

1. The member selects a label and either Household scope or one household member.
2. DomusMind creates a subscription that expires 12 months after creation.
3. DomusMind provides the private subscription URL once and lists the subscription metadata.
4. The member adds the URL to an external calendar client.
5. The member can review each owned subscription’s label, scope, expiry, last access time and access count, and revoke it.

## Alternative Flows

- The member may create multiple subscriptions for separate external calendar clients.

## Failure Conditions

- An unauthenticated person or a person without the required household access attempts to create or manage a subscription: access is denied.
- The member attempts to manage another person's subscription: access is denied.

## Postconditions

- A created subscription grants read-only access to its calendar feed until it is revoked or expires.
- Revoking a subscription prevents further feed access.
- No plan is created or changed by creating, reading, or revoking a subscription.
