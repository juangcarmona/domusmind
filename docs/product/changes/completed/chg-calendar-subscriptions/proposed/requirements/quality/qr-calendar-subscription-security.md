---
id: QR-CALENDAR-SUBSCRIPTION-SECURITY
type: quality-requirement
title: Calendar subscriptions protect shared calendar information
status: draft
quality-attribute: security
applies-to:
  - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
  - UC-CALENDAR-READ-SUBSCRIPTION-FEED
verification:
  - scenario-ref: SB-CALENDAR-CREATE-SECURE-SUBSCRIPTION
  - scenario-ref: SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
  - scenario-ref: SB-CALENDAR-READ-SUBSCRIPTION-FEED
uses-terms:
  - TERM-CALENDAR-SUBSCRIPTION
---

## Requirement

Only a household member who can view the household timeline can create a subscription. The private URL MUST remain secret and MUST be revealed only at creation; an external client MUST receive only Plan title and timing within the chosen scope. Revocation or the mandatory 12-month expiry MUST deny subsequent feed access.

## Measurement

- Requests using an invalid, revoked, or expired subscription URL are denied.
- A person cannot create or manage another member's subscription without authorization.
- The feed contains only Plan title and timing for the chosen Household or single-member scope.
- The raw private URL is not stored or logged.
