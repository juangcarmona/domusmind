---
id: BR-CALENDAR-SUBSCRIPTION-ACCESS
type: business-rule
title: Calendar subscription access is owner-controlled and read-only
status: draft
applies-to:
  - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
  - UC-CALENDAR-READ-SUBSCRIPTION-FEED
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Rule

- A member may create, review, and revoke only subscriptions they own, and may create one only when authorized to share the selected calendar information.
- A subscription URL grants read-only access to its feed. It does not grant access to other household information or permission to change a plan.
- Revoking a subscription or reaching its expiry ends access to its feed.
- The raw private subscription URL is not stored or written to logs.

## Rationale

A calendar subscription is a private sharing capability. Its owner must be able to withdraw access, and external calendar clients must not gain authority to change DomusMind plans (issue #16).

## Examples

- A member gives an external calendar client the URL for a subscription they created; the client can read the feed.
- A member revokes their subscription; subsequent requests using its URL are denied.
- A client tries to edit a plan through the feed; no edit is performed.

## Exceptions

None recorded.
