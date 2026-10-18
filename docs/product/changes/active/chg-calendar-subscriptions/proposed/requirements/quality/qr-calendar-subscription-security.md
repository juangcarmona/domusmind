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

Only a person authorized to share the selected calendar information can create a subscription. The private subscription URL MUST remain secret, and an external client MUST receive no more information than that subscription permits. Revocation or expiry MUST deny subsequent feed access.

## Measurement

- Requests using an invalid, revoked, or expired subscription URL are denied.
- A person cannot create or manage another member's subscription without authorization.
- The feed contains only the information permitted by the subscription's accepted visibility rules.
- The raw private URL is not stored or logged.
