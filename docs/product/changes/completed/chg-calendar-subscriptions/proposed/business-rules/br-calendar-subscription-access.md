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

- Any household member who can view the household timeline may create, review and revoke only their own subscriptions. A subscription scope is either the household timeline or one selected household member’s Plans. A member scope must name a current household member.
- A subscription URL grants read-only access to DomusMind Plans in its scope. The feed contains only Plan title and timing. It excludes imported Outlook entries, descriptions, locations and participants, and grants no access to other household information or permission to change a Plan.
- A subscription expires 12 months after creation. Revoking it or reaching expiry ends access. Renewal creates a new subscription with a new private URL.
- The raw private subscription URL is shown only at creation, is not stored or written to logs, and is represented in storage by a one-way verifier. The owner can see the subscription label, scope, creation and expiry times, revocation state, last access time and total successful feed access count.

## Rationale

A calendar subscription is a private sharing capability. Its owner must be able to withdraw access, and external calendar clients must not gain authority to change DomusMind plans (issue #16).

## Examples

- A member gives an external calendar client the URL for a subscription they created; the client can read the feed.
- A member revokes their subscription; subsequent requests using its URL are denied.
- A client tries to edit a plan through the feed; no edit is performed.

## Exceptions

None recorded.
